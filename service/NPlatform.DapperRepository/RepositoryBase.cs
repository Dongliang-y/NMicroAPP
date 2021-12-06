/***********************************************************
**项目名称:
**功能描述: 仓储  的摘要说明
**作    者:   易栋梁
**版 本 号:    1.0
**创建日期： 2015/12/7 16:06:56
**修改历史：
************************************************************/

namespace NPlatform.Repositories
{
    using DapperExtensions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using MySql.Data.MySqlClient;
    using NPlatform.Domains.Entity;
    using NPlatform.Domains.IRepositories;
    using NPlatform.Filters;
    using NPlatform.Infrastructure.Config;
    using NPlatform.Repositories.IRepositories;
    using NPlatform.Result;
    using ServiceStack;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 聚合仓储基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public  class RepositoryBase<TEntity, TPrimaryKey> : ResultBase, IRepository<TEntity, TPrimaryKey>
        where TEntity : EntityBase<TPrimaryKey>
    {

        public ILogger loggerSvc;
        public IConfiguration Config;

        private IRepositoryOptions _Options;
        private DPContext DBMain { get; set; }
        private DPContext DBMinor { get; set; }

        /// <summary>
        /// 仓储配置
        /// </summary>
        public IRepositoryOptions Options
        {
            get { return _Options; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{TEntity,TPrimaryKey}"/> class. 
        /// 仓储基类
        /// </summary>
        /// <param name="option">
        /// 仓储配置
        /// </param>
        public RepositoryBase(IRepositoryOptions option)
        {
            _Options = option;
            var serverConfig = Config.GetServiceConfig();
            DBProvider provider;
            if (Enum.TryParse(serverConfig.DBProvider, out provider))
            {
                DBMain = new DPContext(option.MainConection, option.DBProvider, (int)option.TimeOut);
                if(!serverConfig.MinorConnection.IsNullOrEmpty())
                {
                    DBMinor = new DPContext(serverConfig.MinorConnection, provider, (int)option.TimeOut);
                }
            }
        }


        #region 增删改
        /// <summary>
        /// 实现[]操作
        /// </summary>
        /// <param name="key">对象的Id</param>
        /// <returns>对象</returns>
        public virtual TEntity this[TPrimaryKey key]
        {
            get
            {
                return DBMain.Get<TEntity>(key);
            }

            set
            {
                if (this.DBMinor != null)
                {
                    this.DBMinor.Update<TEntity>(value);
                }
                else
                {
                    this.DBMain.Update(value);
                }
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item">新增对象</param>
        /// <returns>新增后创建了Id 的对象。</returns>
        public virtual async Task<TEntity> AddAsync(TEntity item)
        {
            this.SetFilter(item);
            return await this.CUDContext.InsertAsync<TEntity>(item);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="items">新增对象的集合</param>
        public virtual async Task<int> AddsAsync(IEnumerable<TEntity> items)
        {
            foreach (var item in items)
            {
                // 实体如果实现了过滤器， 那么仓储就可以拿注入进来的过滤器对实体进行设置与过滤。
                this.SetFilter(item);
            }
            int rst = -1;

            var trans = this.CUDContext.BeginTransaction();
            try
            {
                rst = await this.CUDContext.InsertsAsync(items, trans);
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            trans.Dispose();
            return rst;

        }

        /// <summary>
        /// 异步新增
        /// </summary>
        /// <param name="items">新增对象的集合</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
        public async Task<int> AddOrUpdate(TEntity item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var ext = await this.ExistsAsync(item.Id);
            if (ext)
            {
                return await this.CUDContext.UpdateAsync(item) ? 1 : 0;
            }
            else
            {
                this.SetFilter(item);
                return await this.CUDContext.InsertAsync(item);
            }
        }

        /// <summary>
        /// create update delete
        /// </summary>
        private DPContext CUDContext
        {
            get
            {
                return this.DBMinor != null ? this.DBMinor : DBMain;
            }
        }
        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="entity">对象</param>
        /// <returns></returns>
        public async Task<int> RemoveAsync(TEntity entity)
        {
            this.loggerSvc.LogTrace("删除了{0}数据ID：{1}", entity.GetType().Name, entity.Id.ToString());
            var enabled = this.Options.QueryFilters.ContainsKey(nameof(LogicDeleteFilter));

            if (typeof(ILogicDelete).IsAssignableFrom(typeof(TEntity)) && enabled)
            {
                ((ILogicDelete)entity).IsDeleted = true;

                return await CUDContext.UpdateAsync(entity) ? 1 : 0;
            }
            else
            {
                return await CUDContext.DeleteAsync<TEntity>(entity);
            }
        }

        /// <summary>
        /// 键值删除
        /// </summary>
        public virtual async Task<int> RemoveAsync(params TPrimaryKey[] keys)
        {
            if (keys == null)
            {
                throw new ArgumentEmptyException(nameof(keys));
            }

            if (keys.Length == 0)
            {
                throw new ArgumentException("Argument is empty collection", nameof(keys));
            }

            Expression<Func<TEntity, bool>> exp = t => keys.Contains(t.Id);

            return await this.RemoveAsync(exp);
        }

        /// <summary>
        /// 条件删除
        /// </summary>
        /// <param name="filter">删除条件</param>
        /// <returns>移除结果</returns>
        public async Task<int> RemoveAsync(Expression<Func<TEntity, bool>> filter)
        {
            var enabled = this.Options.QueryFilters.ContainsKey(nameof(LogicDeleteFilter));
            if (typeof(ILogicDelete).IsAssignableFrom(typeof(TEntity)) && enabled)
            {
                using (var unitwork = new UnitOfWork(Options))
                {
                    try
                    {
                        var entitys = await this.GetListByExpAsync(filter);
                        foreach (var entity in entitys)
                        {
                            ((ILogicDelete)entity).IsDeleted = true;
                            await unitwork.ChangeAsync(entity);
                        }
                        var keys = entitys.Select(t => t.Id);
                        var ids = string.Join(",", keys);
                        loggerSvc.LogTrace("删除{0}的数据：{1}，", typeof(TEntity).Name, keys);
                        unitwork.Commit();
                        return ids.Length;
                    }
                    catch
                    {
                        unitwork.Rollback();
                        throw;
                    }
                }
            }
            else
            {
                var predicate = QueryBuilder<TEntity>.FromExpression(filter);

                loggerSvc.LogTrace("删除{0}的数据：{1}，", typeof(TEntity).Name, filter.ToString());
                return await CUDContext.DeleteAsync<TEntity>(predicate);
            }
        }
        #endregion

        #region 查询

        /// <summary>
        /// 判断对象是否已存在
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>是否存在</returns>
        public virtual async Task<bool> ExistsAsync(TPrimaryKey key)
        {
            if (EqualityComparer<TPrimaryKey>.Default.Equals(key, default(TPrimaryKey)))
            {
                return false;
            }
            return await this.GetFirstOrDefaultAsync(t=>t.Id.Equals(key)) != null;
        }

        /// <summary>
        /// 数据是否存在
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>返回结果</returns>
        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
        {
            // 应用过滤器
            foreach (var ft in this.Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(exp);
                }
            }

            var rst= await this.GetListByExpAsync(filter);
            return rst.Any();
        }

        /// <summary>
        /// 查询所有数据，注意这是个异步方法。
        /// </summary>
        /// <param name="sorts">排序字段</param>
        /// <returns>集合</returns>
        public async virtual Task<IEnumerable<TEntity>> GetAllAsync(IEnumerable<SelectSort> sorts = null)
        {
            IList<ISort> dapperSorts = null;
            if (sorts != null)
            {
                dapperSorts = sorts.Select(t => new Sort { Ascending = t.Ascending, PropertyName = t.PropertyName })
                    .ToArray();
            }

            if (this.Options.QueryFilters.Count > 0)
            {
                Expression<Func<TEntity, bool>> filter = x => !x.Id.Equals(default(TPrimaryKey));

                foreach (var ft in this.Options.QueryFilters)
                {
                    var exp = ft.Value.GetFilter<TEntity>();
                    if (exp != null)
                    {
                        filter = filter.AndAlso(exp);
                    }
                }

                var predicate = QueryBuilder<TEntity>.FromExpression(filter);
                var qResult = await this.DBMain.GetListAsync<TEntity>(predicate, dapperSorts);

                return qResult;
            }
            else
            {
                return await this.DBMain.GetListAsync<TEntity>(null, dapperSorts);
            }
        }


        /// <summary>
        /// 从仓储查找对象
        /// </summary>
        /// <param name="key">主键字段</param>
        /// <returns>对象</returns>
        public virtual async Task<TEntity> FindByAsync(TPrimaryKey key)
        {
            if (this.Options.QueryFilters.Count > 0)
            {
                Expression<Func<TEntity, bool>> filter = x => x.Id.Equals((object)key);
                foreach (var ft in this.Options.QueryFilters)
                {
                    var exp = ft.Value.GetFilter<TEntity>();
                    if (exp != null)
                    {
                        filter = filter.AndAlso(exp);
                    }
                }

                var predicate = QueryBuilder<TEntity>.FromExpression(filter);

                    return (await this.DBMain.GetListAsync<TEntity>(predicate)).FirstOrDefault();
            }
            else
            {
                    return await DBMain.GetAsync<TEntity>(key);
            }
        }

        /// <summary>
        /// 查询单个对象
        /// </summary>
        /// <param name="filter">筛选条件</param>
        public virtual async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
        {
            // 应用过滤器
            foreach (var ft in this.Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(ft.Value.GetFilter<TEntity>());
                }
            }

            var predicate = QueryBuilder<TEntity>.FromExpression(filter);

            var qResult = await this.DBMain.GetListAsync<TEntity>(predicate);
            return qResult.FirstOrDefault();
        }


        /// <summary>
        /// 筛选数据
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <param name="sorts">排序字段</param>
        /// <returns>实体集合</returns>
        public virtual async Task<IEnumerable<TEntity>> GetListByExpAsync(
            Expression<Func<TEntity, bool>> filter,
            IEnumerable<SelectSort> sorts = null)
        {
            // 应用过滤器
            foreach (var ft in this.Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(exp);
                }
            }
            var predicate = QueryBuilder<TEntity>.FromExpression(filter);
            IList<ISort> dapperSorts = null;
            if (sorts != null)
            {
                dapperSorts = sorts.Select(t => new Sort { Ascending = t.Ascending, PropertyName = t.PropertyName })
                    .ToArray();
            }
            var result=await this.DBMain.GetListAsync<TEntity>(predicate, dapperSorts);
            return result;
        }
        /// <summary>
        /// 筛选数据
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <param name="sorts">排序字段</param>
        /// <returns>实体集合</returns>
        public virtual async Task<IEnumerable<TEntity>> GetListByExpAsync<T1>(
            Expression<Func<TEntity, bool>> filter,
            IEnumerable<SelectSort> sorts = null)
        {
            // 应用过滤器
            foreach (var ft in this.Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(exp);
                }
            }
            var predicate = QueryBuilder<TEntity>.FromExpression(filter);
            IList<ISort> dapperSorts = null;
            if (sorts != null)
            {
                dapperSorts = sorts.Select(t => new Sort { Ascending = t.Ascending, PropertyName = t.PropertyName })
                    .ToArray();
            }
            var result = await this.DBMain.GetListAsync<TEntity>(predicate, dapperSorts);
            return result;
        }

        /// <summary>
        /// 指定字段范围查询，返回的实体只有这几个字段有值，目的是为了避免字段多时全字段查询（select *）
        /// </summary>
        /// <param name="columnNames">需要指定查询的字段</param>
        /// <param name="filter">筛选条件</param>
        /// <param name="sorts">排序字段</param>
        /// <returns>实体集合</returns>
        public virtual async Task<IEnumerable<TEntity>> GetListWithColumnsAsync(IEnumerable<string> columnNames,
            Expression<Func<TEntity, bool>> filter,
            IEnumerable<SelectSort> sorts = null)
        {
            // 应用过滤器
            foreach (var ft in this.Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(exp);
                }
            }
            var predicate = QueryBuilder<TEntity>.FromExpression(filter);
            IList<ISort> dapperSorts = null;
            if (sorts != null)
            {
                dapperSorts = sorts.Select(t => new Sort { Ascending = t.Ascending, PropertyName = t.PropertyName })
                    .ToArray();
            }

            var result = await DBMain.GetListWithColumnsAsync<TEntity>(columnNames, predicate, dapperSorts);
            return result;
        }

        /// <summary>
        /// 分页查询对象集合,起始页码0
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="filter">数据筛选</param>
        /// <param name="sorts">排序字段</param>
        /// <returns>实体集合</returns>
        public virtual async Task<IListResult<TEntity>> GetPagedAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<TEntity, bool>> filter,
            IEnumerable<SelectSort> sorts)
        {
            // 应用过滤器
            foreach (var ft in this.Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(ft.Value.GetFilter<TEntity>());
                }
            }

            var predicate = QueryBuilder<TEntity>.FromExpression(filter);
            IList<ISort> dapperSorts = null;
            if (sorts != null)
            {
                dapperSorts = sorts.Select(t => new Sort { Ascending = t.Ascending, PropertyName = t.PropertyName })
                    .ToArray();
            }
            else
            {
                dapperSorts = new List<ISort> { new Sort { Ascending = false, PropertyName = "Id" } };
            }
                var listPage =await this.DBMain.GetPageAsync<TEntity>(predicate, dapperSorts, pageIndex, pageSize);
                return new ListResult<TEntity>(listPage.Item1, listPage.Item2);
        }

        /// <summary>
        /// 设置实体的过滤器属性
        /// </summary>
        /// <param name="item">实体</param>
        private void SetFilter(TEntity item)
        {
            // 实体如果实现了过滤器， 那么仓储就可以拿注入进来的过滤器对实体进行设置与过滤。
            if (typeof(IFilter).IsAssignableFrom(typeof(TEntity)))
            {
                foreach (var filter in this.Options.QueryFilters)
                {
                    filter.Value.SetFilterProperty(item); // 设置过滤器
                }
            }
        }

        /// <summary>
        /// 对已存在的数据应用过滤器过滤。注意这属于事后过滤。sql级别过滤属于事前过滤。
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="datas">需要过滤的数据。</param>
        /// <returns></returns>
        protected IEnumerable<T> Filter<T>(IEnumerable<T> datas) where T : IEntity
        {
            FilterManager filterManager = new FilterManager(this.Options);
            return filterManager.ResultFilter(datas);
        }
        #endregion

        #region 统计
        /// <summary>
        /// 统计记录数
        /// </summary>
        /// <param name="filter">筛选条件</param>
        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter)
        {
            // 应用过滤器
            foreach (var ft in this.Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(exp);
                }
            }

            var predicate = QueryBuilder<TEntity>.FromExpression(filter);
            return await CUDContext.CountAsync<TEntity>(predicate);
        }

        /// <summary>
        /// 求最大值
        /// </summary>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="attrName">属性名</param>
        /// <param name="filter">过滤条件</param>
        /// <returns>最大值</returns>
        public async Task<TValue> MaxAsync<TValue>(Expression<Func<TEntity, TValue>> selector, Expression<Func<TEntity, bool>> filter = null)
        {
            // 应用过滤器
            foreach (var ft in this.Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(exp);
                }
            }

            var predicate = QueryBuilder<TEntity>.FromExpression(filter);
            var maxVal = await CUDContext.MaxAsync<TValue, TEntity>(selector.Name, predicate);
            return maxVal;
        }

        /// <summary>
        /// 求最小值
        /// </summary>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="attrName">属性名</param>
        /// <param name="filter">过滤条件</param>
        /// <returns>最小值</returns>
        public async Task<TValue> MinAsync<TValue>(Expression<Func<TEntity, TValue>> selector, Expression<Func<TEntity, bool>> filter = null)
        {
            // 应用过滤器
            foreach (var ft in this.Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(exp);
                }
            }

            var predicate = QueryBuilder<TEntity>.FromExpression(filter);
            var maxVal = await CUDContext.MinAsync<TValue, TEntity>(selector.Name, predicate);
            return maxVal;
        }

        /// <summary>
        /// 求和
        /// </summary>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="attrName">属性名</param>
        /// <param name="filter">过滤条件</param>
        /// <returns>和</returns>
        public async Task<decimal> SumAsync(Expression<Func<TEntity, decimal>> selector, Expression<Func<TEntity, bool>> filter = null)
        {
            // 应用过滤器
            foreach (var ft in this.Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(exp);
                }
            }
            var predicate = QueryBuilder<TEntity>.FromExpression(filter);
                var sumVal = await CUDContext.SumAsync<decimal, TEntity>(selector.Name, predicate);
                return sumVal;
        }
        /// <summary>
        /// 求平均值
        /// </summary>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="attrName">属性名</param>
        /// <param name="filter">过滤条件</param>
        /// <returns>平均值</returns>
        public async Task<decimal> AVGAsync(Expression<Func<TEntity, decimal>> selector, Expression<Func<TEntity, bool>> filter = null)
        {
            // 应用过滤器
            foreach (var ft in this.Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(exp);
                }
            }
            
            var predicate = QueryBuilder<TEntity>.FromExpression(filter);
                var avgVal =await CUDContext.AVGAsync<decimal, TEntity>(selector.Name, predicate);
                return avgVal;
        }
        #endregion

        #region 批量导入

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="dt">要导入的数据表，注意列头要和数据库匹配</param>
        /// <returns></returns>
        public int BulkLoad(DataTable table)
        {

            var columns = table.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList();

            var cacheFileInfo = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), table.TableName + ".csv");

            var file = new System.IO.FileInfo(cacheFileInfo);
            if (!file.Directory.Exists)
            {
                try
                {
                    file.Directory.Create();
                }
                catch (System.IO.IOException ex)
                {
                    throw new NPlatformException("导入失败，无法创建缓存目录！", ex, "500");
                }
                catch (UnauthorizedAccessException ex)
                {
                    throw new NPlatformException("导入失败，无法创建缓存目录！", ex, "500");
                }
            }
            string csv = DataTableToCsv(table);
            File.WriteAllText(cacheFileInfo, csv);

            MySqlBulkLoader bulk = new MySqlBulkLoader((MySqlConnection)((IDbConnection)this.CUDContext))
            {
                FieldTerminator = ",",
                FieldQuotationCharacter = '"',
                EscapeCharacter = '"',
                LineTerminator = "\r\n",
                FileName = file.FullName,
                NumberOfLinesToSkip = 0,
                TableName = table.TableName,
                Local = true
            };

            bulk.Columns.AddRange(columns);
            return bulk.Load();
        }

        ///将DataTable转换为标准的CSV  
        /// </summary>  
        /// <param name="table">数据表</param>  
        /// <returns>返回标准的CSV</returns>  
        private static string DataTableToCsv(DataTable table)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。  
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。  
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。  
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    colum = table.Columns[i];
                    if (i != 0) sb.Append(",");
                    var value = row[colum].ToStrNoNull();

                    if (colum.DataType == typeof(string))
                    {
                        if (value.Contains("\"") || value.Contains(","))
                        {
                            value = "\"" + value.Replace("\"", "\"\"") + "\"";
                        }
                        sb.Append(value);
                    }
                    else sb.Append(row[colum].ToString());
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
        #endregion
    }
}
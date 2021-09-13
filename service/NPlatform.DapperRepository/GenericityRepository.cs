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
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Dapper;

    using DapperExtensions;
    using ServiceStack;
    using NPlatform.Config;
    using NPlatform.Domains.Entity;
    using NPlatform.Domains.IRepositories;
    using NPlatform.Domains.Service;
    using NPlatform.Filters;
    using NPlatform.IOC;
    using NPlatform.Repositories.DapperExt;
    using NPlatform.Result;

    /// <summary>
    /// 聚合仓储基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public  class GenericityRepository<TEntity, TPrimaryKey> : ResultBase, IRepositories<TEntity, TPrimaryKey>
        where TEntity : EntityBase<TPrimaryKey>
    {
        private IRepositoryOptions _Options;
        private ILogerService logerSvc;

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregationRepository{TEntity,TPrimaryKey}"/> class. 
        /// 仓储基类
        /// </summary>
        /// <param name="option">
        /// 仓储配置
        /// </param>
        public GenericityRepository(IRepositoryOptions option)
        {
            _Options = option;
            logerSvc = IOCManager.BuildService<ILogerService>();
        }

        /// <summary>
        /// 仓储配置
        /// </summary>
        public IRepositoryOptions Options
        {
            get { return _Options; }
        }

        /// <summary>
        /// 创建上下文
        /// </summary>
        /// <returns></returns>
        private IDbConnection CreateDbContent()
        {
            if (!this.Options.ConnectSring.IsNullOrEmpty() && this.Options.DBProvider != default(DBProvider))
            {
                return new EPDBContext(this.Options.ConnectSring, this.Options.DBProvider);
            }
            else
            {
                return new EPDBContext(this.Options.ConnectName);
            }
        }

        /// <summary>
        /// 实现[]操作
        /// </summary>
        /// <param name="key">对象的Id</param>
        /// <returns>对象</returns>
        public virtual TEntity this[TPrimaryKey key]
        {
            get
            {
                return FindBy(key);
            }

            set
            {
                using (IDbConnection rsp = this.CreateDbContent())
                {
                    rsp.Update<TEntity>(value);
                }
            }
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item">新增对象</param>
        /// <returns>新增后创建了Id 的对象。</returns>
        public virtual TEntity Add(TEntity item)
        {
            this.SetFilter(item);

            using (var rsp = this.CreateDbContent())
            {
                var rst = rsp.Insert<TEntity>(item);
                return item;
            }
        }

        /// <summary>
        /// 新增或更新对象
        /// </summary>
        /// <param name="item">新增/修改对象</param>
        /// <returns>操作完成后的对象</returns>
        public virtual TEntity AddOrUpdate(TEntity item)
        {
            if (this.Exists(item.Id))
            {
                this[item.Id] = item;
            }
            else
            {
                this.SetFilter(item);

                this.Add(item);
            }

            return item; 
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="items">新增对象的集合</param>
        public virtual int Adds(IEnumerable<TEntity> items)
        {
            foreach (var item in items)
            {
                // 实体如果实现了过滤器， 那么仓储就可以拿注入进来的过滤器对实体进行设置与过滤。
                this.SetFilter(item);
            }

            using (var rsp = this.CreateDbContent())
            {
                var trans = rsp.BeginTransaction();
                try
                {
                    int rst=  rsp.Inserts(items, trans);
                    trans.Commit();
                    return rst;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 异步新增
        /// </summary>
        /// <param name="items">新增对象的集合</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
        public async Task<int> AddsAsync(IEnumerable<TEntity> items)
        {
            return await Task.Run(
                       () =>
                           {
                               return this.Adds(items);
                           });
        }

        /// <summary>
        /// 判断对象是否已存在
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>是否存在</returns>
        public virtual bool  Exists(TPrimaryKey key)
        {
            if (EqualityComparer<TPrimaryKey>.Default.Equals(key, default(TPrimaryKey)))
            {
                return false;
            }
            return this.FindBy(key) != null;
        }

        /// <summary>
        /// 数据是否存在
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>返回结果</returns>
        public virtual bool Exists(Expression<Func<TEntity, bool>> filter)
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

            return this.GetListByExp(filter).Any();
        }

        /// <summary>
        /// 从仓储查找对象
        /// </summary>
        /// <param name="key">主键字段</param>
        /// <returns>对象</returns>
        public virtual Task<TEntity> FindByAsync(TPrimaryKey key)
        {
            return Task.Run(() =>
                {
                    return FindBy(key);
                });
        }

        /// <summary>
        /// 从仓储查找对象
        /// </summary>
        /// <param name="key">主键字段</param>
        /// <returns>对象</returns>
        public virtual TEntity FindBy(TPrimaryKey key)
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

                using (var rsp = this.CreateDbContent())
                {
                    return rsp.GetList<TEntity>(predicate).FirstOrDefault();
                }
            }
            else
            {
                using (var rsp = this.CreateDbContent())
                {
                    return rsp.Get<TEntity>(key);
                }
            }
        }

        /// <summary>
        /// 查询所有数据，注意这是个异步方法。
        /// </summary>
        /// <param name="sorts">排序字段</param>
        /// <returns>集合</returns>
        public async virtual Task<IEnumerable<TEntity>> GetAllAsync(IList<Sort> sorts = null)
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

                var qResult = Task.Run(
                    () =>
                        {
                            using (var rsp = this.CreateDbContent())
                            {
                                return rsp.GetList<TEntity>(predicate, dapperSorts).ToList();
                            }
                        });
                await qResult;
                return qResult.Result;
            }
            else
            {
                var qResult = Task.Run(
                    () =>
                        {
                            using (var rsp = this.CreateDbContent())
                            {
                                return rsp.GetList<TEntity>(null, dapperSorts).ToList();
                            }
                        });
                await qResult;
                return qResult.Result;
            }
        }

        /// <summary>
        /// 查询单个对象
        /// </summary>
        /// <param name="filter">筛选条件</param>
        public virtual TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter)
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

            using (var rsp = this.CreateDbContent())
            {
                return rsp.GetList<TEntity>(predicate).FirstOrDefault();
            }
        }

        /// <summary>
        /// 查询单个对象
        /// </summary>
        /// <param name="filter">筛选条件</param>
        public virtual Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
        {
            return Task.Run(() =>
                {
                    return this.GetFirstOrDefault(filter);
                });
        }

        /// <summary>
        /// 筛选数据
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <param name="sorts">排序字段</param>
        /// <returns>实体集合</returns>
        public virtual IEnumerable<TEntity> GetListByExp(
            Expression<Func<TEntity, bool>> filter,
            IList<Sort> sorts = null)
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

            using (var rsp = this.CreateDbContent())
            {
                var result= rsp.GetList<TEntity>(predicate, dapperSorts).ToList();
                return result;
            }
        }

        /// <summary>
        /// 指定字段范围查询，返回的实体只有这几个字段有值，目的是为了避免字段多时全字段查询（select *）
        /// </summary>
        /// <param name="columnNames">需要指定查询的字段</param>
        /// <param name="filter">筛选条件</param>
        /// <param name="sorts">排序字段</param>
        /// <returns>实体集合</returns>
        public virtual IEnumerable<TEntity> GetListWithColumns(IEnumerable<string> columnNames,
            Expression<Func<TEntity, bool>> filter,
            IList<Sort> sorts = null)
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

            using (var rsp = this.CreateDbContent())
            {
                var result = rsp.GetListWithColumns<TEntity>(columnNames,predicate, dapperSorts).ToList();
                return result;
            }
        }

        /// <summary>
        /// 分页查询对象集合,起始页码0
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="filter">数据筛选</param>
        /// <param name="sorts">排序字段</param>
        /// <returns>实体集合</returns>
        public virtual IListResult<TEntity> GetPaged(
            int pageIndex,
            int pageSize,
            Expression<Func<TEntity, bool>> filter,
            IList<Sort> sorts)
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

            // var listPage = this.DBContext.GetPage<TEntity>(predicate, dapperSorts, pageIndex, pageSize);
            // dataCount = DBContext.Count<TEntity>(predicate);
            using (var rsp = this.CreateDbContent())
            {
                var listPage = rsp.GetPage<TEntity>(predicate, dapperSorts, pageIndex, pageSize);
                return new ListResult<TEntity>(listPage.Item1, listPage.Item2);
            }
        }

        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="entity">对象</param>
        /// <returns></returns>
        public bool Remove(TEntity entity)
        {
            logerSvc.BLLDeleteLog<TEntity>(entity.Id.ToString(), NPlatform.Infrastructure.SerializerHelper.ToJson(entity));
            var enabled = this.Options.QueryFilters.ContainsKey(nameof(LogicDeleteFilter));

            if (typeof(ILogicDelete).IsAssignableFrom(typeof(TEntity)) && enabled)
            {
                ((ILogicDelete)entity).IsDeleted = true;
                using (var rsp = this.CreateDbContent())
                {
                    rsp.Update(entity);
                }
                return true;
            }
            else
            {
                using (var rsp = this.CreateDbContent())
                {
                    return rsp.Delete<TEntity>(entity);
                }
            }
        }

        /// <summary>
        /// 键值删除
        /// </summary>
        public virtual bool Remove(params TPrimaryKey[] keys)
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
            var enabled = this.Options.QueryFilters.ContainsKey(nameof(LogicDeleteFilter));
            if (typeof(ILogicDelete).IsAssignableFrom(typeof(TEntity))&& enabled)
            {
                using (var unitwork = IOC.IOCManager.BuildUnitOfWork())
                {
                    try
                    {
                        var entitys = this.GetListByExp(exp);
                        foreach (var entity in entitys)
                        {
                            ((ILogicDelete)entity).IsDeleted = true;
                            unitwork.Change(entity);
                        }

                        var ids = string.Join(",", keys);
                        logerSvc.BLLDeleteLog<TEntity>(ids, NPlatform.Infrastructure.SerializerHelper.ToJson(entitys));
                        unitwork.Commit();
                        return true;
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
                using (var rsp = this.CreateDbContent())
                {
                    var predicate = QueryBuilder<TEntity>.FromExpression(exp);
                    rsp.Delete<TEntity>(predicate);
                }
                var ids = string.Join(",", keys);
                logerSvc.BLLDeleteLog<TEntity>("",$"根据ID批量删除:{ids}");
                return true;
            }
        }

        /// <summary>
        /// 条件删除
        /// </summary>
        /// <param name="filter">删除条件</param>
        /// <returns>移除结果</returns>
        public bool Remove(Expression<Func<TEntity, bool>> filter)
        {
            var enabled = this.Options.QueryFilters.ContainsKey(nameof(LogicDeleteFilter));
            if (typeof(ILogicDelete).IsAssignableFrom(typeof(TEntity))&& enabled)
            {
                using (var unitwork = IOC.IOCManager.BuildUnitOfWork())
                {
                    try
                    {
                        var entitys = this.GetListByExp(filter);
                        foreach (var entity in entitys)
                        {
                            ((ILogicDelete)entity).IsDeleted = true;
                            unitwork.Change(entity);
                        }
                            var idArray = entitys.Select(t => t.Id);
                            var ids = string.Join(",", idArray);
                            logerSvc.BLLDeleteLog<TEntity>(ids, NPlatform.Infrastructure.SerializerHelper.ToJson(entitys));
                        unitwork.Commit();
                        return true;
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
                using (var rsp = this.CreateDbContent())
                {
                    var predicate = QueryBuilder<TEntity>.FromExpression(filter);
                        var maper = DapperExtensions.GetMap<TEntity>();
                      // logerSvc.BLLDeleteLog<TEntity>("where ", rsp.SerializedLambda(maper, predicate));
                    rsp.Delete<TEntity>(predicate);
                }

                return true;
            }
        }

        /// <summary>
        /// 异步移除
        /// </summary>
        /// <param name="keys">键值</param>
        /// <returns>结果</returns>
        public async Task<bool> RemoveAsync(params TPrimaryKey[] keys)
        {
            return await Task.Run<bool>(() => { return this.Remove(keys); });
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

        /// <summary>
        /// 统计记录数
        /// </summary>
        /// <param name="filter">筛选条件</param>
        public virtual int Count(Expression<Func<TEntity, bool>> filter)
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
            using (var rsp = this.CreateDbContent())
            {
                return rsp.Count<TEntity>(predicate);
            }
        }

        /// <summary>
        /// 求最大值
        /// </summary>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="attrName">属性名</param>
        /// <param name="filter">过滤条件</param>
        /// <returns>最大值</returns>
        public TValue Max<TValue>(string attrName, Expression<Func<TEntity, bool>> filter=null)
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
            using (var rsp = this.CreateDbContent())
            {
                var maxVal = rsp.Max<TValue, TEntity>(attrName, predicate);
                return maxVal;
            }
        }

        /// <summary>
        /// 求最小值
        /// </summary>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="attrName">属性名</param>
        /// <param name="filter">过滤条件</param>
        /// <returns>最小值</returns>
        public TValue Min<TValue>(string attrName, Expression<Func<TEntity, bool>> filter = null)
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
            using (var rsp = this.CreateDbContent())
            {
                var maxVal = rsp.Min<TValue, TEntity>(attrName, predicate);
                return maxVal;
            }
        }

        /// <summary>
        /// 求和
        /// </summary>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="attrName">属性名</param>
        /// <param name="filter">过滤条件</param>
        /// <returns>和</returns>
        public TValue Sum<TValue>(string attrName, Expression<Func<TEntity, bool>> filter = null)
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
            using (var rsp = this.CreateDbContent())
            {
                var sumVal = rsp.Sum<TValue, TEntity>(attrName, predicate);
                return sumVal;
            }
        }
        /// <summary>
        /// 求平均值
        /// </summary>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="attrName">属性名</param>
        /// <param name="filter">过滤条件</param>
        /// <returns>平均值</returns>
        public TValue AVG<TValue>(string attrName, Expression<Func<TEntity, bool>> filter = null)
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
            using (var rsp = this.CreateDbContent())
            {
                var avgVal = rsp.AVG<TValue, TEntity>(attrName, predicate);
                return avgVal;
            }
        }

    }
}
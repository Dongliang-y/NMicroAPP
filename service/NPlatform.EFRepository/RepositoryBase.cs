/***********************************************************
**项目名称:
**功能描述: 仓储  的摘要说明
**作    者:   易栋梁
**版 本 号:    1.0
**创建日期： 2015/12/7 16:06:56
**修改历史：
************************************************************/

using Microsoft.EntityFrameworkCore;
using NPlatform.Domains.Entity;
using NPlatform.Domains.IRepositories;
using NPlatform.Extends;
using NPlatform.Result;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using NPlatform;
using Microsoft.Extensions.Logging;
using NPlatform.Filters;
using NPlatform.Repositories;
using NPlatform.Repositories.IRepositories;

namespace NPlatform.Repositories
{
    /// <summary>
    /// 聚合仓储基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="string">主键类型</typeparam>
    public class RepositoryBase<TEntity, TPrimaryKey> : ResultBase, IRepository<TEntity, TPrimaryKey>
        where TEntity : EntityBase<TPrimaryKey>,new()
    {
        protected EFContext DBMain;
        protected EFContext DBMinor;

        public ILogger<RepositoryBase<TEntity,TPrimaryKey>> loggerSvc;

        protected IRepositoryOptions Options;


        /// <summary>
        /// create update delete
        /// </summary>
        private EFContext CUDContext
        {
            get
            {
                return this.DBMinor != null ? this.DBMinor : DBMain;
            }
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
            Options = option;
            DBMain = new EFContext(option.MainConection,option.DBProvider,(int)option.TimeOut);

            if (!option.MinorConnection.IsNullOrEmpty())
            {
                DBMain = new EFContext(option.MinorConnection, option.DBProvider, (int)option.TimeOut);
            }
        }


        #region 新增、修改、删除

        public TEntity this[TPrimaryKey key]
        {
            get
            {
                return this.DBMain.Find<TEntity>(key);
            }
            set
            {
                var entity = this.CUDContext.Find<TEntity>(key);
                if (entity == null)
                {
                    throw new KeyNotFoundException("未找到指定对象！");
                }
                loggerSvc.LogTrace("this[TPrimaryKey key] {0} ：共{1}条，", typeof(TEntity).Name, key);
                entity.CopyOrCreate(value);
                this.CUDContext.SaveChanges();
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item">新增对象</param>
        /// <returns>新增后创建了Id 的对象。</returns>
        public virtual async Task<TEntity> AddAsync(TEntity item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            loggerSvc.LogTrace("AddAsync{0} ：共{1}条，", typeof(TEntity).Name, item.Id);
            SetFilter(item);
            await this.CUDContext.AddAsync<TEntity>(item);
            await this.CUDContext.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// 异步新增
        /// </summary>
        /// <param name="items">新增对象的集合</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
        public async Task<int> AddsAsync(IEnumerable<TEntity> items)
        {
            if (items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            loggerSvc.LogTrace("AddsAsync{0} ：共{1}条，", typeof(TEntity).Name,items.Count());
            foreach (var item in items)
            {
                // 实体如果实现了过滤器， 那么仓储就可以拿注入进来的过滤器对实体进行设置与过滤。
                this.SetFilter(item);
            }
            await this.CUDContext.AddRangeAsync(items);
            return await this.CUDContext.SaveChangesAsync();
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
            loggerSvc.LogTrace("AddOrUpdate{0} ：{1}，", typeof(TEntity).Name, item.Id);
            var entity=await this.CUDContext.FindAsync<TEntity>(item.Id);
            entity.CopyOrCreate<TEntity>(item);
            return await this.CUDContext.SaveChangesAsync();
        }


        public async Task<int> RemoveAsync(Expression<Func<TEntity, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }
            var entitys = this.CUDContext.Set<TEntity>().Where(filter);

            var enabled = this.Options.QueryFilters.ContainsKey(nameof(LogicDeleteFilter));

            if (typeof(ILogicDelete).IsAssignableFrom(typeof(TEntity)) && enabled)
            {
                using (var unitwork = new UnitOfWork(Options))
                {
                    try
                    {
                        foreach (var entity in entitys)
                        {
                            ((ILogicDelete)entity).IsDeleted = true;
                            await unitwork.ChangeAsync(entity);
                        }
                        var keys = entitys.Select(t => t.Id);
                        var ids = string.Join(",", keys);
                        loggerSvc.LogTrace("逻辑删除{0}的数据：{1}，", typeof(TEntity).Name, keys);
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
                loggerSvc.LogTrace("物理删除{0}的数据：{1}，", typeof(TEntity).Name, filter.ToString());
                CUDContext.RemoveRange(entitys);
                return await CUDContext.SaveChangesAsync();
            }
        }

        public async Task<int> RemoveAsync(params TPrimaryKey[] keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }
            return await this.RemoveAsync(t => keys.Contains(t.Id));
        }

        public async Task<int> RemoveAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            this.loggerSvc.LogTrace("删除了{0}数据ID：{1}", entity.GetType().Name, entity.Id.ToString());
            var enabled = this.Options.QueryFilters.ContainsKey(nameof(LogicDeleteFilter));

            if (typeof(ILogicDelete).IsAssignableFrom(typeof(TEntity)) && enabled)
            {
                ((ILogicDelete)entity).IsDeleted = true;
            }
            else
            {
                CUDContext.Remove(entity);
            }
            return await CUDContext.SaveChangesAsync();
        }

        #endregion

        #region 查询

        public async Task<bool> ExistsAsync(TPrimaryKey key)
        {
            if (EqualityComparer<TPrimaryKey>.Default.Equals(key, default(TPrimaryKey)))
            {
                return false;
            }
            var rst = await this.DBMain.FindAsync<TEntity>(key);
            return rst != null;
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
        {
            if (filter == null)
            {
                return false;
            }
            // 应用过滤器
            foreach (var ft in this.Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(exp);
                }
            }

            var rst = await this.DBMain.Set<TEntity>().Where(filter).FirstOrDefaultAsync();
            return rst != null;
        }


        public async Task<IEnumerable<TEntity>> GetAllAsync(IEnumerable<SelectSort> sorts = null)
        {
            Expression<Func<TEntity, bool>> filter = x => !x.Id.Equals(default(TPrimaryKey));

            if (this.Options.QueryFilters.Count > 0)
            {

                foreach (var ft in this.Options.QueryFilters)
                {
                    var exp = ft.Value.GetFilter<TEntity>();
                    if (exp != null)
                    {
                        filter = filter.AndAlso(exp);
                    }
                }
            }

            var dataAll = this.DBMain.Set<TEntity>().Where(filter);

            if (sorts != null)
            {
                foreach (var sort in sorts)
                {
                    dataAll = OrderBy(dataAll, sort.PropertyName, !sort.Ascending);
                }
            }
            return await dataAll.ToListAsync();
        }

        public async Task<TEntity> FindByAsync(TPrimaryKey key)
        {
            if (EqualityComparer<TPrimaryKey>.Default.Equals(key, default(TPrimaryKey)))
            {
                return null;
            }

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


                return await this.DBMain.Set<TEntity>().Where(filter).FirstOrDefaultAsync();
            }
            else
            {
                return await this.DBMain.FindAsync<TEntity>(key);
            }
        }

        public async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
        {
            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (this.Options.QueryFilters.Count > 0)
            {
                foreach (var ft in this.Options.QueryFilters)
                {
                    var exp = ft.Value.GetFilter<TEntity>();
                    if (exp != null)
                    {
                        filter = filter.AndAlso(exp);
                    }
                }
            }
            return await this.DBMain.Set<TEntity>().Where(filter).FirstOrDefaultAsync();

        }

        /// <summary>
        /// 根据指定属性名称对序列进行排序
        /// </summary>
        /// <typeparam name="TSource">source中的元素的类型</typeparam>
        /// <param name="source">一个要排序的值序列</param>
        /// <param name="property">属性名称</param>
        /// <param name="descending">是否降序</param>
        /// <returns></returns>
        public IQueryable<TSource> OrderBy<TSource>(IQueryable<TSource> source, string property, bool descending) where TSource : class
        {
            ParameterExpression param = Expression.Parameter(typeof(TSource), "c");
            PropertyInfo pi = typeof(TSource).GetProperty(property);
            MemberExpression selector = Expression.MakeMemberAccess(param, pi);
            LambdaExpression le = Expression.Lambda(selector, param);
            string methodName = (descending) ? "OrderByDescending" : "OrderBy";
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(TSource), pi.PropertyType }, source.Expression, le);
            return source.Provider.CreateQuery<TSource>(resultExp);
        }


        public async Task<IEnumerable<TEntity>> GetListByExpAsync(Expression<Func<TEntity, bool>> filter, IEnumerable<SelectSort> sorts = null)
        {
            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter));
            }
            // 应用过滤器
            foreach (var ft in this.Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(exp);
                }
            }

            var setAll = this.DBMain.Set<TEntity>().Where(filter);
            if(sorts!=null)
            {
                foreach (var sort in sorts)
                {
                    setAll = OrderBy(setAll, sort.PropertyName, !sort.Ascending);
                }
            }
            return await setAll.ToListAsync();
        }

        /// <summary>
        /// EF 版本，EF实现指定字段查询比较困难，所以先查出所有字段
        /// </summary>
        /// <param name="columnNames"></param>
        /// <param name="filter"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetListWithColumnsAsync(IEnumerable<string> columnNames, Expression<Func<TEntity, bool>> filter, IEnumerable<SelectSort> sorts = null)
        {
            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter));
            }
            // 应用过滤器
            foreach (var ft in this.Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(exp);
                }
            }
            var rst = await GetListByExpAsync(filter, sorts);
            return rst;          
        }


        public async Task<IListResult<TEntity>> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> filter, IEnumerable<SelectSort> sorts)
        {
            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter));
            }
            // 应用过滤器
            foreach (var ft in this.Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(exp);
                }
            }

            var resultSet = this.DBMain.Set<TEntity>().Where(filter);

            var total = this.DBMain.Set<TEntity>().Where(filter).Count();
            if (sorts != null)
            {
                foreach (var sort in sorts)
                {
                    resultSet = OrderBy(resultSet, sort.PropertyName, !sort.Ascending);
                }
            }
            
            resultSet = resultSet.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return base.ListData(await resultSet.ToListAsync(), total);
        }
        #endregion

        #region 统计
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter)
        {
            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter));
            }
            // 应用过滤器
            foreach (var ft in this.Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(exp);
                }
            }
            return await this.DBMain.Set<TEntity>().Where(filter).CountAsync();
        }

        public async Task<TValue> MaxAsync<TValue>(Expression<Func<TEntity, TValue>> selector, Expression<Func<TEntity, bool>> filter = null)
        {
            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter));
            }
            // 应用过滤器
            foreach (var ft in this.Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(exp);
                }
            }
            return await this.DBMain.Set<TEntity>().Where(filter).MaxAsync(selector);
        }

        public async Task<TValue> MinAsync<TValue>(Expression<Func<TEntity, TValue>> selector, Expression<Func<TEntity, bool>> filter = null)
        {
            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter));
            }
            // 应用过滤器
            foreach (var ft in this.Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(exp);
                }
            }
            return await this.DBMain.Set<TEntity>().Where(filter).MinAsync(selector);
        }

        public async Task<decimal> SumAsync(Expression<Func<TEntity, decimal>> selector, Expression<Func<TEntity, bool>> filter = null)
        {
            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter));
            }
            // 应用过滤器
            foreach (var ft in this.Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(exp);
                }
            }
            return await this.DBMain.Set<TEntity>().Where(filter).SumAsync(selector);
        }

        public async Task<decimal> AVGAsync(Expression<Func<TEntity, decimal>> selector, Expression<Func<TEntity, bool>> filter = null)
        {
            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter));
            }
            // 应用过滤器
            foreach (var ft in this.Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(exp);
                }
            }

            return await this.DBMain.Set<TEntity>().Where(filter).AverageAsync(selector);
        }
        #endregion

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
    }

    public static class objExt
    {
        /// <summary>
        /// 创建对象，只处理基本数据类型的字段，
        /// </summary>
        /// <typeparam name="T">要创建的类型</typeparam>
        /// <param name="values">字典</param>
        /// <param name="fun">委托，复杂类型交给调用方自己处理</param>
        /// <returns></returns>
        public static TEntity CopyOrCreate<TEntity>(this TEntity entity, TEntity src, Func<TEntity, TEntity> fun = null) where TEntity: new()
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }

            var attr = typeof(TEntity).GetProperties();

            if (entity == null)
                entity = new TEntity();
            foreach (var prop in attr)
            {
                var val = prop.GetValue(src);
                if (val == null)
                {
                    continue;
                }

                if (prop.PropertyType == typeof(string))
                {
                    prop.SetValue(entity, val.ToString());
                }
                else if (prop.PropertyType == typeof(long) || prop.PropertyType == typeof(long?))
                {
                    prop.SetValue(entity, Convert.ToInt64(val));
                }
                else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
                {
                    prop.SetValue(entity, Convert.ToInt32(val));
                }
                else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
                {
                    prop.SetValue(entity, Convert.ToBoolean(val));
                }
                else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                {
                    prop.SetValue(entity, Convert.ToDecimal(val));
                }
                else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                {
                    prop.SetValue(entity, Convert.ToDateTime(val));
                }
                else if (prop.PropertyType == typeof(float) || prop.PropertyType == typeof(float?))
                {
                    prop.SetValue(entity, Convert.ToDouble(val));
                }
                else if (prop.PropertyType == typeof(byte) || prop.PropertyType == typeof(byte?))
                {
                    prop.SetValue(entity, Convert.ToByte(val));
                }
                else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
                {
                    prop.SetValue(entity, Convert.ToDouble(val));
                }
            }
            entity = fun != null ? fun(entity) : entity;
            return entity;
        }
    }
}
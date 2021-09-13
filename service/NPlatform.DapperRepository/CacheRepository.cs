/***********************************************************
**项目名称:	                                                                  				   
**功能描述:	  的摘要说明
**作    者: 	易栋梁                                         			   
**版 本 号:	1.0                                             			   
**创建日期： 2015/12/7 16:06:56
**修改历史：
************************************************************/

namespace NPlatform.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using DapperExtensions;
    using NPlatform.Domains.Entity;
    using NPlatform.Domains.IRepositories;
    using NPlatform.Domains.Service;
    using NPlatform.Filters;
    using NPlatform.Infrastructure.Loger;
    using NPlatform.Infrastructure.Redis;
    using NPlatform.Result;

    /// <summary>
    /// 聚合仓储基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public class CacheRepository<TEntity, TPrimaryKey> : AggregationRepository<TEntity, TPrimaryKey>
        where TEntity : AggregationBase<TPrimaryKey>
    {
        /// <summary>
        /// 缓存的数据
        /// </summary>
        private IList<TEntity> cacheDatas;

        /// <summary>
        /// 缓存名称
        /// </summary>
        private string cacheName = $"CacheRepository{ typeof(TEntity).Name}";

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheRepository{TEntity,TPrimaryKey}"/> class. 
        /// 仓储基类
        /// </summary>
        /// <param name="option">
        /// The options.
        /// </param>
        public CacheRepository(IRepositoryOptions option)
            : base(option)
        {
        }

        /// <summary>
        /// 缓存的数据
        /// </summary>
        private IList<TEntity> CacheDatas
        {
            get
            {
                // 判断缓存是否存在
                if (cacheDatas == null)
                {
                    cacheDatas = LoadCacheDataAsyn();
                }

                return cacheDatas;
            }
        }

        /// <summary>
        /// 实现[]操作
        /// </summary>
        /// <param name="key">实体的key值</param>
        /// <returns>实体对象</returns>
        public override TEntity this[TPrimaryKey key]
        {
            get
            {
                return FindBy(key);
            }

            set
            {
                using (var rsp = new EPDBContext(Options.ConnectName))
                {
                    rsp.Update(value);
                }
                ClearCache();
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item">新增对象</param>
        /// <returns>新增的实体</returns>
        public override TEntity Add(TEntity item)
        {
            if (typeof(ITenant).IsAssignableFrom(typeof(TEntity)))
            {
                var tenant = Options.QueryFilters.Values.FirstOrDefault(t => t is TenantFilter);
                if (tenant != null)
                {
                    (item as ITenant).TenantId = tenant.FilterParameters[DataFilterParameters.TenantId].ToString();
                }
            }

            using (var rsp = new EPDBContext(Options.ConnectName))
            {
                ClearCache();
                var rst = rsp.Insert(item);
                return item;
            }

        }

        /// <summary>
        /// 新增或更新对象
        /// </summary>
        /// <param name="entity">实体</param>
        public override TEntity AddOrUpdate(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentEmptyException(nameof(entity));
            }
            if (Exists(entity.Id))
            {
                this[entity.Id] = entity;
            }
            else
            {
                if (typeof(ITenant).IsAssignableFrom(typeof(TEntity)))
                {
                    var tenant = Options.QueryFilters.Values.FirstOrDefault(t => t is TenantFilter);
                    if (tenant != null)
                    {
                        (entity as ITenant).TenantId =
                            tenant.FilterParameters[DataFilterParameters.TenantId].ToString();
                    }
                }

                Add(entity);
            }
            ClearCache();
            return entity;
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="items">实体集合</param>
        public override int Adds(IEnumerable<TEntity> items)
        {
            var tEntities = items as TEntity[] ?? items.ToArray();
            foreach (var item in tEntities)
            {
                if (typeof(ITenant).IsAssignableFrom(typeof(TEntity)))
                {
                    var tenant = Options.QueryFilters.Values.FirstOrDefault(t => t is TenantFilter);
                    if (tenant != null)
                    {
                        (item as ITenant).TenantId = tenant.FilterParameters[DataFilterParameters.TenantId].ToString();
                    }
                }
            }
            ClearCache();
            using (var rsp = new EPDBContext(Options.ConnectName))
            {
                var intRst = rsp.Inserts(tEntities);
                return intRst;
            }

        }

        /// <summary>
        /// 统计记录数
        /// </summary>
        /// <param name="filter">过滤条件</param>
        public override int Count(Expression<Func<TEntity, bool>> filter)
        {
            // 应用过滤器
            foreach (var ft in Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(exp);
                }
            }

            var result = CacheDatas.Count(filter.Compile());
            return result;
        }

        /// <summary>
        /// 判断对象是否已存在
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>结果</returns>
        public override bool Exists(TPrimaryKey key)
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
        public override bool Exists(Expression<Func<TEntity, bool>> filter)
        {
            // 应用过滤器
            foreach (var ft in Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(exp);
                }
            }

            var datas = GetListByExp(filter);
            return datas.Any();
        }

        /// <summary>
        /// 从仓储查找对象
        /// </summary>
        /// <param name="key">主键字段</param>
        /// <returns>查找结果</returns>
        public override TEntity FindBy(TPrimaryKey key)
        {
            if (Options.QueryFilters.Count > 0)
            {
                Expression<Func<TEntity, bool>> filter = x => x.Id.Equals((object)key);
                foreach (var ft in Options.QueryFilters)
                {
                    var exp = ft.Value.GetFilter<TEntity>();
                    if (exp != null)
                    {
                        filter = filter.AndAlso(exp);
                    }
                }

                return CacheDatas.FirstOrDefault(filter.Compile());
            }
            else
            {
                return CacheDatas.FirstOrDefault(t => t.Id.Equals((object)key));
            }
        }

        /// <summary>
        /// 查询单个对象
        /// </summary>
        /// <param name="filter">筛选条件</param>
        public override TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter)
        {
            // 应用过滤器
            foreach (var ft in Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(ft.Value.GetFilter<TEntity>());
                }
            }

            return CacheDatas.FirstOrDefault(filter.Compile());
        }

        /// <summary>
        /// 筛选数据
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <param name="sorts">排序字段</param>
        /// <returns>查询结果</returns>
        public override IEnumerable<TEntity> GetListByExp(
            Expression<Func<TEntity, bool>> filter,
            IList<Sort> sorts = null)
        {
            if (sorts != null)
            {
                throw new NPlatformException("缓存不支持排序，请在服务里使用linq实现", "GetListByExp");
            }
            var t1 = DateTime.Now;
            // 应用过滤器
            foreach (var ft in Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(exp);
                }
            }
            var t2 = DateTime.Now;
            // System.Diagnostics.Debug.WriteLine("GetListByExp " + DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            var express = filter.Compile();
            var t3 = DateTime.Now;


            // System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            var result = CacheDatas.Where(express).ToList();
            var t4 = DateTime.Now;
            Console.WriteLine($"缓存测试t2-t1:{(t2 - t1).TotalMilliseconds},t3-t2:{(t3 - t2).TotalMilliseconds},t4-t3:{(t4 - t3).TotalMilliseconds}");
            // System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            return result;
        }

        /// <summary>
        /// 分页查询对象集合,起始页码0
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="filter">数据筛选</param>
        /// <param name="sorts">基于缓存的排序字段暂时未实现</param>
        public override IListResult<TEntity> GetPaged(
            int pageIndex,
            int pageSize,
            Expression<Func<TEntity, bool>> filter,
            IList<Sort> sorts)
        {
            if (sorts != null)
            {
                throw new NPlatformException("缓存不支持排序，请在服务里使用linq实现", "GetListByExp");
            }

            //应用过滤器
            foreach (var ft in Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<TEntity>();
                if (exp != null)
                {
                    filter = filter.AndAlso(ft.Value.GetFilter<TEntity>());
                }
            }

            //var predicate = QueryBuilder<TEntity>.FromExpression(filter);

            // var listPage = this.DBContext.GetPage<TEntity>(predicate, dapperSorts, pageIndex, pageSize);
            // dataCount = DBContext.Count<TEntity>(predicate);
            try
            {
                var func = filter.Compile();
                var result = this.CacheDatas.Where(func);
                var pageResult = SearchArrayPage(result, pageIndex, pageSize);

                return new ListResult<TEntity>(pageResult, result.Count());
            }
            catch (NullReferenceException ex)
            {
                throw new NPlatformException($"{nameof(filter)}条件表达式中所需的字段，在集合实体相应字段的值为空值，拉姆达表达式无法执行，请更改条件！" + ex.Message, "CacheRepository");
            }
        }

        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回结果</returns>
        public new IEPResult<bool> Remove(TEntity entity)
        {
            if (typeof(ILogicDelete).IsAssignableFrom(typeof(TEntity)))
            {
                ((ILogicDelete)entity).IsDeleted = true;
                using (var rsp = new EPDBContext(Options.ConnectName))
                {
                    rsp.Update(entity);
                }
                ClearCache();
                return Success("删除成功！");
            }
            else
            {
                using (var rsp = new EPDBContext(Options.ConnectName))
                {
                    rsp.Delete(entity);
                    ClearCache();
                    return Success("删除成功！");
                }
            }
        }

        /// <summary>
        /// 键值删除
        /// </summary>
        /// <param name="keys">键集合</param>
        /// <returns>删除结果</returns>
        public override bool Remove(params TPrimaryKey[] keys)
        {
            Expression<Func<TEntity, bool>> exp = t => keys.Contains(t.Id);

            if (typeof(ILogicDelete).IsAssignableFrom(typeof(TEntity)))
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

                        unitwork.Commit();
                        ClearCache();
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
                using (var rsp = new EPDBContext(Options.ConnectName))
                {
                    rsp.Delete(exp);
                }
                ClearCache();
                return true;
            }
        }

        /// <summary>
        /// 条件删除
        /// </summary>
        /// <param name="filter">删除条件</param>
        /// <returns>删除结果</returns>
        public new bool Remove(Expression<Func<TEntity, bool>> filter)
        {
            if (typeof(ILogicDelete).IsAssignableFrom(typeof(TEntity)))
            {
                ClearCache();
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

                        unitwork.Commit();
                        ClearCache();
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
                using (var rsp = new EPDBContext(Options.ConnectName))
                {
                    rsp.Delete(filter);
                    ClearCache();
                }

                return true;
            }
        }

        /// <summary>
        /// 对集合继续分页查询
        /// </summary>
        /// <typeparam name="T">集合的类型</typeparam>
        /// <param name="sources">源集合</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>返回结果</returns>
        public IEnumerable<T> SearchArrayPage<T>(IEnumerable<T> sources, int page, int pageSize)
        {
            if (sources == null)
            {
                return new List<T>();
            }
            if (page > 0)
            {
                // 分页
                page--;
                var result = sources.Skip(page * pageSize).Take(pageSize).ToList();
                return result;
            }
            else
            {
                throw new LogicException("页码不能小于等于0", "SearchArrayPage");
            }
        }

        /// <summary>
        /// 清理当前仓储的缓存
        /// </summary>
        public void ClearCache()
        {
            TableRedis.Instance.Remove(cacheName);
            cacheDatas = null;
        }

        /// <summary>
        /// 异步加载缓存数据
        /// </summary>
        /// <returns>加载的数据</returns>
        private IList<TEntity> LoadCacheDataAsyn()
        {
            IList<TEntity> data = null;
            data = TableRedis.Instance.Get<IList<TEntity>>(this.cacheName);
            if (data == null)
            {
                using (var rsp = new EPDBContext(this.Options.ConnectName))
                {
                    var datas = rsp.GetList<TEntity>().ToList();
                    if (datas.Count > 10000)
                    {
                        LogerHelper.Warn("缓存的整表数据超过1W条，请谨慎使用缓存。", "Sys");
                    }

                    TableRedis.Instance.Add(this.cacheName, datas);
                    return datas;
                }
            }
            return data;
        }
    }
}
#region << 版 本 注 释 >>

/*----------------------------------------------------------------
* 项目名称 ：NPlatform.Domains.Service
* 类 名 称 ：FilterManager
* 类 描 述 ：过滤器管服务，解决软删除数据过滤，租户数据过滤，项目权限数据过滤。
* 命名空间 ：NPlatform.Domains.Service
* CLR 版本 ：4.0.30319.42000
* 作    者 ：DongliangYi
* 创建时间 ：2018-11-20 15:53:07
* 更新时间 ：2018-11-20 15:53:07
* 版 本 号 ：v1.0.0.0
//----------------------------------------------------------------*/

#endregion

namespace NPlatform.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using NPlatform.Domains.Entity;
    using NPlatform.Domains.IRepositories;
    using NPlatform.Result;

    /// <summary>
    /// 过滤器管理服务
    /// </summary>
    public class FilterManager
    {
        /// <summary>
        /// 仓储参数
        /// </summary>
        private IRepositoryOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterManager"/> class. 
        /// </summary>
        /// <param name="repositoryOptions">
        /// 需要配置的仓储的参数列表
        /// </param>
        public FilterManager(IRepositoryOptions repositoryOptions)
        {
            this.options = repositoryOptions ?? throw new ArgumentNullException(nameof(repositoryOptions));
        }

        /// <summary>
        /// 禁用过滤器
        /// </summary>
        /// <typeparam name="TFilter">过滤器类型</typeparam>
        /// <returns>禁用结果</returns>
        public bool DisabledFilter<TFilter>() where TFilter : class , IFilter
        {
            var filterName = typeof(TFilter).Name;

            if (this.options.QueryFilters.ContainsKey(filterName))
            {
                var filter = this.options.QueryFilters[filterName];
                filter.IsEnabled = false;
                return true;
            }
            else if (this.options.ResultFilters.ContainsKey(filterName))
            {
                var filter = this.options.ResultFilters[filterName];
                filter.IsEnabled = false;
                return true;
            }
            else
            {
                throw new NPlatformException("过滤器不存在！", "FilterManager.EnabledFilter");
            }
        }

        /// <summary>
        /// 启用过滤器
        /// </summary>
        /// <typeparam name="TFilter">过滤器类型</typeparam>
        /// <returns>结果</returns>
        public bool EnabledFilter<TFilter>() where TFilter : class , IFilter
        {
            var filterName = typeof(TFilter).Name;

            if (this.options.QueryFilters.ContainsKey(filterName))
            {
                var filter = this.options.QueryFilters[filterName];
                filter.IsEnabled = true;
                return true;
            }
            if (this.options.ResultFilters.ContainsKey(filterName))
            {
                var filter = this.options.ResultFilters[filterName];
                filter.IsEnabled = true;
                return true;
            }
            else
            {
                throw new NPlatformException("过滤器不存在！", "FilterManager.EnabledFilter");
            }
        }

        /// <summary>
        /// 注册一个过滤器
        /// </summary>
        /// <param name="filter">过滤器对象</param>
        /// <exception cref="ArgumentEmptyException">过滤器对象为空</exception>
        public void Register(IQueryFilter filter)
        {
            var filterName = filter.GetType().Name;
            if (filter == null)
            {
                throw new ArgumentEmptyException(nameof(filter));
            }

            if (this.options.AllQueryFilters.ContainsKey(filterName))
            {
                this.options.AllQueryFilters[filterName].IsEnabled = true;
            }
            
            this.options.AllQueryFilters.Add(filterName, filter);
        }
        /// <summary>
        /// 设置过滤器参数
        /// </summary>
        /// <typeparam name="TFilter">过滤器</typeparam>
        /// <param name="par">参数</param>
        public void SetParameters<TFilter>(params KeyValuePair<string, object>[] par)
        {
            var filterName = typeof(TFilter).Name;
            IFilter filter;
            if (this.options.QueryFilters.ContainsKey(filterName))
            {
                filter = this.options.QueryFilters[filterName];
            }
            else if (this.options.ResultFilters.ContainsKey(filterName))
            {
                filter = this.options.QueryFilters[filterName];
            }
            else
            {
                return;
            }
            foreach (var prm in par)
            {
                if (filter.FilterParameters.ContainsKey(prm.Key))
                {
                    filter.FilterParameters[prm.Key] = prm.Value;
                }
                else
                {
                    var tmpKv = new KeyValuePair<string, object>(prm.Key, prm.Value);
                    filter.FilterParameters.Add(tmpKv);
                }
            }
        }

        public IEnumerable<T> ResultFilter<T>(IEnumerable<T> result) where T : IEntity
        {
            // 应用过滤器
            Expression<Func<T, bool>> filter = t => true;
            foreach (var ft in this.options.ResultFilters)
            {
                var exp = ft.Value.GetFilter<T>();
                if (exp != null)
                {
                    filter = filter.AndAlso(ft.Value.GetFilter<T>());
                }
            }

            result = result.Where(filter.Compile());
            return result;
        }
    }
}
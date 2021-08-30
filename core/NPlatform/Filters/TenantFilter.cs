#region << 版 本 注 释 >>

/*----------------------------------------------------------------
* 项目名称 ：NPlatform.Domains.Service
* 类 名 称 ：IsDeleteFilter
* 类 描 述 ：
* 命名空间 ：NPlatform.Domains.Service
* CLR 版本 ：4.0.30319.42000
* 作    者 ：DongliangYi
* 创建时间 ：2018-11-21 8:56:05
* 更新时间 ：2018-11-21 8:56:05
* 版 本 号 ：v1.0.0.0
//----------------------------------------------------------------*/

#endregion

namespace NPlatform.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// 租户过滤器
    /// </summary>
    public class TenantFilter : BaseFilter, IQueryFilter
    {
        /// <summary>
        /// 获取过滤器
        /// </summary>
        /// <typeparam name="T">过滤的类型</typeparam>
        /// <returns>过滤表达式</returns>
        public override Expression<Func<T, bool>> GetFilter<T>()
        {
            var tenantId = string.Empty;
            if (this.FilterParameters.ContainsKey(DataFilterParameters.TenantId))
            {
                tenantId = this.FilterParameters[DataFilterParameters.TenantId].ToString().TrimNull();
            }

            if (typeof(ITenant).IsAssignableFrom(typeof(T)) && !tenantId.IsNullOrEmpty())
            {
                Expression<Func<T, bool>> filter = t =>
                    (t as ITenant).TenantId == tenantId;
                return filter;
            }

            return null;
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="item">实体</param>
        public override void SetFilterProperty<T>(T item)
        {
            var tenantId = string.Empty;
            if (this.FilterParameters.ContainsKey(DataFilterParameters.TenantId))
            {
                tenantId = this.FilterParameters[DataFilterParameters.TenantId].ToString().TrimNull();
            }

            if (typeof(ITenant).IsAssignableFrom(typeof(T)) && !tenantId.IsNullOrEmpty())
            {
                (item as ITenant).TenantId = this.FilterParameters[DataFilterParameters.TenantId].ToString();
            }
        }
    }
}
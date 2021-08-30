#region << 版 本 注 释 >>

/*----------------------------------------------------------------
* 项目名称 ：NPlatform.Domains.Entity
* 类 名 称 ：IFilter
* 类 描 述 ：过滤器，过滤器模式也称作标准模式，可以根据不同标准来分别或统一过滤对象。该模式属于结构型模式。
* http://www.runoob.com/design-pattern/intercepting-filter-pattern.html
* 
* 比如常见场景为 ， 软删除数据过滤、租户数据过滤、不同数据权限过滤。
* 命名空间 ：NPlatform.Domains.Entity
* CLR 版本 ：4.0.30319.42000
* 作    者 ：DongliangYi
* 创建时间 ：2018-11-20 15:37:15
* 更新时间 ：2018-11-20 15:37:15
* 版 本 号 ：v1.0.0.0
//----------------------------------------------------------------*/

#endregion

namespace NPlatform.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using NPlatform.Domains.Entity;

    /// <summary>
    /// 过滤器接口
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Gets filterParameters 
        /// </summary>
        IDictionary<string, object> FilterParameters { get; }

        /// <summary>
        /// Gets or sets a value indicating whether 是否启用
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// 设置过滤器
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        void SetFilterProperty<T>(T item) where T : IEntity;

        /// <summary>
        /// 过滤表达式，直接作用于仓储对于数据的筛选。
        /// </summary>
        /// <typeparam name="T">需要过滤的实体类型</typeparam>
        /// <returns>返回的表达式</returns>
        Expression<Func<T, bool>> GetFilter<T>()
            where T : IEntity;
    }
}
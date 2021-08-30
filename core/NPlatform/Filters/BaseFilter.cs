#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 类 名 称 ：FilterBase
* 类 描 述 ：
* 所在的域 ：DESKTOP-UU3LV10
* 命名空间 ：NPlatform.Filters
* CLR 版本 ：4.0.30319.42000
* 作    者 ：Dongliang Yi
* 创建时间 ：2019/1/27 16:58:07
* 更新时间 ：2019/1/27 16:58:07
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ yidon 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion
namespace NPlatform.Filters
{
    using System;
    using System.Collections.Generic;

    using System.Linq.Expressions;
    using NPlatform.Domains.Entity;

    /// <summary>
    /// 过滤器基类
    /// </summary>
    public abstract class BaseFilter : IFilter
    {
        /// <summary>
        /// Gets 参数列表
        /// </summary>
        public IDictionary<string, object> FilterParameters { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets a value indicating whether 是否启用
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 设置过滤器
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="item">实体对象</param>
        public abstract void SetFilterProperty<T>(T item) where T : IEntity;

        /// <summary>
        /// 获取过滤器
        /// </summary>
        /// <typeparam name="T">过滤器对象</typeparam>
        /// <returns>linq表达式方式的筛选条件</returns>
        public abstract Expression<Func<T, bool>> GetFilter<T>()
            where T : IEntity;
    }
}

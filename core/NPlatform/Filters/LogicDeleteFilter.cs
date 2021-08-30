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
    /// 软删除过滤器
    /// </summary>
    public class LogicDeleteFilter : BaseFilter, IQueryFilter
    {
        /// <summary>
        /// 获取过滤表达式
        /// </summary>
        /// <typeparam name="T">要过滤的类型</typeparam>
        /// <returns>表达式</returns>
        public override Expression<Func<T, bool>> GetFilter<T>()
        {
            if (typeof(ILogicDelete).IsAssignableFrom(typeof(T)))
            {
                Expression<Func<T, bool>> filter = t => (t as ILogicDelete).IsDeleted == false;
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
            if (typeof(ILogicDelete).IsAssignableFrom(typeof(T)))
            {
                (item as ILogicDelete).IsDeleted = false; // 设置过滤器属性的时候一般是创建对象的时候，所以给与物理删除的默认值，false。
            }
        }
    }
}
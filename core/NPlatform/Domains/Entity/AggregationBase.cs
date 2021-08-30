#region << 版 本 注 释 >>

/*----------------------------------------------------------------
* 项目名称 ：NPlatform.Domains.Entity
* 类 名 称 ：AggregationBase
* 类 描 述 ：
* 命名空间 ：NPlatform.Domains.Entity
* CLR 版本 ：4.0.30319.42000
* 作    者 ：DongliangYi
* 创建时间 ：2018-12-13 16:21:58
* 更新时间 ：2018-12-13 16:21:58
* 版 本 号 ：v1.0.0.0
//----------------------------------------------------------------*/

#endregion

namespace NPlatform.Domains.Entity
{
    using System;

    /// <summary>
    /// The aggregation base.
    /// </summary>
    /// <typeparam name="TPrimaryKey">
    /// 主键类型
    /// </typeparam>
    [Serializable]
    public class AggregationBase<TPrimaryKey> : EntityBase<TPrimaryKey>, IAggregation<TPrimaryKey>, IDisposable
    {
    }
}
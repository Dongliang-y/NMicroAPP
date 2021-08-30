#region << 版 本 注 释 >>

/*----------------------------------------------------------------
* 项目名称 ：NPlatform.Domain
* 类 名 称 ：IAggregation
* 类 描 述 ： 聚合根接口
* 命名空间 ：NPlatform.Domain
* CLR 版本 ：4.0.30319.42000
* 作    者 ：DongliangYi
* 创建时间 ：2018-11-19 17:10:23
* 更新时间 ：2018-11-19 17:10:23
* 版 本 号 ：v1.0.0.0
//----------------------------------------------------------------*/

#endregion

namespace NPlatform.Domains.Entity
{
    /// <summary>
    /// 聚合根接口
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public interface IAggregation<TPrimaryKey> : IEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        TPrimaryKey Id { get; set; }
    }
}
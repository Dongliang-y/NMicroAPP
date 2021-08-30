#region << 版 本 注 释 >>

/*----------------------------------------------------------------
* 项目名称 ：NPlatform.Domains.Entity
* 类 名 称 ：ITenant
* 类 描 述 ：是否是多租户过滤器接口
* 命名空间 ：NPlatform.Domains.Entity
* CLR 版本 ：4.0.30319.42000
* 作    者 ：DongliangYi
* 创建时间 ：2018-11-20 15:13:31
* 更新时间 ：2018-11-20 15:13:31
* 版 本 号 ：v1.0.0.0
//----------------------------------------------------------------*/

#endregion

namespace NPlatform.Filters
{
    /// <summary>
    /// 多租户实体
    /// </summary>
    public interface ITenant : IFilterProperties
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        string TenantId { get; set; }
    }
}
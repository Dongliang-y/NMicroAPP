#region << 版 本 注 释 >>

/*----------------------------------------------------------------
* 项目名称 ：NPlatform.Domains.Entity
* 类 名 称 ：DataFilterParameters
* 类 描 述 ：
* 命名空间 ：NPlatform.Domains.Entity
* CLR 版本 ：4.0.30319.42000
* 作    者 ：DongliangYi
* 创建时间 ：2018-11-21 9:37:57
* 更新时间 ：2018-11-21 9:37:57
* 版 本 号 ：v1.0.0.0
//----------------------------------------------------------------*/

#endregion

namespace NPlatform.Filters
{
    /// <summary>
    /// 平台基本过滤器参数
    /// </summary>
    public class DataFilterParameters
    {
        /// <summary>
        /// 软删除参数名
        /// </summary>
        public const string IsDeleted = "ISDELETED";

        /// <summary>
        /// 租户参数名
        /// </summary>
        public const string TenantId = "TENANTID";

        /// <summary>
        /// 客户端参数名
        /// </summary>
        public const string ClientId = "CLIENTID";
    }
}
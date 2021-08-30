#region << 版 本 注 释 >>

/*----------------------------------------------------------------
* 项目名称 ：NPlatform.Domains.Entity
* 类 名 称 ：ISoftDelete
* 类 描 述 ：是否为软删除过滤器接口
* 命名空间 ：NPlatform.Domains.Entity
* CLR 版本 ：4.0.30319.42000
* 作    者 ：DongliangYi
* 创建时间 ：2018-11-20 15:09:44
* 更新时间 ：2018-11-20 15:09:44
* 版 本 号 ：v1.0.0.0
//----------------------------------------------------------------*/

#endregion

namespace NPlatform.Filters
{
    /// <summary>
    /// 逻辑删除过滤器
    /// </summary>
    public interface ILogicDelete : IFilterProperties
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        bool IsDeleted { get; set; }
    }
}
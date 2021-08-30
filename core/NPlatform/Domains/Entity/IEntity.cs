/***********************************************************
**项目名称:	                                                                  				   
**功能描述:	  的摘要说明
**作    者: 	易栋梁                                         			   
**版 本 号:	1.0                                             			   
**创建日期： 2015/5/15 11:19:07
**修改历史：
************************************************************/

namespace NPlatform.Domains.Entity
{
    /// <summary>
    ///     Defines interface for base entity type. All entities in the system must implement this interface.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        ///     检查当前对象是否未临时对象，不是从仓储加载的实例。
        /// </summary>
        /// <returns>True, if this entity is transient</returns>
        bool IsTransient();
        /// <summary>
        /// 获取当前实体ID
        /// </summary>
        /// <returns>id</returns>
        string GetID();
    }
}
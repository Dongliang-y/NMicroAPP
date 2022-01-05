/***********************************************************
**项目名称:NPlatform.Entity                                                                				   
**功能描述:	 Rule_Data   的摘要说明
**作    者: 	此代码由CodeSmith生成。                                         			   
**版 本 号:	1.0                                                  
**修改历史：
************************************************************/

namespace NPlatform.Domains.Entity.Sys
{
    using NPlatform.Domains.Entity;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
	/// <summary>
	///Rule_Data，sys_Rule_Data
	/// </summary>
	[Serializable]
    [Table(name:"sys_rule_data")]
    public partial class Rule_Data : EntityBase<string>, ISys
	{
		/// <summary>
		/// 数据源名称
		/// </summary>
		
        [Display(Name = "数据源名称")]
        [StringLength(600)]
        public string DataName { get; set; }

		/// <summary>
		/// 数据筛选脚本
		/// </summary>
		
        [Display(Name = "数据筛选脚本")]
        [StringLength(300)]
        public string DataScript { get; set; }

		/// <summary>
		/// 角色ID
		/// </summary>
		
        [Display(Name = "角色ID")]
        [StringLength(300)]
        public string RoleId { get; set; }

		/// <summary>
		/// 关联表
		/// </summary>
		
        [Display(Name = "关联表")]
        [StringLength(600)]
        public string tabName { get; set; }
	}
}

/***********************************************************
**项目名称:NPlatform.Entity                                                                				   
**功能描述:	 Role_Group   的摘要说明
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
	///Role_Group，角色-用户组
	/// </summary>
	[Serializable]
    [Table(name:"sys_role_group")]
    public partial class Role_Group : EntityBase<string>, ISys
	{
		/// <summary>
		/// 角色ID
		/// </summary>
		
        [Display(Name = "角色ID")]
        [StringLength(150)]
        public string role_id { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		
        [Display(Name = "创建时间")]
        public DateTime? create_time { get; set; }

		/// <summary>
		/// 创建人
		/// </summary>
		
        [Display(Name = "创建人")]
        [StringLength(1500)]
        public string create_user { get; set; }

		/// <summary>
		/// 用户组ID
		/// </summary>
		
        [Display(Name = "用户组ID")]
        [StringLength(150)]
        public string group_id { get; set; }
	}
}

/***********************************************************
**项目名称:NPlatform.Entity                                                                				   
**功能描述:	 User_Role   的摘要说明
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
	///User_Role，用户-角色
	/// </summary>
	[Serializable]
    [Table(name:"sys_user_role")]
    public partial class User_Role : EntityBase<string>, ISys
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
		/// 用户Id
		/// </summary>
		
        [Display(Name = "用户Id")]
        [StringLength(150)]
        public string user_id { get; set; }
	}
}

/***********************************************************
**项目名称:NPlatform.Entity                                                                				   
**功能描述:	 Group_User   的摘要说明
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
	///Group_User，用户组-用户
	/// </summary>
	[Serializable]
    [Table(name:"sys_group_user")]
    public partial class Group_User : EntityBase<string>, ISys
	{
		/// <summary>
		/// 用户id
		/// </summary>
		
        [Display(Name = "用户id")]
        [StringLength(150)]
        public string user_id { get; set; }

		/// <summary>
		/// 用户组Id
		/// </summary>
		
        [Display(Name = "用户组Id")]
        [StringLength(150)]
        public string user_group_id { get; set; }

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
	}
}

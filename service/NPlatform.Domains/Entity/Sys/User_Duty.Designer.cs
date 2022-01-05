/***********************************************************
**项目名称:NPlatform.Entity                                                                				   
**功能描述:	 User_Duty   的摘要说明
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
	///User_Duty，用户-职务
	/// </summary>
	[Serializable]
    [Table(name:"sys_user_duty")]
    public partial class User_Duty : EntityBase<string>, ISys
	{
		/// <summary>
		/// 员工编码
		/// </summary>
		
        [Display(Name = "员工编码")]
        [StringLength(300)]
        public string user_id { get; set; }

		/// <summary>
		/// 职务编码
		/// </summary>
		
        [Display(Name = "职务编码")]
        [StringLength(150)]
        public string duty_id { get; set; }

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

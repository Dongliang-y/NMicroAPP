/***********************************************************
**项目名称:NPlatform.Entity                                                                				   
**功能描述:	 Login_Record   的摘要说明
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
	///Login_Record，用户登录记录
	/// </summary>
	[Serializable]
    [Table(name:"sys_login_record")]
    public partial class Login_Record : EntityBase<string>, ISys
	{
		/// <summary>
		/// 内容
		/// </summary>
		
        [Display(Name = "内容")]
        [StringLength(1500)]
        public string contents { get; set; }

		/// <summary>
		/// 用户id
		/// </summary>
		
        [Display(Name = "用户id")]
        [StringLength(150)]
        public string user_Id { get; set; }

		/// <summary>
		/// 登录IP
		/// </summary>
		
        [Display(Name = "登录IP")]
        [StringLength(150)]
        public string ip { get; set; }

		/// <summary>
		/// 登录地点
		/// </summary>
		
        [Display(Name = "登录地点")]
        [StringLength(750)]
        public string address { get; set; }

		/// <summary>
		/// 登录时间
		/// </summary>
		
        [Display(Name = "登录时间")]
        public DateTime? login_time { get; set; }
	}
}

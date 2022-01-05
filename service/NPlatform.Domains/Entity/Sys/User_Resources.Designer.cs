/***********************************************************
**项目名称:NPlatform.Entity                                                                				   
**功能描述:	 User_Resources   的摘要说明
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
	///User_Resources，用户-资源
	/// </summary>
	[Serializable]
    [Table(name:"sys_user_resources")]
    public partial class User_Resources : EntityBase<string>, ISys
	{
		/// <summary>
		/// 用户Id
		/// </summary>
		
        [Display(Name = "用户Id")]
        [StringLength(150)]
        public string user_id { get; set; }

		/// <summary>
		/// 资源Id
		/// </summary>
		
        [Display(Name = "资源Id")]
        [StringLength(150)]
        public string resource_id { get; set; }

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

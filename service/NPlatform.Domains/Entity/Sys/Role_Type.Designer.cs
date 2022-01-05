/***********************************************************
**项目名称:NPlatform.Entity                                                                				   
**功能描述:	 Role_Type   的摘要说明
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
	///Role_Type，角色分类
	/// </summary>
	[Serializable]
    [Table(name:"sys_role_type")]
    public partial class Role_Type : EntityBase<string>, ISys
	{
		/// <summary>
		/// 分类名称
		/// </summary>
		
        [Display(Name = "分类名称")]
        [StringLength(150)]
        public string name { get; set; }

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

/***********************************************************
**项目名称:NPlatform.Entity                                                                				   
**功能描述:	 Role   的摘要说明
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
	///Role，角色
	/// </summary>
	[Serializable]
    [Table(name:"sys_role")]
    public partial class Role : EntityBase<string>, ISys
	{
		/// <summary>
		/// 名称
		/// </summary>
		
        [Display(Name = "名称")]
        [StringLength(1500)]
        public string name { get; set; }

		/// <summary>
		/// 角色分类id
		/// </summary>
		
        [Display(Name = "角色分类id")]
        [StringLength(150)]
        public string role_type_id { get; set; }

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
		/// 排序号
		/// </summary>
		
        [Display(Name = "排序号")]
        public int sorted_num { get; set; }
	}
}

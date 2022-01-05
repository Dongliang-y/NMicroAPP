/***********************************************************
**项目名称:NPlatform.Entity                                                                				   
**功能描述:	 Unit   的摘要说明
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
	///Unit，单位
	/// </summary>
	[Serializable]
    [Table(name:"sys_unit")]
    public partial class Unit : EntityBase<string>, ISys
	{
		/// <summary>
		/// 描述
		/// </summary>
		
        [Display(Name = "描述")]
        [StringLength(4500)]
        public string description { get; set; }

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

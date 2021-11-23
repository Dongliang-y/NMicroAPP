/***********************************************************
**项目名称:NPlatform.Entity                                                                				   
**功能描述:	 Position   的摘要说明
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
	///Position，岗位
	/// </summary>
	[Serializable]
    [Table(name:"sys_position")]
    public partial class Position : EntityBase<string>, ISys
	{
		/// <summary>
		/// 描述
		/// </summary>
		
        [Display(Name = "描述")]
        [StringLength(6000)]
        public string descrption { get; set; }

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
		/// 岗位名称
		/// </summary>
		
        [Display(Name = "岗位名称")]
        [StringLength(1500)]
        public string name { get; set; }
	}
}

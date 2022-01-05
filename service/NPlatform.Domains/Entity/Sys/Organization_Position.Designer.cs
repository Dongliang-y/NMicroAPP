/***********************************************************
**项目名称:NPlatform.Entity                                                                				   
**功能描述:	 Organization_Position   的摘要说明
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
	///Organization_Position，机构岗位设定
	/// </summary>
	[Serializable]
    [Table(name:"sys_organization_position")]
    public partial class Organization_Position : EntityBase<string>, ISys
	{
		/// <summary>
		/// 机构编码
		/// </summary>
		
        [Display(Name = "机构编码")]
        [StringLength(150)]
        public string organization_id { get; set; }

		/// <summary>
		/// 岗位名称
		/// </summary>
		
        [Display(Name = "岗位名称")]
        [StringLength(1500)]
        public string name { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
		
        [Display(Name = "描述")]
        [StringLength(6000)]
        public string descrption { get; set; }
	}
}

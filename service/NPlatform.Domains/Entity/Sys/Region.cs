/***********************************************************
**项目名称:NPlatform.Entity                                                                				   
**功能描述:	 Region   的摘要说明
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
	///Region，行政区划
	/// </summary>
	[Serializable]
    [Table(name:"sys_region")]
    public partial class Region : EntityBase<string>, ISys
	{
		/// <summary>
		/// 名称
		/// </summary>
		
        [Display(Name = "名称")]
        [StringLength(600)]
        public string name { get; set; }

		/// <summary>
		/// 简称
		/// </summary>
		
        [Display(Name = "简称")]
        [StringLength(600)]
        public string short_name { get; set; }

		/// <summary>
		/// 全称
		/// </summary>
		
        [Display(Name = "全称")]
        [StringLength(600)]
        public string all_name { get; set; }

		/// <summary>
		/// 等级
		/// </summary>
		
        [Display(Name = "等级")]
        public int at_level { get; set; }

		/// <summary>
		/// 编码
		/// </summary>
		
        [Display(Name = "编码")]
        [StringLength(150)]
        public string code { get; set; }

		/// <summary>
		/// 上级区划Id
		/// </summary>
		
        [Display(Name = "上级区划Id")]
        [StringLength(150)]
        public string parent_id { get; set; }

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
		/// 所在国家Id
		/// </summary>
		
        [Display(Name = "所在国家Id")]
        [StringLength(96)]
        public string country_id { get; set; }
	}
}

/***********************************************************
**项目名称:NPlatform.Entity                                                                				   
**功能描述:	 Country   的摘要说明
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
	///Country，国家
	/// </summary>
	[Serializable]
    [Table(name:"sys_country")]
    public partial class Country : EntityBase<string>, ISys
	{
		/// <summary>
		/// 国家编码
		/// </summary>
		
        [Display(Name = "国家编码")]
        [StringLength(150)]
        public string code { get; set; }

		/// <summary>
		/// 国家全称
		/// </summary>
		
        [Display(Name = "国家全称")]
        [StringLength(600)]
        public string all_name { get; set; }

		/// <summary>
		/// 国家名称
		/// </summary>
		
        [Display(Name = "国家名称")]
        [StringLength(600)]
        public string name { get; set; }

		/// <summary>
		/// 国家分类（DICT）
		/// </summary>
		
        [Display(Name = "国家分类（DICT）")]
        [StringLength(600)]
        public string type { get; set; }

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

		/// <summary>
		/// 国际域名缩写
		/// </summary>
		
        [Display(Name = "国际域名缩写")]
        [StringLength(600)]
        public string dnsname { get; set; }

		/// <summary>
		/// 时差
		/// </summary>
		
        [Display(Name = "时差")]
        [StringLength(600)]
        public string time_diff { get; set; }
	}
}

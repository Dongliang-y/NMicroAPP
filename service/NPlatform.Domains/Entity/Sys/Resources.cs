/***********************************************************
**项目名称:NPlatform.Entity                                                                				   
**功能描述:	 Resources   的摘要说明
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
	///Resources，资源
	/// </summary>
	[Serializable]
    [Table(name:"sys_resources")]
    public partial class Resources : EntityBase<string>, ISys
	{
		/// <summary>
		/// 资源名称
		/// </summary>
		
        [Display(Name = "资源名称")]
        [StringLength(1500)]
        public string name { get; set; }

		/// <summary>
		/// 资源别名
		/// </summary>
		
        [Display(Name = "资源别名")]
        [StringLength(1500)]
        public string alias_name { get; set; }

		/// <summary>
		/// 资源类型（DICT)
		/// </summary>
		
        [Display(Name = "资源类型（DICT)")]
        [StringLength(150)]
        public string res_type_code { get; set; }

		/// <summary>
		/// 资源所在分类（DICT)
		/// </summary>
		
        [Display(Name = "资源所在分类（DICT)")]
        [StringLength(1500)]
        public string res_addr_type { get; set; }

		/// <summary>
		/// 所在业务系统ID
		/// </summary>
		
        [Display(Name = "所在业务系统ID")]
        [StringLength(150)]
        public string system_id { get; set; }

		/// <summary>
		/// 图标路径
		/// </summary>
		
        [Display(Name = "图标路径")]
        [StringLength(1500)]
        public string icon_path { get; set; }

		/// <summary>
		/// 资源路径
		/// </summary>
		
        [Display(Name = "资源路径")]
        [StringLength(1500)]
        public string path { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
		
        [Display(Name = "描述")]
        [StringLength(4500)]
        public string description { get; set; }

		/// <summary>
		/// 资源层级编码
		/// </summary>
		
        [Display(Name = "资源层级编码")]
        [StringLength(1500)]
        public string level_code { get; set; }

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
		/// 父资源ID
		/// </summary>
		
        [Display(Name = "父资源ID")]
        [StringLength(150)]
        public string parent_id { get; set; }
	}
}

/***********************************************************
**项目名称:ZJJWFoundationPlatform.DTO                                                              				   
**功能描述:	数据传输层
**作    者: 	codesmith脚本生成                                         			   
**版 本 号:	1.0                                                  
**修改历史：
************************************************************/

namespace  NPlatform.DTO.Sys
{
	using System;
	using System.Runtime.Serialization;
	using NPlatform;
    using System.ComponentModel.DataAnnotations;
	/// <summary>
	/// Resources，资源，数据传输对象
	/// </summary>
   // [DataContract]
    public class ResourcesDTO:BaseDTO,IDTO
	{
		/// <summary>
		/// 编码
		/// </summary>
        [Display(Name ="编码")]
		[StringLength(150)]
        public string id { get; set; }

		/// <summary>
		/// 资源名称
		/// </summary>
        [Display(Name ="资源名称")]
		[StringLength(1500)]
        public string name { get; set; }

		/// <summary>
		/// 资源别名
		/// </summary>
        [Display(Name ="资源别名")]
		[StringLength(1500)]
        public string alias_name { get; set; }

		/// <summary>
		/// 资源类型（DICT)
		/// </summary>
        [Display(Name ="资源类型（DICT)")]
		[StringLength(150)]
        public string res_type_code { get; set; }

		/// <summary>
		/// 资源所在分类（DICT)
		/// </summary>
        [Display(Name ="资源所在分类（DICT)")]
		[StringLength(1500)]
        public string res_addr_type { get; set; }

		/// <summary>
		/// 所在业务系统ID
		/// </summary>
        [Display(Name ="所在业务系统ID")]
		[StringLength(150)]
        public string system_id { get; set; }

		/// <summary>
		/// 图标路径
		/// </summary>
        [Display(Name ="图标路径")]
		[StringLength(1500)]
        public string icon_path { get; set; }

		/// <summary>
		/// 资源路径
		/// </summary>
        [Display(Name ="资源路径")]
		[StringLength(1500)]
        public string path { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
        [Display(Name ="描述")]
		[StringLength(4500)]
        public string description { get; set; }

		/// <summary>
		/// 资源层级编码
		/// </summary>
        [Display(Name ="资源层级编码")]
		[StringLength(1500)]
        public string level_code { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
        [Display(Name ="创建时间")]
     [DisplayFormat(DataFormatString ="YYYY/MM/dd HH:mm:ss")]
        public DateTime create_time { get; set; }

		/// <summary>
		/// 创建人
		/// </summary>
        [Display(Name ="创建人")]
		[StringLength(1500)]
        public string create_user { get; set; }

		/// <summary>
		/// 排序号
		/// </summary>
        [Display(Name ="排序号")]
        public int sorted_num { get; set; }

		/// <summary>
		/// 父资源ID
		/// </summary>
        [Display(Name ="父资源ID")]
		[StringLength(150)]
        public string parent_id { get; set; }
	}
}

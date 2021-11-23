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
	/// Region，行政区划，数据传输对象
	/// </summary>
   // [DataContract]
    public class RegionDTO:BaseDTO,IDTO
	{
		/// <summary>
		/// 区划id
		/// </summary>
        [Display(Name ="区划id")]
		[StringLength(150)]
        public string id { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
        [Display(Name ="名称")]
		[StringLength(600)]
        public string name { get; set; }

		/// <summary>
		/// 简称
		/// </summary>
        [Display(Name ="简称")]
		[StringLength(600)]
        public string short_name { get; set; }

		/// <summary>
		/// 全称
		/// </summary>
        [Display(Name ="全称")]
		[StringLength(600)]
        public string all_name { get; set; }

		/// <summary>
		/// 等级
		/// </summary>
        [Display(Name ="等级")]
        public int at_level { get; set; }

		/// <summary>
		/// 编码
		/// </summary>
        [Display(Name ="编码")]
		[StringLength(150)]
        public string code { get; set; }

		/// <summary>
		/// 上级区划Id
		/// </summary>
        [Display(Name ="上级区划Id")]
		[StringLength(150)]
        public string parent_id { get; set; }

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
		/// 所在国家Id
		/// </summary>
        [Display(Name ="所在国家Id")]
		[StringLength(96)]
        public string country_id { get; set; }
	}
}

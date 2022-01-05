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
	/// Ruledata，sys_Rule_Data，数据传输对象
	/// </summary>
   // [DataContract]
    public class RuledataDTO:BaseDTO,IDTO
	{
		/// <summary>
		/// ID
		/// </summary>
        [Display(Name ="ID")]
		[StringLength(150)]
        public string ID { get; set; }

		/// <summary>
		/// 数据源名称
		/// </summary>
        [Display(Name ="数据源名称")]
		[StringLength(600)]
        public string DataName { get; set; }

		/// <summary>
		/// 数据筛选脚本
		/// </summary>
        [Display(Name ="数据筛选脚本")]
		[StringLength(300)]
        public string DataScript { get; set; }

		/// <summary>
		/// 角色ID
		/// </summary>
        [Display(Name ="角色ID")]
		[StringLength(300)]
        public string RoleId { get; set; }

		/// <summary>
		/// 关联表
		/// </summary>
        [Display(Name ="关联表")]
		[StringLength(600)]
        public string tabName { get; set; }
	}
}

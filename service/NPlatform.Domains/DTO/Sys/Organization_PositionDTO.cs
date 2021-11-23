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
	/// Organizationposition，机构岗位设定，数据传输对象
	/// </summary>
   // [DataContract]
    public class OrganizationpositionDTO:BaseDTO,IDTO
	{
		/// <summary>
		/// 编码
		/// </summary>
        [Display(Name ="编码")]
		[StringLength(96)]
        public string id { get; set; }

		/// <summary>
		/// 机构编码
		/// </summary>
        [Display(Name ="机构编码")]
		[StringLength(150)]
        public string organization_id { get; set; }

		/// <summary>
		/// 岗位名称
		/// </summary>
        [Display(Name ="岗位名称")]
		[StringLength(1500)]
        public string name { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
        [Display(Name ="描述")]
		[StringLength(6000)]
        public string descrption { get; set; }
	}
}

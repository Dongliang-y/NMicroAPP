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
	/// Group，用户组，数据传输对象
	/// </summary>
   // [DataContract]
    public class GroupDTO:BaseDTO,IDTO
	{
		/// <summary>
		/// 编码
		/// </summary>
        [Display(Name ="编码")]
		[StringLength(150)]
        public string id { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
        [Display(Name ="名称")]
		[StringLength(1500)]
        public string name { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
        [Display(Name ="描述")]
		[StringLength(6000)]
        public string descrption { get; set; }

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
	}
}

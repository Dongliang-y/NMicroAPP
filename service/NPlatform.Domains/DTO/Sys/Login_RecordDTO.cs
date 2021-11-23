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
	/// Loginrecord，用户登录记录，数据传输对象
	/// </summary>
   // [DataContract]
    public class LoginrecordDTO:BaseDTO,IDTO
	{
		/// <summary>
		/// 编码
		/// </summary>
        [Display(Name ="编码")]
		[StringLength(96)]
        public string id { get; set; }

		/// <summary>
		/// 内容
		/// </summary>
        [Display(Name ="内容")]
		[StringLength(1500)]
        public string contents { get; set; }

		/// <summary>
		/// 用户id
		/// </summary>
        [Display(Name ="用户id")]
		[StringLength(150)]
        public string user_Id { get; set; }

		/// <summary>
		/// 登录IP
		/// </summary>
        [Display(Name ="登录IP")]
		[StringLength(150)]
        public string ip { get; set; }

		/// <summary>
		/// 登录地点
		/// </summary>
        [Display(Name ="登录地点")]
		[StringLength(750)]
        public string address { get; set; }

		/// <summary>
		/// 登录时间
		/// </summary>
        [Display(Name ="登录时间")]
     [DisplayFormat(DataFormatString ="YYYY/MM/dd HH:mm:ss")]
        public DateTime login_time { get; set; }
	}
}

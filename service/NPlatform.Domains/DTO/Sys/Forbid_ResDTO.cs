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
	/// Forbidres，禁止资源，数据传输对象
	/// </summary>
   // [DataContract]
    public class ForbidresDTO:BaseDTO,IDTO
	{
		/// <summary>
		/// 编号
		/// </summary>
        [Display(Name ="编号")]
		[StringLength(96)]
        public string id { get; set; }

		/// <summary>
		/// 资源id
		/// </summary>
        [Display(Name ="资源id")]
		[StringLength(150)]
        public string resources_id { get; set; }

		/// <summary>
		/// 用户id
		/// </summary>
        [Display(Name ="用户id")]
		[StringLength(150)]
        public string user_id { get; set; }

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

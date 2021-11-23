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
	/// Recvmessage，接收消息，数据传输对象
	/// </summary>
   // [DataContract]
    public class RecvmessageDTO:BaseDTO,IDTO
	{
		/// <summary>
		/// 主键
		/// </summary>
        [Display(Name ="主键")]
		[StringLength(96)]
        public string id { get; set; }

		/// <summary>
		/// 站内信ID
		/// </summary>
        [Display(Name ="站内信ID")]
		[StringLength(96)]
        public string message_id { get; set; }

		/// <summary>
		/// 用户ID
		/// </summary>
        [Display(Name ="用户ID")]
		[StringLength(96)]
        public string user_id { get; set; }

		/// <summary>
		/// 阅读时间
		/// </summary>
        [Display(Name ="阅读时间")]
     [DisplayFormat(DataFormatString ="YYYY/MM/dd HH:mm:ss")]
        public DateTime read_time { get; set; }

		/// <summary>
		/// 阅读状态(DICT)
		/// </summary>
        [Display(Name ="阅读状态(DICT)")]
		[StringLength(300)]
        public string state { get; set; }

		/// <summary>
		/// 逻辑删除
		/// </summary>
        [Display(Name ="逻辑删除")]
        public int logical_delete { get; set; }

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

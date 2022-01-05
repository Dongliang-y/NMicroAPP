/***********************************************************
**项目名称:NPlatform.Entity                                                                				   
**功能描述:	 Message   的摘要说明
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
	///Message，消息表
	/// </summary>
	[Serializable]
    [Table(name:"sys_message")]
    public partial class Message : EntityBase<string>, ISys
	{
		/// <summary>
		/// 发送用户ID
		/// </summary>
		
        [Display(Name = "发送用户ID")]
        [StringLength(300)]
        public string user_id { get; set; }

		/// <summary>
		/// 消息内容
		/// </summary>
		
        [Display(Name = "消息内容")]
        [StringLength(65535)]
        public string Contents { get; set; }

		/// <summary>
		/// 发送时间
		/// </summary>
		
        [Display(Name = "发送时间")]
        public DateTime? send_time { get; set; }

		/// <summary>
		/// 消息类型(DICT)
		/// </summary>
		
        [Display(Name = "消息类型(DICT)")]
        [StringLength(300)]
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
	}
}

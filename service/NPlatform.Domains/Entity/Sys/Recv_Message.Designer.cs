/***********************************************************
**项目名称:NPlatform.Entity                                                                				   
**功能描述:	 Recv_Message   的摘要说明
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
	///Recv_Message，接收消息
	/// </summary>
	[Serializable]
    [Table(name:"sys_recv_message")]
    public partial class Recv_Message : EntityBase<string>, ISys
	{
		/// <summary>
		/// 站内信ID
		/// </summary>
		
        [Display(Name = "站内信ID")]
        [StringLength(96)]
        public string message_id { get; set; }

		/// <summary>
		/// 用户ID
		/// </summary>
		
        [Display(Name = "用户ID")]
        [StringLength(96)]
        public string user_id { get; set; }

		/// <summary>
		/// 阅读时间
		/// </summary>
		
        [Display(Name = "阅读时间")]
        public DateTime? read_time { get; set; }

		/// <summary>
		/// 阅读状态(DICT)
		/// </summary>
		
        [Display(Name = "阅读状态(DICT)")]
        [StringLength(300)]
        public string state { get; set; }

		/// <summary>
		/// 逻辑删除
		/// </summary>
		
        [Display(Name = "逻辑删除")]
        public int logical_delete { get; set; }

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

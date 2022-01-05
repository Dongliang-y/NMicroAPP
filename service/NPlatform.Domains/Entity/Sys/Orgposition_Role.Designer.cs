/***********************************************************
**项目名称:NPlatform.Entity                                                                				   
**功能描述:	 Orgposition_Role   的摘要说明
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
	///Orgposition_Role，sys_orgposition_role
	/// </summary>
	[Serializable]
    [Table(name:"sys_orgposition_role")]
    public partial class Orgposition_Role : EntityBase<string>, ISys
	{
		/// <summary>
		/// 机构岗位ID
		/// </summary>
		
        [Display(Name = "机构岗位ID")]
        [StringLength(96)]
        public string PositionId { get; set; }

		/// <summary>
		/// 角色ID
		/// </summary>
		
        [Display(Name = "角色ID")]
        [StringLength(96)]
        public string RoleId { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		
        [Display(Name = "创建时间")]
        public DateTime? create_time { get; set; }

		/// <summary>
		/// 创建人
		/// </summary>
		
        [Display(Name = "创建人")]
        [StringLength(150)]
        public string create_user { get; set; }
	}
}

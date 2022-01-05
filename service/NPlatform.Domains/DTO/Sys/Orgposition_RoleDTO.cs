﻿/***********************************************************
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
	/// Orgpositionrole，sys_orgposition_role，数据传输对象
	/// </summary>
   // [DataContract]
    public class OrgpositionroleDTO:BaseDTO,IDTO
	{
		/// <summary>
		/// 
		/// </summary>
        [Display(Name ="")]
		[StringLength(96)]
        public string id { get; set; }

		/// <summary>
		/// 机构岗位ID
		/// </summary>
        [Display(Name ="机构岗位ID")]
		[StringLength(96)]
        public string PositionId { get; set; }

		/// <summary>
		/// 角色ID
		/// </summary>
        [Display(Name ="角色ID")]
		[StringLength(96)]
        public string RoleId { get; set; }

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
		[StringLength(150)]
        public string create_user { get; set; }
	}
}

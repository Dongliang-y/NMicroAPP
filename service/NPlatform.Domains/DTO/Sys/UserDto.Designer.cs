/***********************************************************
**项目名称:ZJJWFoundationPlatform.Dto                                                             	
**功能描述:	数据传输层
**作    者: 	codesmith脚本生成                                         			   
**版 本 号:	1.0                                                  
**修改历史：
************************************************************/

namespace  NPlatform.Dto.Sys
{
	using System;
	using System.Runtime.Serialization;
	using NPlatform;
    using System.ComponentModel.DataAnnotations;
	/// <summary>
	/// User，用户，数据传输对象
	/// </summary>
   // [DataContract]
    public class UserDto:BaseDto,IDto
	{
		/// <summary>
		/// 员工编码
		/// </summary>
        [Display(Name ="员工编码")]
		[StringLength(300)]
        public string id { get; set; }

		/// <summary>
		/// 机构编码Id
		/// </summary>
        [Display(Name ="机构编码Id")]
		[StringLength(150)]
        public string org_id { get; set; }

		/// <summary>
		/// 所在区划id
		/// </summary>
        [Display(Name ="所在区划id")]
		[StringLength(150)]
        public string region_id { get; set; }

		/// <summary>
		/// 姓名
		/// </summary>
        [Display(Name ="姓名")]
		[StringLength(150)]
        public string name { get; set; }

		/// <summary>
		/// 昵称
		/// </summary>
        [Display(Name ="昵称")]
		[StringLength(150)]
        public string nick_name { get; set; }

		/// <summary>
		/// 登录名
		/// </summary>
        [Display(Name ="登录名")]
		[StringLength(150)]
        public string login_name { get; set; }

		/// <summary>
		/// 登录密码
		/// </summary>
        [Display(Name ="登录密码")]
		[StringLength(150)]
        public string password { get; set; }

		/// <summary>
		/// 姓名拼音
		/// </summary>
        [Display(Name ="姓名拼音")]
		[StringLength(450)]
        public string pingyin_name { get; set; }

		/// <summary>
		/// 头像(附件的Id)
		/// </summary>
        [Display(Name ="头像(附件的Id)")]
		[StringLength(450)]
        public string head_icon { get; set; }

		/// <summary>
		/// 籍贯
		/// </summary>
        [Display(Name ="籍贯")]
		[StringLength(450)]
        public string native_place { get; set; }

		/// <summary>
		/// 民族(DICT)
		/// </summary>
        [Display(Name ="民族(DICT)")]
		[StringLength(150)]
        public string national { get; set; }

		/// <summary>
		/// 身份证
		/// </summary>
        [Display(Name ="身份证")]
		[StringLength(150)]
        public string card_num { get; set; }

		/// <summary>
		/// 手机号码
		/// </summary>
        [Display(Name ="手机号码")]
		[StringLength(150)]
        public string mobile_num { get; set; }

		/// <summary>
		/// 办公电话
		/// </summary>
        [Display(Name ="办公电话")]
		[StringLength(150)]
        public string office_tel_num { get; set; }

		/// <summary>
		/// 工作邮箱
		/// </summary>
        [Display(Name ="工作邮箱")]
		[StringLength(150)]
        public string email { get; set; }

		/// <summary>
		/// 微信号
		/// </summary>
        [Display(Name ="微信号")]
		[StringLength(750)]
        public string weixin { get; set; }

		/// <summary>
		/// QQ号
		/// </summary>
        [Display(Name ="QQ号")]
		[StringLength(150)]
        public string qq { get; set; }

		/// <summary>
		/// 工作地址
		/// </summary>
        [Display(Name ="工作地址")]
		[StringLength(1500)]
        public string work_address { get; set; }

		/// <summary>
		/// 性别(DICT)
		/// </summary>
        [Display(Name ="性别(DICT)")]
		[StringLength(150)]
        public string sex { get; set; }

		/// <summary>
		/// 出生年月
		/// </summary>
        [Display(Name ="出生年月")]
     [DisplayFormat(DataFormatString ="YYYY/MM/dd")]
        public DateTime birthday { get; set; }

		/// <summary>
		/// 逻辑删除
		/// </summary>
        [Display(Name ="逻辑删除")]
        public int logic_delete { get; set; }

		/// <summary>
		/// 个人介绍
		/// </summary>
        [Display(Name ="个人介绍")]
		[StringLength(6000)]
        public string descriptions { get; set; }

		/// <summary>
		/// 注册方式(DICT)
		/// </summary>
        [Display(Name ="注册方式(DICT)")]
		[StringLength(450)]
        public string regist_way { get; set; }

		/// <summary>
		/// 注册时间
		/// </summary>
        [Display(Name ="注册时间")]
     [DisplayFormat(DataFormatString ="YYYY/MM/dd HH:mm:ss")]
        public DateTime regist_time { get; set; }

		/// <summary>
		/// 注册设备
		/// </summary>
        [Display(Name ="注册设备")]
		[StringLength(750)]
        public string regist_device { get; set; }

		/// <summary>
		/// 上次登录时间
		/// </summary>
        [Display(Name ="上次登录时间")]
		[StringLength(750)]
        public string last_login_time { get; set; }

		/// <summary>
		/// 用户状态(DICT)
		/// </summary>
        [Display(Name ="用户状态(DICT)")]
		[StringLength(750)]
        public string user_state { get; set; }

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

		/// <summary>
		/// 业务ID
		/// </summary>
        [Display(Name ="业务ID")]
		[StringLength(450)]
        public string system_id { get; set; }

		/// <summary>
		/// 排序号
		/// </summary>
        [Display(Name ="排序号")]
        public int sorted_num { get; set; }

		/// <summary>
		/// AppToken
		/// </summary>
        [Display(Name ="AppToken")]
		[StringLength(750)]
        public string app_token { get; set; }

		/// <summary>
		/// 上次登录设备
		/// </summary>
        [Display(Name ="上次登录设备")]
		[StringLength(750)]
        public string last_login_device { get; set; }
	}
}

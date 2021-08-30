#region << 版 本 注 释 >>

/*----------------------------------------------------------------
* 类 名 称 ：AuthInfoDto
* 类 描 述 ：
* 所在的域 ：DESKTOP-QUTJPVF
* 命名空间 ：NPlatform.AuthorizationDto
* 机器名称 ：DESKTOP-QUTJPVF
* CLR 版本 ：4.0.30319.42000
* 作    者 ：王盈攀
* 创建时间 ：2018-12-29 9:59:42
* 更新时间 ：2018-12-29 9:59:42
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ 王盈攀 2018. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/

#endregion << 版 本 注 释 >>

using System;
using System.Collections.Generic;
using System.Text;
using NPlatform;

namespace NPlatform
{
    /// <summary>
    /// 授权数据
    /// </summary>
    public class AuthInfoVO : IVO
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 账户名
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 中文名
        /// </summary>
        public string CnName { get; set; }

        /// <summary>
        /// CA 的  KeySN
        /// </summary>
        public string CA { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int? UserSex { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserPic { get; set; }

        /// <summary>
        /// 签字图片
        /// </summary>
        /// <value>The sign pic.</value>
        public string SignPic { get; set; }

        /// <summary>
        /// 客户端Id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>

        public string Telephone { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string IdNumber { get; set; }

        /// <summary>
        /// 令牌
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 设备推送ID
        /// </summary>
        public  string RegisterId { get; set; }

        /// <summary>
        /// 当前登录角色
        /// </summary>
        public string CurrentRoleId { get; set; } = "";
        /// <summary>
        /// 当前登录角色类型
        /// </summary>
        public string CurrentRoleType { get; set; }

        /// <summary>
        /// 默认项目
        /// </summary>
        public string DefaultOrgCode { get; set; }

        /// <summary>
        /// 默认项目名称
        /// </summary>
        public string DefaultOrgName { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Post { get; set; }

        /// <summary>
        /// 考勤员  0普通员工 1考勤员 2考勤管理员
        /// </summary>
        public int? CheckWorker { get; set; } 

        /// <summary>
        /// Macid
        /// </summary>
        public string Macid { get; set; }

        /// <summary>
        /// KeySN
        /// </summary>
        public string KeySN { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
    }
}
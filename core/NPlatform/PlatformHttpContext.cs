/*************************************************************************************
  * CLR版本：       4.0.30319.42000
  * 类 名 称：       PlatformContext
  * 机器名称：       DESKTOP123
  * 命名空间：       NPlatform.Infrastructure
  * 文 件 名：       PlatformContext
  * 创建时间：       2020-5-21 9:19:34
  * 作    者：          xxx
  * 说   明：。。。。。
  * 修改时间：
  * 修 改 人：
*************************************************************************************/
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace NPlatform
{
    /// <summary>
    ///  平台  http 上下文
    /// </summary>
    public interface IPlatformHttpContext
    {
        /// <summary>
        /// http 上下文
        /// </summary>
        HttpContext Context
        {
            get;
        }
        /// <summary>
        /// Claims
        /// </summary>
        IEnumerable<Claim> Claims { get; }

        /// <summary>
        /// 用户ID
        /// </summary>
        string UserID { get; }

        /// <summary>
        /// 登录账号
        /// </summary>
        string Account { get; }

        /// <summary>
        /// 中文名
        /// </summary>
        string CName { get; }
    }
    /// <summary>
    /// 平台  http 上下文
    /// </summary>
    public class PlatformHttpContext: IPlatformHttpContext
    {
        /// <summary>
        ///  平台  http 上下文
        /// </summary>
        /// <param name="Accessor"></param>
        public PlatformHttpContext(IHttpContextAccessor Accessor)
        {
            _Accessor = Accessor;
        }
        private IHttpContextAccessor _Accessor;
        /// <summary>
        /// http 上下文
        /// </summary>
        public  HttpContext Context
        {
            get
            {
                return _Accessor.HttpContext;
            }
        }

        /// <summary>
        /// 登陆用户的附加信息
        /// </summary>
        public  IEnumerable<Claim>  Claims
        {
            get
            {
                return _Accessor.HttpContext?.User?.Claims;
            }
        }

        /// <summary>
        /// 用户ID（登录后可用）
        /// </summary>
        public string UserID
        {
            get
            {
                if (Claims.Any(t => t.Type == "id"))
                {
                    var uid = Claims.FirstOrDefault(t => t.Type == "id");
                    return uid != null ? uid.Value : "";
                }
                return "";
            }
        }

        /// <summary>
        /// 账号（登录后可用）
        /// </summary>
        public string Account
        {
            get
            {
                if (Claims.Any(t => t.Type == "name"))
                {
                    var account = Claims.FirstOrDefault(t => t.Type == "name");
                    return account != null ? account.Value : "";
                }
                return "";
            }
        }

        /// <summary>
        /// 中文名（登录后可用）
        /// </summary>
        public string CName
        {
            get
            {
                if (Claims.Any(t => t.Type == "given_name"))
                {
                    var CnName = Claims.FirstOrDefault(t => t.Type == "given_name");
                    return CnName != null ? CnName.Value:"";
                }
                return "";
            }
        }
    }
}

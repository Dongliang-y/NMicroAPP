/*************************************************************************************
  * CLR版本：       4.0.30319.42000
  * 类 名 称：       AuthInfo
  * 机器名称：       DESKTOP123
  * 命名空间：       EPlatform.Infrastructure.Config
  * 文 件 名：       AuthInfo
  * 创建时间：       2020-3-24 9:25:28
  * 作    者：          xxx
  * 说   明：。。。。。
  * 修改时间：
  * 修 改 人：
*************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPlatform.Config
{
    /// <summary>
    /// Auth 服务器信息配置
    /// </summary>
    public class AuthServerConfig
    {
        private string authorizationUrl;
        private string accessTokenLifetime;

        /// <summary>
        /// ApiScope 服务端有效
        /// </summary>
        public CustomDic ApiScope { get; set; }

        /// <summary>
        /// 授权中心的地址
        /// </summary>
        public string AuthorizationUrl
        {
            get
            {
                return ApolloConfiguration.GetConfig(nameof(NPlatformConfig), nameof(AuthorizationUrl), this.authorizationUrl);
            }
            set
            {
                this.authorizationUrl = value;
            }
        }

        /// <summary>
        /// Token 过期时间
        /// </summary>
        public string AccessTokenLifetime
        {
            get
            {
                return ApolloConfiguration.GetConfig(nameof(NPlatformConfig), nameof(AccessTokenLifetime), this.accessTokenLifetime);
            }
            set
            {
                this.accessTokenLifetime = value;
            }
        }
    }
}

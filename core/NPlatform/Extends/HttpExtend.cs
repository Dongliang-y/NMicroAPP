using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPlatform.Extends
{
    /// <summary>
    /// HTTP扩展类型
    /// </summary>
    public static class HttpExtend
    {
        /// <summary>
        /// 获取当前请求的完整路径
        /// 如：http://www.xxx.com/api/user/delete?userid=123
        /// </summary>
        /// <param name="request">HttpRequest</param>
        /// <returns></returns>
        public static string GetAbsoluteUri(this HttpRequest request)
        {
            return new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .Append(request.PathBase)
                .Append(request.Path)
                .Append(request.QueryString)
                .ToString();
        }
        /// <summary>
        /// 获取当前请求的根路径  
        /// 如 http://www.xxx.com
        /// </summary>
        /// <param name="request">HttpRequest</param>
        /// <returns></returns>
        public static string GetBaseUrl(this HttpRequest request)
        {
            return new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .ToString();
        }
    }
}

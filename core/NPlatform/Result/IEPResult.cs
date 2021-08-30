/********************************************************************************

** auth： DongliangYi

** date： 2017/8/24 12:34:28

** desc： 尚未编写描述

** Ver.:  V1.0.0

*********************************************************************************/
using System;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NPlatform.Infrastructure;

namespace NPlatform.Result
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEPResult
    {
        /// <summary>
        /// Gets or sets 信息
        /// </summary>
        [DataMember]
                [JsonProperty(PropertyName = "message")]
        string Message { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 成功否
        /// </summary>
        [DataMember]
        [JsonProperty(PropertyName = "success")]
        bool Success { get; set; }
    }
}
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using NPlatform;


namespace NPlatform.Result
{
    /// <summary>
    /// 基于字符串提供 HTTP 内容。
    /// </summary>
    public class EPResult : IEPResult
    {
        /// <summary>
        /// 消息
        /// </summary>
        [DataMember]
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        /// <summary>
        /// 是否成功！
        /// </summary>
        [DataMember]
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; } = true;

        /// <summary>
        /// 数据列表内容对象
        /// </summary>
        public EPResult(string content) 
        {
            Message = content;
        }

        /// <summary>
        /// 数据列表内容对象
        /// </summary>
        public EPResult()
        {
        }
    }
}
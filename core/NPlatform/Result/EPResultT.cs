/********************************************************************************

** auth： DongliangYi

** date： 2017/8/23 13:21:01

** desc： 尚未编写描述

** Ver.:  V1.0.0

*********************************************************************************/
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using NPlatform;

namespace NPlatform.Result
{

    /// <summary>
    /// 操作结果
    /// </summary>
    public class EPResult<T> : IEPResult<T>
    {
        /// <summary>
        /// 需要返回的数据对象
        /// </summary>
        [DataMember]
        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        [DataMember]
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        [DataMember]
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; } = true;

        /// <summary>
        /// 操作结果
        /// </summary>
        public EPResult() { }
        /// <summary>
        /// 成功的结果内容
        /// </summary>
        /// <param name="message">消息</param>
        public EPResult(string message) 
        {
            this.Message = message;
        }
        /// <summary>
        /// 成功的结果内容
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">消息</param>
        public EPResult(string message, T data)
        {
            this.Message = message;
            this.Data = data;
        }

        /// <summary>
        /// 操作结果
        /// </summary>
        /// <param name="success">是否成功，默认true</param>
        /// <param name="message">消息</param>
        /// <param name="result">T 类型对象</param>
        public EPResult(bool success, string message, T result)
        {
            this.Success = success;
            this.Message = message;
            this.Data = result;
        }

    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NPlatform.Result
{
    /// <summary>
    /// 错误信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ErrorResult<T> : IEPResult<T>, IListResult<T>, ITreeResult<T>, IEPResult
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="ex"></param>
        public ErrorResult(Exception ex)
        {
            this.Success = false;
            this.Message = ex.Message;
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="message"></param>
        public ErrorResult(string message)
        {
            this.Success = false;
            this.Message = message;
        }

        /// <summary>
        /// 数据，无需赋值
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
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
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; } = false;

        /// <summary>
        /// Total，无需赋值
        /// </summary>
        [JsonIgnore]
        [System.Xml.Serialization.XmlIgnore]
        public long Total { get; set; }

        /// <summary>
        /// Total，无需赋值
        /// </summary>
        [JsonIgnore]
        [System.Xml.Serialization.XmlIgnore]
        IEnumerable<T> IListResult<T>.Data { get; set; } = null;

        public IList<T> ToList()
        {
            throw new NotImplementedException();
        }
    }
}

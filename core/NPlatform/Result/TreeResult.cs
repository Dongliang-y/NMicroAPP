using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NPlatform.Result
{
    /// <summary>
    /// 树类型的结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeResult<T> : List<T>, ITreeResult<T>
    {
        /// <summary>
        /// 树类型的结构
        /// </summary>
        public TreeResult() { }

        /// <summary>
        /// 消息
        /// </summary>
        [DataMember]
        [JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        [DataMember]
        [JsonPropertyName("success")]
        public bool Success { get; set; } = true;
    }
}

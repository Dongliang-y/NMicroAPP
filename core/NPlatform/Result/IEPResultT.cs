/********************************************************************************

** auth： DongliangYi

** date： 2017/8/23 13:21:01

** desc： 尚未编写描述

** Ver.:  V1.0.0

*********************************************************************************/

using Newtonsoft.Json;

namespace NPlatform.Result
{
    /// <summary>
    /// 成功的结果内容
    /// </summary>
    public interface IEPResult<T> : IEPResult
    {
        /// <summary>
        /// 需要返回的数据对象
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        T Data { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        string Message { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        [JsonProperty(PropertyName = "success")]
        bool Success { get; set; }
    }
}
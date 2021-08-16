﻿namespace ZJJWEPlatform
{
    /// <summary>
    /// 第三方响应结果异常
    /// </summary>
    public class ThirdPartyResultException : ZJJWEPlatformException
    {
        /// <summary>
        /// 第三方响应结果异常
        /// </summary>
        public ThirdPartyResultException(string msg)
            : base(msg, nameof(ThirdPartyResultException))
        {
        }
    }
}
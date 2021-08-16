/***********************************************************
**项目名称:	                                                                  				   
**功能描述:	  的摘要说明
**作    者: 	易栋梁                                         			   
**版 本 号:	1.0                                             			   
**创建日期： 2015/12/29 16:15:32
**修改历史：
************************************************************/

namespace ZJJWEPlatform
{
    using System;

    /// <summary>
    /// 异常基类
    /// </summary>
    public class ZJJWEPlatformException : Exception, IZJJWEPlatformException
    {
        /// <summary>
        /// 异常基类
        /// </summary>
        public ZJJWEPlatformException(string msg, Exception ex, string errorCode)
            : base(msg, ex)
        {
            ErrorCode = errorCode;
            Message = msg;
        }

        /// <summary>
        /// 异常基类
        /// </summary>
        public ZJJWEPlatformException(string msg, string errorCode)
            : base(msg)
        {
            ErrorCode = errorCode;
            Message = msg;
        }

        /// <summary>
        /// 错误码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public new string Message { get; set; }
    }
}
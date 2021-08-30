/***********************************************************
**项目名称:	                                                                  				   
**功能描述:	  的摘要说明
**作    者: 	易栋梁                                         			   
**版 本 号:	1.0                                             			   
**创建日期： 2015/12/29 16:15:32
**修改历史：
************************************************************/

namespace NPlatform
{
    /// <summary>
    /// 平台异常基类
    /// </summary>
    public interface INPlatformException
    {
        /// <summary>
        /// 错误码
        /// </summary>
        string ErrorCode { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        string Message { get; set; }
    }
}
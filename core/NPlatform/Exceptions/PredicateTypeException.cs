/********************************************************************************

** auth： DongliangYi

** date： 2017/8/17 16:34:26

** desc： 尚未编写描述

** Ver.:  V1.0.0

*********************************************************************************/

namespace NPlatform.Repositories.Exceptions
{
    /// <summary>
    /// 查询谓词类型错误
    /// </summary>
    public class PredicateTypeException : NPlatformException
    {
        /// <summary>
        /// 查询谓词类型错误
        /// </summary>
        /// <param name="msg">异常信息</param>
        public PredicateTypeException(string msg)
            : base(msg, "PredicateTypeException")
        {
        }
    }
}
/********************************************************************************

** auth： DongliangYi

** date： 2016/8/29 10:08:46

** desc：Dto接口

** Ver.:  V1.0.0

*********************************************************************************/

using System;
using NPlatform.Result;

namespace NPlatform
{
    /// <summary>
    /// Dto 接口
    /// </summary>
    public interface IDTO
    {
        /// <summary>
        /// DTO值的合法性校验
        /// </summary>
        /// <returns></returns>
        ValidateResult Validate();
    }
}
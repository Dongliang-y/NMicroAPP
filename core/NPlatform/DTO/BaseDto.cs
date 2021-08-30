/********************************************************************************

** auth： DongliangYi

** date： 2016/8/29 10:08:46

** desc：Dto接口 基类

** Ver.:  V1.0.0

*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NPlatform.Result;

namespace NPlatform.DTO
{
    /// <summary>
    /// DTO 基类
    /// </summary>
    /// <typeparam name="T">子类类型</typeparam>
    public class BaseDTO : IDTO
    {
        /// <summary>
        /// 为了兼容老接口,实现在service层的主动校验实体属性
        /// </summary>
        /// <returns></returns>
        public ValidateResult Validate()
        {
            return this.Validates();
        }
    }
}

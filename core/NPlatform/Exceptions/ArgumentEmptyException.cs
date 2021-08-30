#region << 版 本 注 释 >>

/*----------------------------------------------------------------
* 类 名 称 ：ArgumentEmptyException
* 类 描 述 ：
* 所在的域 ：DESKTOP-UU3LV10
* 命名空间 ：NPlatform.Exceptions
* CLR 版本 ：4.0.30319.42000
* 作    者 ：Dongliang Yi
* 创建时间 ：2019/1/14 17:12:27
* 更新时间 ：2019/1/14 17:12:27
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ yidon 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/

#endregion

namespace NPlatform
{
    /// <summary>
    /// 参数为空异常
    /// </summary>
    public class ArgumentEmptyException : LogicException
    {
        /// <summary>
        /// 参数为空异常
        /// </summary>
        public ArgumentEmptyException(string paramName)
            : base($"参数“{paramName}”，不能为空", "ArgumentEmptyException")
        {
        }
    }
}
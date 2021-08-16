#region << 版 本 注 释 >>

/*----------------------------------------------------------------
* 项目名称 ：ZJJWEPlatform.Domains.Exceptions
* 类 名 称 ：ValidateException
* 类 描 述 ：
* 命名空间 ：ZJJWEPlatform.Domains.Exceptions
* CLR 版本 ：4.0.30319.42000
* 作    者 ：DongliangYi
* 创建时间 ：2018-12-13 18:06:23
* 更新时间 ：2018-12-13 18:06:23
* 版 本 号 ：v1.0.0.0
//----------------------------------------------------------------*/

#endregion

namespace ZJJWEPlatform
{
    /// <summary>
    /// 校验失败
    /// </summary>
    public class ValidateException : LogicException
    {
        /// <summary>
        /// 校验失败
        /// </summary>
        public ValidateException(string msg)
            : base(msg, "ValidateException")
        {
        }
    }
}
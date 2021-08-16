namespace ZJJWEPlatform
{
    /// <summary>
    /// 操作异常
    /// </summary>
    public class OperateException : ZJJWEPlatformException
    {
        /// <summary>
        /// 操作异常
        /// </summary>
        public OperateException(string msg)
            : base(msg, nameof(OperateException))
        {
        }
    }
}
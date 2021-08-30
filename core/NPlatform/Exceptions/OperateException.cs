namespace NPlatform
{
    /// <summary>
    /// 操作异常
    /// </summary>
    public class OperateException : NPlatformException
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
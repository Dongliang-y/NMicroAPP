namespace NPlatform
{
    /// <summary>
    /// 逻辑异常
    /// </summary>
    public class LogicException : NPlatformException
    {
        /// <summary>
        /// 逻辑异常
        /// </summary>
        public LogicException(string msg, string code)
            : base(msg, code)
        {
        }
    }
}
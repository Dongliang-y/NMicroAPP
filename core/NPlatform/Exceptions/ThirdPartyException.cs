namespace NPlatform
{
    /// <summary>
    /// 第三方系统异常
    /// </summary>
    public class ThirdPartyException : NPlatformException
    {
        /// <summary>
        /// 第三方系统异常
        /// </summary>
        public ThirdPartyException(string msg)
            : base(msg, nameof(ThirdPartyException))
        {
        }
    }
}
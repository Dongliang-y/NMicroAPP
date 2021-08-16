namespace ZJJWEPlatform
{
    /// <summary>
    /// 领域层异常
    /// </summary>
    public class DomainException : ZJJWEPlatformException
    {
        /// <summary>
        /// 领域层异常
        /// </summary>
        public DomainException(string msg)
            : base(msg, "DomainException")
        {
        }
    }
}
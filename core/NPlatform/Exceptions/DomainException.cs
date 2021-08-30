namespace NPlatform
{
    /// <summary>
    /// 领域层异常
    /// </summary>
    public class DomainException : NPlatformException
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
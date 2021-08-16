namespace ZJJWEPlatform
{
    /// <summary>
    /// 运行环境异常
    /// </summary>
    public class EnvironmentException : ZJJWEPlatformException
    {
        /// <summary>
        /// 运行环境异常
        /// </summary>
        public EnvironmentException(string msg)
            : base(msg, nameof(EnvironmentException))
        {
        }
    }
}
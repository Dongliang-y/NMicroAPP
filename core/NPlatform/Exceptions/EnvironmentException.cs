namespace NPlatform
{
    /// <summary>
    /// 运行环境异常
    /// </summary>
    public class EnvironmentException : NPlatformException
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
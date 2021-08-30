namespace NPlatform
{
    /// <summary>
    /// 配置异常
    /// </summary>
    public class ConfigException : NPlatformException
    {
        /// <summary>
        /// 配置异常
        /// </summary>
        public ConfigException(string msg)
            : base(msg, nameof(ConfigException))
        {
        }
    }
}
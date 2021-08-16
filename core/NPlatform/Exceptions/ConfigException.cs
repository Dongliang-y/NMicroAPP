namespace ZJJWEPlatform
{
    /// <summary>
    /// 配置异常
    /// </summary>
    public class ConfigException : ZJJWEPlatformException
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
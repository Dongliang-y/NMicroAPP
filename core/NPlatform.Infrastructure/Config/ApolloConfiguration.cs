using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Core;
using Com.Ctrip.Framework.Apollo.Model;
using System;

namespace NPlatform.Config
{
    /// <summary>
    /// apollo配置获取
    /// </summary>
    public static class ApolloConfiguration
    {
        private static Com.Ctrip.Framework.Apollo.IConfig _config;
        public static ConfigChangeEvent OnChangeConfig;
        public static void Init(string configID)
        {
            _config = ApolloConfigurationManager.GetConfig($"{configID}.{ConfigConsts.NamespaceApplication}.json",
                ConfigConsts.NamespaceApplication).GetAwaiter().GetResult();
            _config.ConfigChanged += OnChanged;
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="key">key值</param>
        /// <param name="defaultValue">如果没找到key,则返回默认值</param>
        /// <returns></returns>
        public static string GetConfig(string pre,string key,string defaultValue)
        {
            key = $"{pre}-{key}";
            if (_config == null) return defaultValue;
            var result = _config.GetProperty(key, defaultValue);
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Loading key: {0} with value: {1}", key, result);
            Console.ForegroundColor = color;

            return result;
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="key">key值</param>
        /// <param name="defaultValue">如果没找到key,则返回默认值</param>
        /// <returns></returns>
        public static bool GetConfig(string pre, string key, bool defaultValue)
        {
            key = $"{pre}-{key}";
            if (_config == null) return defaultValue;
            var result = _config.GetProperty(key, defaultValue);
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Loading key: {0} with value: {1}", key, result);
            Console.ForegroundColor = color;

            return result.HasValue ? result.Value : defaultValue;
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="key">key值</param>
        /// <param name="defaultValue">如果没找到key,则返回默认值</param>
        /// <returns></returns>
        public static int GetConfig(string pre, string key, int defaultValue)
        {
            key = $"{pre}-{key}";
            if (_config == null) return defaultValue;
            var result = _config.GetProperty(key, defaultValue);
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Loading key: {0} with value: {1}", key, result);
            Console.ForegroundColor = color;

            return result.HasValue ? result.Value : defaultValue;
        }

        /// <summary>
        /// 动态监听配置改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="changeEvent"></param>
        private static void OnChanged(object sender, ConfigChangeEventArgs changeEvent)
        {
            if(OnChangeConfig!=null)
            {
                OnChangeConfig(null, changeEvent);
            }
            foreach (var kv in changeEvent.Changes)
            {
                Console.WriteLine("Change - key: {0}, oldValue: {1}, newValue: {2}, changeType: {3}",
                    kv.Value.PropertyName, kv.Value.OldValue, kv.Value.NewValue, kv.Value.ChangeType);
            }
        }
    }
}

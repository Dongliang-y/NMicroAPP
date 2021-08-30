/********************************************************************************

** auth： DongliangYi

** date： 2017/8/19 20:04:32

** desc： 尚未编写描述

** Ver.:  V1.0.0

*********************************************************************************/

namespace NPlatform.Config
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using NPlatform.Infrastructure;

    /// <summary>
    /// 配置文件工厂
    /// </summary>
    /// <typeparam name="T">配置实体类</typeparam>
    public class ConfigFactory<T>
        where T : class, IConfig, new()
    {
        private static string _currentPath = System.IO.Directory.GetCurrentDirectory();

        private static Dictionary<string, IConfig> configs = new Dictionary<string, IConfig>();
        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        /// <returns></returns>
        public static string GetFilePath()
        {
            try
            {
                var fileName = typeof(T).Name;

                // _currentPath = _currentPath.Replace("\\Debug", "").Replace("\\Release", "").Replace("\\bin", "");
                var dic = $"{_currentPath}\\Config";
                var fileFullPath = $"{dic}\\{fileName}.json";
                if (!Directory.Exists(dic))
                    Directory.CreateDirectory(dic);
                return fileFullPath;
            }
            catch (IOException ex)
            {
                throw new Exception($"IO错误，配置目录创建失败！--{ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new Exception($"无访问权限，无法创建配置目录！--{ex.Message}");
            }
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        public static void Save(T config)
        {
            var fileFullPath = GetFilePath();
            var json = SerializerHelper.ToJson(config);
            File.WriteAllText(fileFullPath, json);
        }

        /// <summary>
        /// 创建配置对象
        /// </summary>
        /// <returns></returns>
        public T Build()
        {
            var key = typeof(T).Name;
            if (configs.ContainsKey(key)) return configs[key] as T;

            var fileFullPath = GetFilePath();
            T config;
            if (File.Exists(fileFullPath))
            {
                config=SerializerHelper.FromJson<T>(File.ReadAllText(fileFullPath));
            }
            else
            {
                config = new T();
            }
            if(typeof(T) is NPlatformConfig)
            {
                var cfg = config as NPlatformConfig;
                if(cfg!=null)
                {
                    ApolloConfiguration.Init(cfg.ServiceID + cfg.MachineID+cfg.SystemName);
                }
            }
            return config;
        }
    }
}
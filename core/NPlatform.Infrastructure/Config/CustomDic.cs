using Com.Ctrip.Framework.Apollo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZJJWEPlatform.Config;

namespace ZJJWEPlatform.Config
{
    public class CustomDic:Dictionary<string,string>, ZJJWEPlatform.Config.IConfig
    {
        /// <summary>
        /// 实现[]操作
        /// </summary>
        /// <param name="key">对象的Id</param>
        /// <returns>对象</returns>
        public new string this[string key]
        {

            get
            {
                return ApolloConfiguration.GetConfig(nameof(ZJJWEPlatformConfig), key, base[key]);
            }
            set
            {
                base[key] = value;
            }
        }
    }
}

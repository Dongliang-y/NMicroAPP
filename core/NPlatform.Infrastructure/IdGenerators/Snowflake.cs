/*************************************************************************************
  * CLR版本：       4.0.30319.42000
  * 类 名 称：       Snowflake
  * 机器名称：       DESKTOP123
  * 命名空间：       ZJJWEPlatform.Infrastructure.IdGenerators
  * 文 件 名：       Snowflake
  * 创建时间：       2020-5-9 17:57:41
  * 作    者：          xxx
  * 说   明：。。。。。
  * 修改时间：
  * 修 改 人：
*************************************************************************************/
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Text;
using ZJJWEPlatform.Config;

namespace ZJJWEPlatform.IdGenerators
{
    /// <summary>
    /// 雪花算法
    /// </summary>
    public static class SnowflakeHelper 
    {
        private static ZJJWEPlatformConfig config = new ConfigFactory<ZJJWEPlatformConfig>().Build();
        private static readonly Snowflake.Core.IdWorker Snow = new Snowflake.Core.IdWorker(config.MachineID, config.ServiceID);
        public static long GenerateId()
        {
            return Snow.NextId();
        }

        public static bool IsEmpty(object id)
        {
            if (id == null)
            {
                return true;
            }
            return id.ToString().Trim().IsNullOrEmpty();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using ZJJWEPlatform.IdGenerators;

namespace ZJJWEPlatform.Infrastructure.IdGenerators
{
    /// <summary>
    /// ID 生成器
    /// </summary>
    public class IdGenerator: IIdGenerator
    {
        /// <summary>
        /// 生成ID
        /// </summary>
        /// <returns></returns>
        public object GenerateId()
        {
            return SnowflakeHelper.GenerateId();
        }

        public bool IsEmpty(object id)
        {
            return SnowflakeHelper.IsEmpty(id);
        }
    }
}

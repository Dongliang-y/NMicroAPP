using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZJJWEPlatform.Domains.Entity;

namespace ZJJWEPlatform.Domains.Service
{
    /// <summary>
    /// 缓存开放引用接口
    /// </summary>
    public interface ICacheKeyOpenService
    {
        /// <summary>
        /// 全量取得缓存Key
        /// </summary>
        /// <returns></returns>
        CacheKey[] FullGetCacheKey();


        /// <summary>
        /// 全量取得缓存关系 
        /// </summary>
        /// <returns></returns>
        CacheKeyRelations[] FullGetCacheKeyRelations();


    }
}

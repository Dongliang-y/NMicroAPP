using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NPlatform.Domains.Entity;
using NPlatform.Domains.IRepositories;
using NPlatform.IOC;

namespace NPlatform.Domains.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class CacheService : BaseService
    {
        /// <summary>
        ///  基于缓存的服务
        /// </summary>
        /// <param name="ctt"></param>
        public CacheService()
        { }
        /// <summary>
        /// 全量获取正常的缓存实例
        /// </summary>
        /// <returns>缓存实例集</returns>
        public CacheKey[] FullGetCacheKey()
        {
            var exp = CreateExpression<CacheKey>();
            exp = exp.AndAlso(m => m.IsDelete == false);
            IList<Sort> sortList = new List<Sort>();
            var cacheArray = IOCManager.BuildRepository<ICacheKeyRepository, CacheKey>().GetListByExp(exp, sortList);
            return AutoMapperHelper.Map<CacheKey[]>(cacheArray);
        }

        /// <summary>
        /// 全量获取正常的缓存实例的关系
        /// </summary>
        /// <returns>缓存关系 实例集</returns>
        public CacheKeyRelations[] FullGetCacheKeyRelations()
        {
            var exp = CreateExpression<CacheKeyRelations>();

            IList<Sort> sortList = new List<Sort>();
            var cacheRelationArray = IOCManager.BuildRepository<ICacheKeyRelationsRepository, CacheKeyRelations>().GetListByExp(exp, sortList);
            return AutoMapperHelper.Map<CacheKeyRelations[]>(cacheRelationArray);
        }
    }
}

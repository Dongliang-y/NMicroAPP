using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZJJWEPlatform.Domains.Entity;

namespace ZJJWEPlatform.Domains.IRepositories
{
    public partial interface ICacheKeyRelationsRepository : IAggregationRepository<CacheKeyRelations, string>
    {
    }
}

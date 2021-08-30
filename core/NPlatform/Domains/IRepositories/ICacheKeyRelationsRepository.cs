using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NPlatform.Domains.Entity;

namespace NPlatform.Domains.IRepositories
{
    public partial interface ICacheKeyRelationsRepository : IAggregationRepository<CacheKeyRelations, string>
    {
    }
}

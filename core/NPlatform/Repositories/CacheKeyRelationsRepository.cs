using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZJJWEPlatform.Domains.Entity;
using ZJJWEPlatform.Domains.IRepositories;

namespace ZJJWEPlatform.Repositories
{
    public partial class CacheKeyRelationsRepository : AggregationRepository<CacheKeyRelations, string>, ICacheKeyRelationsRepository

    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheKeyRelationsRepository"/> class.
        /// </summary>
        /// <param name="options"></param>
        public CacheKeyRelationsRepository(IRepositoryOptions options)
            : base(options)
        {

        }

    }
}

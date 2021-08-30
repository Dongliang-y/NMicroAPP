using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NPlatform.Domains.Entity;
using NPlatform.Domains.IRepositories;

namespace NPlatform.Repositories
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

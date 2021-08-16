﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZJJWEPlatform.Domains.Entity;
using ZJJWEPlatform.Domains.IRepositories;

namespace ZJJWEPlatform.Repositories
{
    public partial class CacheKeyRepository : AggregationRepository<CacheKey, string>, ICacheKeyRepository

    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheKeyRepository"/> class.
        /// </summary>
        /// <param name="options"></param>
        public CacheKeyRepository(IRepositoryOptions options)
            : base(options)
        {

        }

        #region 追加 标签S
        #endregion 追加 标签E


    }
}

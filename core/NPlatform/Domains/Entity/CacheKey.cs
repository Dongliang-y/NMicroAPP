// <copyright file="CacheKeyService" company="ZJJW">
// Copyright (c) 湖南中交京纬信息技术有限公司. All rights reserved.
// </copyright>
// <author>侯向阳</author>
// <date>2020-09-29 16:30</date>
// <summary>文件功能描述</summary>
// <modify>
//      修改人：侯向阳
//      修改时间：2020-09-29 16:30
//      修改描述：
//      版本：1.0
// </modify>


namespace NPlatform.Domains.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NPlatform;
    using NPlatform.Domains;
    using NPlatform.Domains.Entity;
    using NPlatform.Filters;
    using NPlatform.IdGenerators;

    // Base_CacheKey
    // 缓存键值管理
    /// <summary>
    /// 
    /// </summary>
    [TableName(TabName = "SYS_CacheKey")]
    public partial class CacheKey : AggregationBase<string>     
    {

        /// <summary>
        /// Gets or sets 缓存标识，用于快速索引
        /// </summary>
        //public string Key { get; set; }

        /// <summary>
        /// Gets or sets 方法全量名称
        /// </summary>
        public string MethodFullName { get; set; }

        /// <summary>
        /// Gets or sets 缓存间隔时间
        /// </summary>
        public int? Interval { get; set; }

        /// <summary>
        /// 返回实体类型
        /// </summary>
        public string TransformationMethod { set; get; }

        /// <summary>
        /// 返回实体类型
        /// </summary>
        public string ReturnType { set; get; }

        /// <summary>
        /// Gets or sets 缓存的key
        /// 格式：(短方法名/标识){:参数1}{:参数2}
        /// 
        /// 示例：CustomMethod:{pars1,pars2.value}
        /// </summary>
        //public string CacheCode { get; set; }

        /// <summary>
        /// Gets or sets 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
 

        /// <inheritdoc/>
        public void GenerateId()
        {
            this.Id = SnowflakeHelper.GenerateId().ToString();
        }

        /// <summary>
        /// Gets or sets
        /// </summary>
        [ORMIgnored]
        public List<CacheKeyRelations> CacheKeyRelationsList{set; get;}

        /// <inheritdoc/>
        public void AddCacheKeyRelations(CacheKeyRelations cacheKeyRelations)
        {
            CacheKeyRelationsList.Add(cacheKeyRelations);
        }
    }
}
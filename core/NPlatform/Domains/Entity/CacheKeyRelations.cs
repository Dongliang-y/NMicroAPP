// <copyright file="CacheKeyRelationsService" company="ZJJW">
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

    // Base_CacheKeyRelations
    // 缓存键值关系管理
    /// <summary>
    /// 
    /// </summary>
    [TableName(TabName = "Sys_CacheKeyRelations")]
    public partial class CacheKeyRelations : AggregationBase<string>
    {

        /// <summary>
        /// Gets or sets 缓存标识,CacheKey中的KEY外键,
        /// 用于受影响的方法KEY值，
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 关系KEY，用于控制的方法的KEY值
        /// </summary>
        public string RelationKey { get; set; }

        /// <summary>
        /// Gets or sets 关系KEY要附加的参数
        /// </summary>
        //public string Value { get; set; }

        /// <summary>
        /// Gets or sets 关系信息，多个关系使用,分隔
        /// 现在只需要用到1了
        /// 0：依赖
        /// 1：影响
        /// </summary>
        public string Relations { get; set; }


        /// <inheritdoc/>
        public void GenerateId()
        {
            this.Id = SnowflakeHelper.GenerateId().ToString();
        }
    }
}
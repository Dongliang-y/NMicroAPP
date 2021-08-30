#region << 版 本 注 释 >>

/*----------------------------------------------------------------
* 项目名称 ：NPlatform.Domains.IRepositories
* 类 名 称 ：IRepositoryOptions
* 类 描 述 ：
* 命名空间 ：NPlatform.Domains.IRepositories
* CLR 版本 ：4.0.30319.42000
* 作    者 ：DongliangYi
* 创建时间 ：2018-11-20 17:22:26
* 更新时间 ：2018-11-20 17:22:26
* 版 本 号 ：v1.0.0.0
//----------------------------------------------------------------*/

#endregion

using System.Collections.Generic;
using System.Data;
using NPlatform.Filters;
using NPlatform.Repositories;

namespace NPlatform.Domains.IRepositories
{
    /// <summary>
    /// 仓储上下文配置接口
    /// </summary>
    public interface IContextOptions
    {
        /// <summary>
        /// 链接字符串配置名称，如果此属性为  string.Empty或者null 则使用ConnectSring。
        /// </summary>
        string ConnectName { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        string ConnectSring { get; set; }

        /// <summary>
        /// 数据库驱动程序
        /// </summary>
        DBProvider DBProvider { get; set; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        IDbConnection Connection { get; set; }

        /// <summary>
        /// 能否支持事务处理
        /// </summary>
        bool IsTransactional { get; set; }

        /// <summary>
        /// 事务超时时间,单位秒
        /// </summary>
        int? TimeOut { get; set; }

        /// <summary>
        /// 所有过滤器，包括查询的。
        /// </summary>
        IDictionary<string, IQueryFilter> AllQueryFilters { get; set; }
        /// <summary>
        /// 查询表达式过滤器
        /// </summary>
        IDictionary<string, IQueryFilter> QueryFilters { get; set; }
        /// <summary>
        /// 数据过滤器
        /// </summary>
        IDictionary<string, IResultFilter> ResultFilters { get; set; }
    }
}
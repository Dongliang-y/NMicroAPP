#region << 版 本 注 释 >>

/*----------------------------------------------------------------
* 项目名称 ：NPlatform.Applications
* 项目描述 ：
* 类 名 称 ：Application
* 类 描 述 ：
* 所在的域 ：LDY
* 命名空间 ：NPlatform.Applications
* 机器名称 ：LDY 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：DongliangYi
* 创建时间 ：2018-12-21 10:32:12
* 更新时间 ：2018-12-21 10:32:12
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ DongliangYi 2018. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/

#endregion

namespace NPlatform.Applications
{
    using System;
    using System.Linq.Expressions;
    using System.Collections.Generic;
    using System.Linq;

    using NPlatform.Config;
    using NPlatform.Result;
    using NPlatform.Repositories;
    using NPlatform.Domains.Service;

    /// <summary>
    /// Application 基类
    /// </summary>
    public class ApplicationService : ResultBase, IApplication
    {
        /// <summary>
        /// The _ config.
        /// </summary>
        protected static NPlatformConfig Config { get; } = new ConfigFactory<NPlatformConfig>().Build();

        /// <summary>
        /// 集合分页
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sources">数据源</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="total">总数</param>
        /// <returns>分页结果</returns>
        public IListResult<T> SearchArrayPage<T>(IEnumerable<T> sources, int page, int pageSize, out long total)
        {
            total = sources.Count();
            if (page > 0)
            {
                // 分页
                page--;
                var result = sources.Skip(page * pageSize).Take(pageSize).ToList();
                return ListData<T>(result,total);
            }
            else
            {
                return Error<T>("页码不能小于等于0");
            }
        }
        /// <summary>
        /// 创建某个实体对象的表达式
        /// </summary>
        /// <typeparam name="T">实体类型
        /// </typeparam>
        /// <returns>
        /// The <see cref="Expression"/>.
        /// </returns>
        protected Expression<Func<T, bool>> CreateExpression<T>(Expression<Func<T, bool>> exp=null)
        {
            Expression<Func<T, bool>> expression;
            if (exp==null)
                return expression = x => 1 == 1;
            return exp;
        }
    }
}
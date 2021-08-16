/***********************************************************
**项目名称:	                                                                  				   
**功能描述:	  的摘要说明
**作    者: 	易栋梁                                         			   
**版 本 号:	1.0                                             			   
**创建日期： 2015/12/10 9:45:22
**修改历史：
************************************************************/

namespace ZJJWEPlatform.Domains.IRepositories
{
    using Dapper;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using ZJJWEPlatform.Domains.Entity;

    /// <summary>
    /// 工作单元模式仓储接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 事物是否已提交
        /// </summary>
        bool IsCommitted { get; }

        /// <summary>
        /// 获取/设置工作单元的超时时间
        /// </summary>
        int? Timeout { get; set; }

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T">需要新增的类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>返回T</returns>
        T Add<T>(T entity)
            where T : class, IEntity;

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <typeparam name="T">需要新增的类型</typeparam>
        /// <param name="entitys">需要新增的</param>
        void Adds<T>(IEnumerable<T> entitys)
            where T : class, IEntity;

        /// <summary>
        /// 执行sql脚本
        /// </summary>
        /// <typeparam name="sql">需要执行的SQL</typeparam>
        /// <param name="parameters">参数对象</param>
        /// <returns>执行结果</returns>
        int Execute(string sql, DynamicParameters parameters);

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T">需要修改的类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>返回T</returns>
        bool Change<T>(T entity)
            where T : class, IEntity;

        /// <summary>
        /// 提交
        /// </summary>
        void Commit();

        /// <summary>
        /// 移除实体对象
        /// </summary>
        /// <typeparam name="T">需要新增的类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>返回T</returns>
        bool Remove<T>(T entity)
            where T : class, IEntity;

        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="filter"></param>
        bool Remove<T>(Expression<Func<T, bool>> filter)
            where T : class, IEntity;

        /// <summary>
        /// 回滚事物
        /// </summary>
        void Rollback();
    }
}
/***********************************************************
**项目名称:	                                                                  				   
**功能描述:	  仓储根接口 
**作    者: 	易栋梁  
**版 本 号:	1.0                                             			   
**创建日期： 2015-12-10 10:07
**修改历史：
************************************************************/

namespace ZJJWEPlatform.Domains.IRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using DapperExtensions;

    using ZJJWEPlatform.Domains.Entity;
    using ZJJWEPlatform.Result;

    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="TEntity"> 实体对象
    /// </typeparam>
    /// <typeparam name="TPrimaryKey"> 主键
    /// </typeparam>
    public interface IRepositories<TEntity, in TPrimaryKey> 
        where TEntity : IEntity
    {
        /// <summary>
        /// this重载
        /// </summary>
        TEntity this[TPrimaryKey key] { get; set; }

        /// <summary>
        /// 新增对象
        /// </summary>
        TEntity Add(TEntity item);

        /// <summary>
        /// 新增或更新对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity AddOrUpdate(TEntity entity);

        /// <summary>
        /// 新增对象
        /// </summary>
        int Adds(IEnumerable<TEntity> items);

        /// <summary>
        /// 新增对象
        /// </summary>
        Task<int> AddsAsync(IEnumerable<TEntity> items);

        /// <summary>
        /// 统计记录数
        /// </summary>
        /// <param name="filter">linq表达式</param>
        int Count(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// 数据是否存在
        /// </summary>
        /// <param name="key">主键</param>
        bool Exists(TPrimaryKey key);

        /// <summary>
        /// 查询对象是否存在
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// 查找数据
        /// </summary>
        /// <param name="key">主键</param>
        TEntity FindBy(TPrimaryKey key);

        /// <summary>
        /// 筛选数据
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <param name="sorts">排序字段</param>
        /// <returns></returns>
        IEnumerable<TEntity> GetListByExp(Expression<Func<TEntity, bool>> filter, IList<Sort> sorts = null);

        /// <summary>
        /// 分页查询对象集合,起始页码0
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="filter">数据筛选</param>
        /// <param name="sorts">排序字段</param>
        IListResult<TEntity> GetPaged(
            int pageIndex,
            int pageSize,
            Expression<Func<TEntity, bool>> filter,
            IList<Sort> sorts);

        /// <summary>
        /// 删除对象
        /// </summary>
        bool Remove(TEntity item);

        /// <summary>
        /// 删除对象
        /// </summary>
        bool Remove(params TPrimaryKey[] keys);

        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="filter"></param>
        bool Remove(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// 删除对象
        /// </summary>
        Task<bool> RemoveAsync(params TPrimaryKey[] keys);
    }
}
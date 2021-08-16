#region << 版 本 注 释 >>

/*----------------------------------------------------------------
* 项目名称 ：ZJJWEPlatform.Domains.IUnitOfWork
* 类 名 称 ：IAggregationUnitOfWork
* 类 描 述 ：
* 命名空间 ：ZJJWEPlatform.Domains.IUnitOfWork
* CLR 版本 ：4.0.30319.42000
* 作    者 ：DongliangYi
* 创建时间 ：2018-11-19 17:39:43
* 更新时间 ：2018-11-19 17:39:43
* 版 本 号 ：v1.0.0.0
//----------------------------------------------------------------*/

#endregion

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
    /// 聚合内的工作单元接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型
    /// </typeparam>
    /// <typeparam name="TPrimaryKey">主键类型
    /// </typeparam>
    public interface IAggregationRepository<TEntity, TPrimaryKey>
        where TEntity : AggregationBase<TPrimaryKey>
    {
        /// <summary>
        /// this 重载
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns>返回实体</returns>
        TEntity this[TPrimaryKey key] { get; set; }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item">新增对象</param>
        /// <returns>返回新增后的实体</returns>
        TEntity Add(TEntity item);

        /// <summary>
        /// 新增或更新对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>修改后的实体</returns>
        TEntity AddOrUpdate(TEntity entity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="items">批量新增对象</param>
        int Adds(IEnumerable<TEntity> items);

        /// <summary>
        /// 异步新增
        /// </summary>
        /// <param name="items">实体对象</param>
        Task<int> AddsAsync(IEnumerable<TEntity> items);

        /// <summary>
        /// 统计记录数
        /// </summary>
        /// <param name="filter">linq表达式</param>
        int Count(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// 求最大值
        /// </summary>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="attrName">属性名</param>
        /// <param name="filter">过滤条件</param>
        /// <returns>最大值</returns>
        TValue Max<TValue>(string attrName, Expression<Func<TEntity, bool>> filter = null);


        /// <summary>
        /// 求最小值
        /// </summary>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="attrName">属性名</param>
        /// <param name="filter">过滤条件</param>
        /// <returns>最小值</returns>
        TValue Min<TValue>(string attrName, Expression<Func<TEntity, bool>> filter = null);


        /// <summary>
        /// 求和
        /// </summary>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="attrName">属性名</param>
        /// <param name="filter">过滤条件</param>
        /// <returns>和</returns>
        TValue Sum<TValue>(string attrName, Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// 求平均值
        /// </summary>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="attrName">属性名</param>
        /// <param name="filter">过滤条件</param>
        /// <returns>平均值</returns>
        TValue AVG<TValue>(string attrName, Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// 数据是否存在
        /// </summary>
        /// <param name="key">主键</param>
        bool Exists(TPrimaryKey key);

        /// <summary>
        /// 查询对象是否存在
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>结果</returns>
        bool Exists(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// 查找数据
        /// </summary>
        /// <param name="key">主键</param>
        TEntity FindBy(TPrimaryKey key);

        /// <summary>
        /// 查找数据
        /// </summary>
        /// <param name="key">主键</param>
        Task<TEntity> FindByAsync(TPrimaryKey key);

        /// <summary>
        /// 异步查询所有数据
        /// </summary>
        /// <param name="sorts">排序字段</param>
        /// <returns>结果</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(IList<Sort> sorts = null);

        /// <summary>
        /// 按条件查找第一个
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns>结果对象</returns>
        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// 按条件查找第一个
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns>结果对象</returns>
        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// 筛选数据
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <param name="sorts">排序字段</param>
        /// <returns>查询结果</returns>
        IEnumerable<TEntity> GetListByExp(Expression<Func<TEntity, bool>> filter, IList<Sort> sorts = null);

        /// <summary>
        /// 指定字段范围查询，返回的实体只有这几个字段有值，目的是为了避免字段多时全字段查询（select *）
        /// </summary>
        /// <param name="columnNames">需要指定查询的字段</param>
        /// <param name="filter">筛选条件</param>
        /// <param name="sorts">排序字段</param>
        /// <returns>实体集合</returns>
        IEnumerable<TEntity> GetListWithColumns(IEnumerable<string> columnNames,
            Expression<Func<TEntity, bool>> filter,
            IList<Sort> sorts = null);

        /// <summary>
        /// 根据表达式异步获取
        /// </summary>
        /// <param name="filter">表达式</param>
        /// <param name="sorts">排序</param>
        /// <returns>查询结果</returns>
        Task<IEnumerable<TEntity>> GetListByExpAsync(Expression<Func<TEntity, bool>> filter, IList<Sort> sorts = null);

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
        /// 异步分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="filter">条件</param>
        /// <param name="sorts">排序</param>
        /// <returns>异步结果</returns>
        Task<IListResult<TEntity>> GetPagedAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<TEntity, bool>> filter,
            IList<Sort> sorts);

        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns>结果</returns>
        bool Remove(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// 键值删除
        /// </summary>
        /// <param name="keys">
        /// The keys.
        /// </param>
        bool Remove(params TPrimaryKey[] keys);

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool Remove(TEntity entity);

        /// <summary>
        /// 异步移除
        /// </summary>
        /// <param name="keys">键值集合</param>
        /// <returns>结果</returns>
        Task<bool> RemoveAsync(params TPrimaryKey[] keys);
    }
}
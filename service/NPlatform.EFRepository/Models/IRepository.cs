/***********************************************************
**项目名称:
**功能描述: 仓储  的摘要说明
**作    者:   易栋梁
**版 本 号:    1.0
**创建日期： 2015/12/7 16:06:56
**修改历史：
************************************************************/

using PocManage.Models.TestResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PocManage.Repositories
{
    public interface IRepository<TEntity, TPrimaryKey> where TEntity : BaseModel
    {
        Task<TEntity> AddAsync(TEntity item);
        Task<int> AddOrUpdate(TEntity item);
        Task<int> AddsAsync(IEnumerable<TEntity> items);
        Task<bool> Exists(TPrimaryKey key);
        Task<TEntity> FindByAsync(long key);
        Task<bool> Remove(long key);
        Task<bool> Removes(params TEntity[] entitys);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate=null);

        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate, Func<TEntity, long> orderBy);
        (IEnumerable<TEntity>,long total) GetPage(Expression<Func<TEntity, bool>> predicate, Func<TEntity, long> orderBy,int skip,int take);
    }
}
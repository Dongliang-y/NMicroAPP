/***********************************************************
**项目名称:
**功能描述: 仓储  的摘要说明
**作    者:   易栋梁
**版 本 号:    1.0
**创建日期： 2015/12/7 16:06:56
**修改历史：
************************************************************/

namespace PocManage.Repositories
{
    using PocManage.Models.TestResource;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// 聚合仓储基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="string">主键类型</typeparam>
    public class RepositoryBase<TEntity, TPrimaryKey>  where TEntity : BaseModel,new()
    {
        protected EFContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{TEntity,TPrimaryKey}"/> class. 
        /// 仓储基类
        /// </summary>
        /// <param name="option">
        /// 仓储配置
        /// </param>
        public RepositoryBase(EFContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item">新增对象</param>
        /// <returns>新增后创建了Id 的对象。</returns>
        public async virtual Task<TEntity> AddAsync(TEntity item)
        {
            await this._dbContext.AddAsync<TEntity>(item);
            await this._dbContext.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// 异步新增
        /// </summary>
        /// <param name="items">新增对象的集合</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
        public async Task<int> AddsAsync(IEnumerable<TEntity> items)
        {
            await this._dbContext.AddRangeAsync(items);
            return await this._dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// 异步新增
        /// </summary>
        /// <param name="items">新增对象的集合</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
        public async Task<int> AddOrUpdate(TEntity item)
        {
            var entity=await this._dbContext.FindAsync<TEntity>(item.id);
            entity.CopyOrCreate<TEntity>(item);
            return await this._dbContext.SaveChangesAsync();
        }


        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            return this._dbContext.Set<TEntity>().Where(predicate);
        }


        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return this._dbContext.Set<TEntity>().FirstOrDefault(predicate);
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate, Func<TEntity, long> orderBy)
        {
            return this._dbContext.Set<TEntity>().Where(predicate).OrderBy(orderBy).AsEnumerable();
        }


        public (IEnumerable<TEntity>, long total) GetPage(Expression<Func<TEntity, bool>> predicate, Func<TEntity, long> orderBy, int skip, int take)
        {
            return (this._dbContext.Set<TEntity>().Where(predicate).OrderBy(orderBy)
                .Skip(skip).Take(take), this._dbContext.Set<TEntity>().Count());
        }
        /// <summary>
        /// 判断对象是否已存在
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>是否存在</returns>
        public virtual async Task<bool> Exists(TPrimaryKey key)
        {
            if (EqualityComparer<TPrimaryKey>.Default.Equals(key, default(TPrimaryKey)))
            {
                return false;
            }
            var rst = await this._dbContext.FindAsync<TEntity>(key);
            return rst != null;
        }

        /// <summary>
        /// 从仓储查找对象
        /// </summary>
        /// <param name="key">主键字段</param>
        /// <returns>对象</returns>
        public async virtual Task<TEntity> FindByAsync(long key)
        {
            var rst = await this._dbContext.FindAsync<TEntity>(key);
            return rst;
        }

        /// <summary>
        /// 键值删除
        /// </summary>
        public virtual async Task<bool> Remove(long key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            var result = await this._dbContext.FindAsync<TEntity>(key);
            var rst = this._dbContext.Remove<TEntity>(result);
            var deleteResult=rst.State == Microsoft.EntityFrameworkCore.EntityState.Deleted;
            this._dbContext.SaveChanges();
            return deleteResult;
        }

        /// <summary>
        /// 键值删除
        /// </summary>
        public virtual async Task<bool> Removes(params TEntity[] entitys)
        {
            if (entitys == null)
            {
                throw new ArgumentNullException(nameof(entitys));
            }
            this._dbContext.RemoveRange(entitys);
            this._dbContext.SaveChanges();
            return true;
        }

    }

    public static class objExt
    {


        /// <summary>
        /// 创建对象，只处理基本数据类型的字段，
        /// </summary>
        /// <typeparam name="T">要创建的类型</typeparam>
        /// <param name="values">字典</param>
        /// <param name="fun">委托，复杂类型交给调用方自己处理</param>
        /// <returns></returns>
        public static TEntity CopyOrCreate<TEntity>(this TEntity entity, TEntity src, Func<TEntity, TEntity> fun = null) where TEntity:new()
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }

            var attr = typeof(TEntity).GetProperties();

            if (entity == null)
                entity = new TEntity();
            foreach (var prop in attr)
            {
                var val = prop.GetValue(src);
                if (val == null)
                {
                    continue;
                }

                if (prop.PropertyType == typeof(string))
                {
                    prop.SetValue(entity, val.ToString());
                }
                else if (prop.PropertyType == typeof(long) || prop.PropertyType == typeof(long?))
                {
                    prop.SetValue(entity, Convert.ToInt64(val));
                }
                else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
                {
                    prop.SetValue(entity, Convert.ToInt32(val));
                }
                else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
                {
                    prop.SetValue(entity, Convert.ToBoolean(val));
                }
                else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                {
                    prop.SetValue(entity, Convert.ToDecimal(val));
                }
                else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                {
                    prop.SetValue(entity, Convert.ToDateTime(val));
                }
                else if (prop.PropertyType == typeof(float) || prop.PropertyType == typeof(float?))
                {
                    prop.SetValue(entity, Convert.ToDouble(val));
                }
                else if (prop.PropertyType == typeof(byte) || prop.PropertyType == typeof(byte?))
                {
                    prop.SetValue(entity, Convert.ToByte(val));
                }
                else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
                {
                    prop.SetValue(entity, Convert.ToDouble(val));
                }
            }
            entity = fun != null ? fun(entity) : entity;
            return entity;
        }
    }
}
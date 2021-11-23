/***********************************************************
**项目名称:	                                                                  				   
**功能描述:	 

工作单元服务于仓储，并在工作单元中初始化上下文，为仓储单元提供上下文对象，由此确保同一上下文对象。

************************************************************/

namespace NPlatform.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.Logging;
    using NPlatform.Domains.Entity;
    using NPlatform.Domains.IRepositories;
    using NPlatform.Filters;
    using NPlatform.Repositories.IRepositories;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// IUnitOfWork 的实现，实现事务功能。
    /// </summary>
    public class UnitOfWork : IDisposable, IUnitOfWork
    {

        protected EFContext _dbContext;
        public IRepositoryOptions Options { get; set; }

        private IDbContextTransaction trans;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{TEntity,TPrimaryKey}"/> class. 
        /// IUnitOfWork
        /// </summary>
        /// <param name="option">
        /// EFContext
        /// </param>
        public UnitOfWork(IRepositoryOptions options)
        {
            Options = options;
            _dbContext = new EFContext(options.MainConection, options.DBProvider, (int)options.TimeOut);
            trans = _dbContext.Database.BeginTransaction();
            BeginTrans = true;
        }

        /// <summary>
        /// 是否开启了事务
        /// </summary>
        public bool BeginTrans { get; }

        /// <summary>
        ///     获取 当前单元操作是否已被提交
        /// </summary>
        public bool IsCommitted { get; private set; }

        /// <summary>
        /// 连接超时设定
        /// </summary>
        public int? Timeout { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// 新增对象
        /// </summary>
        public virtual async Task<T> AddAsync<T>(T entity)
            where T :class, IEntity
        {
            SetFilter<T>(entity);
            await this._dbContext.AddAsync(entity);
            IsCommitted = false;
            await this._dbContext.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// 新增对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entitys">实体集合</param>
        public virtual async Task<int> AddsAsync<T>(IEnumerable<T> entitys)
            where T : class, IEntity
        {
            foreach (var entity in entitys)
            {
                SetFilter<T>(entity);
            }
            IsCommitted = false;
            await this._dbContext.AddRangeAsync(entitys);
            return await this._dbContext.SaveChangesAsync();
            
        }

        /// <summary>
        /// 更改对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">entity</param>
        /// <returns>修改结果</returns>
        public virtual async Task<int> ChangeAsync<T>(T entity)
            where T : class, IEntity
        {
            this._dbContext.Update(entity);
            var rst = await this._dbContext.SaveChangesAsync();
            IsCommitted = false;
            return rst;
        }
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>删除结果</returns>
        public virtual async Task<int> RemoveAsync<T>(params T[] entities)
            where T : class, IEntity
        {
            var enabled = Options.QueryFilters.ContainsKey(nameof(LogicDeleteFilter));
            foreach (var entity in entities)
            {
                if (typeof(ILogicDelete).IsAssignableFrom(typeof(T)) && enabled)
                {
                    ((ILogicDelete)entity).IsDeleted = true;
                   this._dbContext.Update(entity);
                }
                else
                {
                    this._dbContext.Remove<T>(entity);
                }
            }
            var rst = await this._dbContext.SaveChangesAsync();
            IsCommitted = false;
            return rst;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="filter">筛选表达式</param>
        /// <returns>返回类型</returns>
        public virtual async Task<int> RemoveAsync<T>(Expression<Func<T, bool>> filter)
            where T : class, IEntity
        {
            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            var rst =await this._dbContext.Set<T>().Where(filter).ToArrayAsync();
            return await this.RemoveAsync<T>(rst);
        }

        public async Task<IEnumerable<T>> QueryFromSql<T>(string sql, object parameters) where T : class, IEntity
        {
            if (sql is null)
            {
                throw new ArgumentNullException(nameof(sql));
            }
            var rst = this._dbContext.Set<T>().FromSqlRaw(sql, parameters);
            return await rst.ToListAsync();
        }

        /// <summary>
        /// 设置实体的过滤器属性
        /// </summary>
        /// <param name="items">实体</param>
        private void SetFilter<T>(params T[] items) where T : class, IEntity
        {
            // 实体如果实现了过滤器， 那么仓储就可以拿注入进来的过滤器对实体进行设置与过滤。
            if (typeof(IFilter).IsAssignableFrom(typeof(T)))
            {
                for (var i = 0; i < items.Length; i++)
                {
                    foreach (var filter in this.Options.QueryFilters)
                    {
                        filter.Value.SetFilterProperty<T>(items[i]); // 设置过滤器
                    }
                }
            }
        }

        /// <summary>
        /// 提交所有工作
        /// </summary>
        public virtual void Commit()
        {
            if (IsCommitted)
            {
                return;
            }

            try
            {
                this.trans?.Commit();
                IsCommitted = true;
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    Console.WriteLine(e.InnerException.ToString());
                    throw e.InnerException;
                }

                throw;
            }
        }

        /// <summary>
        /// 对象销毁是提交任务
        /// </summary>
        public void Dispose()
        {
            if (this._dbContext != null)
            {
                System.Diagnostics.Debug.WriteLine($"{(_dbContext as object).GetHashCode()} | TransConn 销毁");
            }

            if (BeginTrans && !IsCommitted)
            {
                Commit();
            }

            try
            {
                this.trans?.Dispose();
            }
            catch
            {
                // ignored
            }
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }

            this._dbContext = null;


            try
            {
                GC.SuppressFinalize(this);
            }
            catch
            {
                // i
            }
        }

        /// <summary>
        /// 回滚事物
        /// </summary>
        public virtual void Rollback()
        {
            if (IsCommitted)
            {
                return;
            }

            try
            {
                this.trans?.Rollback();
                IsCommitted = true;
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    Console.WriteLine(e.InnerException.ToString());
                    throw e.InnerException;
                }

                throw;
            }
        }
    }
}
/***********************************************************
**项目名称:	                                                                  				   
**功能描述:	 

简说了，主要作用是在数据持久化过程中，数据提交，确保数据的完整性，对象使用确保同一上下文对象。如果有异常，提供回滚。

三，二者的关系
即：

工作单元服务于仓储，并在工作单元中初始化上下文，为仓储单元提供上下文对象，由此确保同一上下文对象。

那么此时，问题来了，怎么在仓储中获取上下文。（使用的orm为 EF，以autofac或者MEF实现注入，以此为例） 所以，此时实现就变得很简单。
**作    者: 	易栋梁                                         			   
**版 本 号:	1.0                                             			   
**创建日期： 2015/12/7 16:18:07
**修改历史：
************************************************************/

namespace NPlatform.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Dapper;
    using DapperExtensions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using NPlatform.Domains.Entity;
    using NPlatform.Domains.IRepositories;
    using NPlatform.Filters;
    /// <summary>
    /// IUnitOfWork 的实现，此UnitOfWork 可以跨业务领域。
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The config.
        /// </summary>
        public IConfiguration Config { get; set; }

        /// <summary>
        /// The df.
        /// </summary>
        private static DbProviderFactory df = null;

        /// <summary>
        /// 连接上下文
        /// </summary>
        private IDbConnection DBContext;


        /// <summary>
        /// The trans.
        /// </summary>
        private IDbTransaction trans;

        public ILogger<UnitOfWork> Logger;
        /// <summary>
        /// 事务的配置项
        /// </summary>
        public IRepositoryOptions Options { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class. 
        /// 创建连接  
        /// </summary>
        /// <param name="option">
        /// The option.
        /// </param>
        public UnitOfWork(IRepositoryOptions option)
        {
            Options = option;
            DapperExtensions.DefaultMapper = typeof(DapperExt.CustomClassMapper<>);

            DbProviderFactories.RegisterFactory(
                NPlatform.Repositories.DBProvider.MySqlClient.GetEnumDes(),
                new MySql.Data.MySqlClient.MySqlClientFactory());
            DapperExtensions.SqlDialect = new global::DapperExtensions.Sql.MySqlDialect();

            Timeout = option.TimeOut;
            this.BeginTrans = true;

            // 惰性加载，第一次使用时加载
            this.DBContext = df.CreateConnection();
            if (this.DBContext != null)
            {
                this.DBContext.ConnectionString = Options.MainConection;
                this.DBContext.Open();
                if (this.BeginTrans)
                {
                    this.trans = this.DBContext.BeginTransaction();
                }

                System.Diagnostics.Debug.WriteLine($"{((object)this.DBContext).GetHashCode()} | QueryContext 创建");
            }
            else
            {
                throw new System.Data.DataException("数据库链接创建失败！");
            }
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
        public  async Task<T> AddAsync<T>(T entity)
                        where T : class, IEntity
        {
            SetFilter<T>(entity);
            await DBContext.InsertAsync(entity, this.trans, Timeout);
            IsCommitted = false;
            return entity;
        }

        public async Task<int> AddsAsync<T>(IEnumerable<T> entitys)
                        where T : class, IEntity
        {
            foreach (var entity in entitys)
            {
                SetFilter<T>(entity);
            }
            await DBContext.InsertsAsync(entitys, this.trans, Timeout);
            IsCommitted = false;
            return entitys.Count();
        }

        public async Task<int> ChangeAsync<T>(T entity)
                        where T : class, IEntity
        {
            var result = await DBContext.UpdateAsync<T>(entity, this.trans, Timeout);
            IsCommitted = false;
            return result ? 1 : 0;
        }

       public async  Task<int> RemoveAsync<T>(params T[] entities) where T : class, IEntity
        {
            var result = false;
            var enabled = Options.QueryFilters.ContainsKey(nameof(LogicDeleteFilter));
            foreach (var entity in entities)
            {
                if (typeof(ILogicDelete).IsAssignableFrom(typeof(T)) && enabled)
                {
                    ((ILogicDelete)entity).IsDeleted = true;
                    result = await this.DBContext.UpdateAsync<T>(entity, this.trans, this.Timeout);
                }
                else
                {
                    result =await this.DBContext.DeleteAsync<T>(entity, this.trans, this.Timeout) > 0;
                }
            }

            IsCommitted = false;
            return entities.Count();
        }


        public async Task<int> RemoveAsync<T>(Expression<Func<T, bool>> filter) where T : class, IEntity
        {
            var result = 0;
            if (filter == null)
            {
                throw new NPlatformException($"filter参数不能为空！", "UnitOfWorkRemove");
            }

            // 应用过滤器
            foreach (var ft in this.Options.QueryFilters)
            {
                var exp = ft.Value.GetFilter<T>();
                if (exp != null)
                {
                    filter = filter.AndAlso(ft.Value.GetFilter<T>());
                }
            }
            var enabled = Options.QueryFilters.ContainsKey(nameof(LogicDeleteFilter));

            if (typeof(ILogicDelete).IsAssignableFrom(typeof(T)) && enabled)
            {
                var predicate = QueryBuilder<T>.FromExpression(filter);
                var entitys = DBContext.GetList<T>(predicate).ToArray();
                foreach (var entity in entitys)
                {
                    ((ILogicDelete)entity).IsDeleted = true;
                    result+= await this.DBContext.UpdateAsync<T>(entity, this.trans, this.Timeout)?1:0;
                }
            }
            else
            {
                var predicate = QueryBuilder<T>.FromExpression(filter);
                result = await DBContext.DeleteAsync<T>(predicate, this.trans, Timeout);

            }

            IsCommitted = false;

            return result;
        }

        /// <summary>
        /// 执行sql脚本
        /// </summary>
        /// <typeparam name="sql">需要执行的SQL</typeparam>
        /// <param name="parameters">参数对象</param>
        /// <returns>执行结果</returns>
        public async virtual Task<IEnumerable<T>> QueryFromSql<T>(string sql, object parameters)
        {
            if (sql.IsNullOrEmpty()) return null;
            var result = await this.DBContext.QueryAsync<T>(sql, parameters, this.trans, this.Timeout);
            IsCommitted = false;
            return result;
        }

        Task<IEnumerable<T>> IUnitOfWork.QueryFromSql<T>(string sql, object parameters)
        {
            throw new NotImplementedException();
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
                    Logger.LogError("UnitOfWork Commit异常！{0}", e);
                    throw e.InnerException;
                }

                throw;
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
                    Logger.LogError("Rollback Error！{ 0}", e.ToString());
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
            try
            {
                if (DBContext != null)
                {
                    System.Diagnostics.Debug.WriteLine($"{(DBContext as object).GetHashCode()} | TransConn 销毁");
                }
            }
            catch
            {
                // 不处理
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

            try
            {
                if (DBContext != null)
                {
                    DBContext.Close();
                    DBContext.Dispose();
                }

                this.DBContext = null;
            }
            catch (Exception ex)
            {
                Logger.LogError("数据库连接关闭异常！{ 0}", ex.ToString());
            }

            try
            {
                GC.SuppressFinalize(this);
            }
            catch
            {
                // i
            }
        }
    }
}
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
    using Dapper;
    using DapperExtensions;

    using NPlatform.Config;
    using NPlatform.Domains.Entity;
    using NPlatform.Domains.IRepositories;
    using NPlatform.Filters;
    using NPlatform.Infrastructure.Loger;

    /// <summary>
    /// IUnitOfWork 的实现，此UnitOfWork 可以跨业务领域。
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The config.
        /// </summary>
        private static readonly NPlatformConfig Config = new ConfigFactory<NPlatformConfig>().Build();

        /// <summary>
        /// The df.
        /// </summary>
        private static DbProviderFactory df = null;

        /// <summary>
        /// 连接上下文
        /// </summary>
        private IDbConnection queryContext;

        /// <summary>
        /// The _connection string.
        /// </summary>
        private string connectionString;

        /// <summary>
        /// The trans.
        /// </summary>
        private IDbTransaction trans;
        const string DBProviderOracle = "System.Data.OracleClient";
        const string DBProviderMySql = "MySql.Data.MySqlClient";
        const string DBProviderSqlServer = "System.Data.SqlClient";
        /// <summary>
        /// 事务的配置项
        /// </summary>
        public IContextOptions Options { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class. 
        /// 创建连接  
        /// </summary>
        /// <param name="option">
        /// The option.
        /// </param>
        public UnitOfWork(IContextOptions option)
        {
            Options = option;
            if (!Config.ConnectionString.ContainsKey(option.ConnectName))
            {
                throw new Exception($"未找到指定key的连接字符串{option.ConnectName}");
            }

            this.connectionString = Config.ConnectionString[option.ConnectName];
            DapperExtensions.DefaultMapper = typeof(DapperExt.CustomClassMapper<>);
            if (ProviderFactoryString == DBProviderOracle)
            {
                DbProviderFactories.RegisterFactory(DBProviderOracle, System.Data.OracleClient.OracleClientFactory.Instance);
                DapperExtensions.SqlDialect = new global::DapperExtensions.Sql.OracleDialect();
            }
            else if (ProviderFactoryString == DBProviderSqlServer)
            {
                DbProviderFactories.RegisterFactory(DBProviderSqlServer, System.Data.SqlClient.SqlClientFactory.Instance);
                DapperExtensions.SqlDialect = new global::DapperExtensions.Sql.SqlServerDialect();
            }
            else if (ProviderFactoryString == DBProviderMySql)
            {
                DbProviderFactories.RegisterFactory(
                    DBProviderMySql,
                    new MySql.Data.MySqlClient.MySqlClientFactory());
                DapperExtensions.SqlDialect = new global::DapperExtensions.Sql.MySqlDialect();
            }
            else
            {
                DapperExtensions.SqlDialect = new global::DapperExtensions.Sql.SqlServerDialect();
            }

            if (df == null)
            {
                df = DbProviderFactories.GetFactory(ProviderFactoryString);
            }

            Timeout = option.TimeOut;
            this.BeginTrans = true;

            // 惰性加载，第一次使用时加载
            this.queryContext = df.CreateConnection();
            if (this.queryContext != null)
            {
                this.queryContext.ConnectionString = this.ConnectionString;
                this.queryContext.Open();
                if (this.BeginTrans)
                {
                    this.trans = this.queryContext.BeginTransaction();
                }

                System.Diagnostics.Debug.WriteLine($"{((object)this.queryContext).GetHashCode()} | QueryContext 创建");
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
        /// 得到web.config里配置项的数据库连接字符串。 
        /// </summary>
        /// <exception cref="Exception">
        /// </exception>
        public string ConnectionString
        {
            get
            {
                if (this.connectionString.IsNullOrEmpty())
                {
                    if (Config.ConnectionString.ContainsKey("default"))
                    {
                        this.connectionString = Config.ConnectionString["default"];
                        return this.connectionString;
                    }
                    else
                    {
                        throw new Exception("Connection string not set！");
                    }
                }
                else
                {
                    return this.connectionString;
                }
            }
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public virtual IDbConnection DBContext
        {
            get
            {
                if (this.queryContext != null)
                {
                    if (this.queryContext.State != ConnectionState.Open)
                    {
                        this.queryContext.Open();
                    }

                    // if(queryContext.State!= ConnectionState.Connecting||queryContext.ConnectionTimeout)
                    return this.queryContext;
                }

                this.queryContext = df.CreateConnection();
                if (this.queryContext != null)
                {
                    this.queryContext.ConnectionString = this.ConnectionString;
                    this.queryContext.Open();
                    if (this.BeginTrans)
                    {
                        this.trans = this.queryContext.BeginTransaction();
                    }

                    System.Diagnostics.Debug.WriteLine($"{this.queryContext.GetHashCode()} | TransConn 创建");
                    return this.queryContext;
                }
                else
                {
                    throw new System.Data.DataException("数据库链接创建失败！");
                }
            }
        }

        /// <summary>
        ///     获取 当前单元操作是否已被提交
        /// </summary>
        public bool IsCommitted { get; private set; }

        /// <summary>
        /// 连接超时设定
        /// </summary>
        public int? Timeout { get; set; }

        /// <summary>
        /// 数据库链接 得到工厂提供器类型
        /// </summary>
        /// <exception cref="Exception">
        /// </exception>
        private static string ProviderFactoryString
        {
            get
            {
                if (!Config.DBProvider.IsNullOrEmpty())
                {
                    return Config.DBProvider;
                }
                else
                {
                    throw new Exception("DBProvider not set！");
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 新增对象
        /// </summary>
        public virtual T Add<T>(T entity)
            where T : class, IEntity
        {
            SetFilter(entity);
            DBContext.Insert(entity, this.trans, Timeout);
            IsCommitted = false;
            return entity;
        }

        /// <summary>
        /// 新增对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entitys">实体集合</param>
        public virtual void Adds<T>(IEnumerable<T> entitys)
            where T : class, IEntity
        {
            this.SetFilter(entitys.ToArray());
            DBContext.Inserts(entitys, this.trans, Timeout);
            IsCommitted = false;
        }

        /// <summary>
        /// 更改对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">entity</param>
        /// <returns>修改结果</returns>
        public virtual bool Change<T>(T entity)
            where T : class, IEntity
        {
            var result = DBContext.Update<T>(entity, this.trans, Timeout);
            IsCommitted = false;
            return result;
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
                    LogerHelper.Error("UnitOfWork Commit异常！", "UnitOfWork", e);
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

                this.queryContext = null;
            }
            catch (Exception ex)
            {
                LogerHelper.Error("数据库连接关闭异常！", "UnitOfWork", ex);
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


        /// <summary>
        /// 删除对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>删除结果</returns>
        public virtual bool Remove<T>(T entity)
            where T : class, IEntity
        {
            var result = false;

            var enabled = Options.QueryFilters.ContainsKey(nameof(LogicDeleteFilter));
            if (typeof(ILogicDelete).IsAssignableFrom(typeof(T)) && enabled)
            {
                ((ILogicDelete)entity).IsDeleted = true;
                result = this.DBContext.Update<T>(entity, this.trans, this.Timeout);
            }
            else
            {
                result = this.DBContext.Delete<T>(entity, this.trans, this.Timeout);
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
        public virtual int Execute(string sql, DynamicParameters parameters)
        {
            if (sql.IsNullOrEmpty()) return -1;
            var result = this.DBContext.Execute(sql, parameters, this.trans, this.Timeout);
            IsCommitted = false;
            return result;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="filter">筛选表达式</param>
        /// <returns>返回类型</returns>
        public virtual bool Remove<T>(Expression<Func<T, bool>> filter)
            where T : class, IEntity
        {
            var result = false;
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
                    this.DBContext.Update<T>(entity, this.trans, this.Timeout);
                }
                result = true;
            }
            else
            {
                var predicate = QueryBuilder<T>.FromExpression(filter);
                result = DBContext.Delete<T>(predicate, this.trans, Timeout);

            }

            IsCommitted = false;

            return result;
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
                    LogerHelper.Error("Rollback Error！", "UnitOfWork", e);
                    throw e.InnerException;
                }

                throw;
            }
        }
    }
}
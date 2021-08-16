﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
//		如存在本生成代码外的新需求，请在相同命名空间下创建同名分部类实现分部方法。
// </auto-generated>
//
//------------------------------------------------------------------------------
/***********************************************************
**项目名称:	                                                                  				   
**功能描述:	  的摘要说明
**作    者: 	易栋梁                                         			   
**版 本 号:	1.0                                                  			   
**创建日期： 2017-08-09 16:49
**修改历史：
************************************************************/
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using DapperExtensions;
using ZJJWEPlatform.Infrastructure;
using ZJJWEPlatform.Domains;
using System.Collections;
using System.Collections.Generic;
using ZJJWEPlatform.Config;
using ZJJWEPlatform.Domains.IRepositories;
using System.Linq;
using System.Diagnostics;
using ZJJWEPlatform.Infrastructure.Loger;
using System.ComponentModel;
using MySql.Data.MySqlClient;
using System.Text;
using System.IO;
using System.Reflection;

namespace ZJJWEPlatform.Repositories
{
    /// <summary>
    /// ZJJWEPlatform上下文
    /// </summary>
    public class EPDBContext: IDbConnection
    {
        private volatile static ZJJWEPlatformConfig _config;
        private static readonly object LockHelper = new object();

        private static ZJJWEPlatformConfig config
        {
            get
            {
                if (_config == null)
                {
                    lock (LockHelper)
                    {
                        if (_config == null)
                        {
                            
                            _config = new ConfigFactory<ZJJWEPlatform.Config.ZJJWEPlatformConfig>().Build();
                        }
                    }
                }
                return _config;
            }
        }
        private string _ConnectionString = string.Empty;
        /// 得到web.config里配置项的数据库连接字符串。  
        public  string ConnectionString
        {
            get
            {
                if (_ConnectionString.IsNullOrEmpty())
                {
                    if (config.ConnectionString.ContainsKey("default"))
                    {
                        _ConnectionString = config.ConnectionString["default"];
                        return _ConnectionString;
                    }
                    else if(config.ConnectionString.ContainsKey("Master"))
                    {
                        _ConnectionString = config.ConnectionString["Master"];
                        return _ConnectionString;
                    }
                    else throw new Exception("Connection string not set！");
                }
                else
                    return _ConnectionString;
            }
            set
            {
                _ConnectionString = value;
            }
        }

        /// 得到工厂提供器类型  
        private static string ProviderFactoryString
        {
            get
            {
                if (!config.DBProvider.IsNullOrEmpty())
                {
                    var pros=config.DBProvider.Split('.');
                    return pros[pros.Length - 1];
                }
                else throw new Exception("DBProvider not set！");
            }
        } 
        private static DbProviderFactory df = null;
        /// <summary>
        /// 连接上下文
        /// </summary>
        protected IDbConnection _queryContext = null;
        private int commandTimeout = 60;

        /// <summary>
        /// 连接上下文
        /// </summary>
        protected IDbConnection Connection
        {
            get
            {
                if (_queryContext != null)
                {
                    if (_queryContext.State != ConnectionState.Open)
                    {
                        _queryContext.Open();
                    }
                    return _queryContext;
                }
                _queryContext = df.CreateConnection();
                _queryContext.ConnectionString = ConnectionString;
                _queryContext.Open();
                System.Diagnostics.Debug.WriteLine($"{(_queryContext as object).GetHashCode()} | TransConn 创建");
                return _queryContext;

            }
        }

        /// <summary>
        /// 超时时间
        /// </summary>
        public int ConnectionTimeout { get; set; }=180;

        /// <summary>
        /// 数据库
        /// </summary>
        public string Database
        {
            get
            {
                return Connection.Database;
            }
        }

        /// <summary>
        /// 连接状态
        /// </summary>
        public ConnectionState State {
            get
            {
                return Connection.State;
            }
        }
        /// <summary>  
        /// 创建连接  
        /// </summary>  
        public EPDBContext()
        {
            DBProvider provider;
            if (Enum.TryParse(ProviderFactoryString, out provider))
            {
                Init(ConnectionString, provider);
            }
            else
            {
                throw new Exception($"配置文件指定的连接驱动{ProviderFactoryString}不正确.");
            }
        }

        /// <summary>  
        /// 通过公用其他仓储的连接，实现连接共享，减少数据库连接次数。
        /// 注意及时关闭连接。
        /// </summary>  
        public EPDBContext(IDbConnection connection)
        {
            _queryContext = connection;

            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
        }
        /// <summary>  
        /// 创建连接  
        /// </summary>  
        public EPDBContext(string connectKey)
        {
            if (!config.ConnectionString.ContainsKey(connectKey))
                throw new Exception($"未找到指定key的连接字符串{connectKey}");
            _ConnectionString = config.ConnectionString[connectKey];
            DBProvider provider;
            if (Enum.TryParse(ProviderFactoryString, out provider))
            {
                Init(_ConnectionString, provider);
            }
            else
            {
                throw new Exception($"配置文件指定的连接驱动{ProviderFactoryString}不正确.");
            }
        }
        /// <summary>  
        /// 创建连接  
        /// </summary>  
        public EPDBContext(string connectionString, DBProvider dbProvider)
        {
            Init(connectionString, dbProvider);
        }
        private void Init(string connectionString, DBProvider dbProvider)
        {
            _ConnectionString = connectionString;
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(DapperExt.CustomClassMapper<>);
            switch (dbProvider)
            {
                case DBProvider.OracleClient:
                    {
                        // oracle 要引入oracle 包，并注入此工厂。
                        DbProviderFactories.RegisterFactory(DBProvider.OracleClient.ToString(), System.Data.OracleClient.OracleClientFactory.Instance);
                        DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.OracleDialect();
                        break;
                    }
                case DBProvider.MySqlClient:
                    {
                        DbProviderFactories.RegisterFactory(DBProvider.MySqlClient.ToString(), new MySql.Data.MySqlClient.MySqlClientFactory());
                        DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.MySqlDialect();
                        break;
                    }
                case DBProvider.SQLite:
                    {
                        DbProviderFactories.RegisterFactory(DBProvider.SQLite.ToString(), new System.Data.SQLite.SQLiteFactory());
                        DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.SqliteDialect();
                        break;
                    }
                default:
                    {
                        DbProviderFactories.RegisterFactory(DBProvider.SqlClient.ToString(), System.Data.SqlClient.SqlClientFactory.Instance);
                        DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.SqlServerDialect();
                        break;
                    }
            }
            if (df == null)
                df = DbProviderFactories.GetFactory(dbProvider.ToString());

        }
        /// <summary>
        /// 注销连接
        /// </summary>
        public void Dispose()
        {
            try
            {
                _queryContext.Close();
                _queryContext.Dispose();
            }
            catch (Exception ex)
            {
                LogerHelper.Error("数据库连接关闭异常！","DBContext", ex);
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
        /// 启动事务
        /// </summary>
        /// <returns></returns>
        public IDbTransaction BeginTransaction()
        {
            return Connection.BeginTransaction();
        }

        /// <summary>
        /// 类型是否为空
        /// </summary>
        /// <param name="t">t</param>
        /// <returns>bool</returns>
        private static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="t">类型</param>
        /// <returns>Type</returns>
        private static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }
        /// <summary>
        /// 大数据导入方法,注意
        /// </summary>
        /// <param name="dt">要导入的数据表，注意列头要和数据库匹配</param>
        /// <returns></returns>
        public int BulkLoad<T>(IEnumerable<T> datas)
        {

            var tableName = typeof(T).CustomAttributes.FirstOrDefault(t => t.AttributeType == typeof(Domains.TableName));
            string strName = "";
            // 1.通过特性移除表名中的下划线val.NamedArguments[0].TypedValue
            if (tableName != null)
            {
                strName = tableName.NamedArguments.First().TypedValue.Value.ToString();
            }
            else
            {
                throw new ZJJWEPlatformException("实体未指定 TableName 属性",null,"500");
            }

            var cacheFileInfo = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), strName + ".csv");

            var file = new System.IO.FileInfo(cacheFileInfo);
            if (!file.Directory.Exists)
            {
                try
                {
                    file.Directory.Create();
                }
                catch (System.IO.IOException ex)
                {
                    throw new ZJJWEPlatformException("导入失败，无法创建缓存目录！", ex, "500");
                }
                catch (UnauthorizedAccessException ex)
                {
                    throw new ZJJWEPlatformException("导入失败，无法创建缓存目录！", ex, "500");
                }
            }
            string csv = ListToCsv(datas);
            File.WriteAllText(cacheFileInfo, csv);

            MySqlBulkLoader bulk = new MySqlBulkLoader((MySqlConnection)Connection)
            {
                FieldTerminator = ",",
                FieldQuotationCharacter = '"',
                EscapeCharacter = '"',
                LineTerminator = "\r\n",
                FileName = file.FullName,
                NumberOfLinesToSkip = 0,
                TableName = strName,
                Local = true
            };
            var columns = new List<string>();
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            columns=props.Select(t => t.Name).ToList();

            bulk.Columns.AddRange(columns);
            return bulk.Load();
        }
        ///将DataTable转换为标准的CSV  
        /// </summary>  
        /// <param name="table">数据表</param>  
        /// <returns>返回标准的CSV</returns>  
        private static string ListToCsv<T>(IEnumerable<T> datas)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。  
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。  
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。 
            Type type = typeof(T);
            PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            StringBuilder sb = new StringBuilder();
            foreach (T entity in datas)
            {
                for(var i=0;i<props.Length;i++)
                {
                    var prop = props[i];
                    string colName = prop.Name;
                    Type t = GetCoreType(prop.PropertyType);
                    var targetAttribute = prop.GetCustomAttribute(typeof(DescriptionAttribute));
                    if (targetAttribute != null)
                    {
                        DescriptionAttribute descAttribute = targetAttribute as DescriptionAttribute;
                        if (descAttribute != null)
                        {
                            colName = descAttribute.Description;
                        }
                    }

                    var value= props[i].GetValue(entity, null).ToStrNoNull();

                    if (i != 0) sb.Append(",");

                    if (t.Name.ToLower() =="string")
                    {
                        if (value.Contains("\"") || value.Contains(","))
                        {
                            value = "\"" + value.Replace("\"", "\"\"") + "\"";
                        }
                        sb.Append(value);
                    }
                    else sb.Append(value);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="dt">要导入的数据表，注意列头要和数据库匹配</param>
        /// <returns></returns>
        public int BulkLoad(DataTable table)
        {

            var columns = table.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList();

            var cacheFileInfo = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),  table.TableName + ".csv");

            var file=new System.IO.FileInfo(cacheFileInfo);
            if (!file.Directory.Exists)
            {
                try
                {
                    file.Directory.Create();
                }
                catch(System.IO.IOException ex)
                {
                    throw new ZJJWEPlatformException("导入失败，无法创建缓存目录！",ex,"500");
                }
                catch (UnauthorizedAccessException ex)
                {
                    throw new ZJJWEPlatformException("导入失败，无法创建缓存目录！", ex, "500");
                }
            }
            string csv = DataTableToCsv(table);
            File.WriteAllText(cacheFileInfo, csv);

            MySqlBulkLoader bulk = new MySqlBulkLoader((MySqlConnection)Connection)
            {
                FieldTerminator = ",",
                FieldQuotationCharacter = '"',
                EscapeCharacter = '"',
                LineTerminator = "\r\n",
                FileName = file.FullName,
                NumberOfLinesToSkip = 0,
                TableName = table.TableName,
                Local = true
            };

            bulk.Columns.AddRange(columns);
            return bulk.Load();
        }

        ///将DataTable转换为标准的CSV  
        /// </summary>  
        /// <param name="table">数据表</param>  
        /// <returns>返回标准的CSV</returns>  
        private static string DataTableToCsv(DataTable table)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。  
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。  
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。  
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    colum = table.Columns[i];
                    if (i != 0) sb.Append(",");
                    var value = row[colum].ToStrNoNull();

                    if (colum.DataType == typeof(string))
                    {
                        if (value.Contains("\"") || value.Contains(","))
                        {
                            value = "\"" + value.Replace("\"", "\"\"") + "\"";
                        }
                        sb.Append(value);
                    }
                    else sb.Append(row[colum].ToString());
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
        /// <summary>
        /// 启动事务
        /// </summary>
        /// <param name="il"></param>
        /// <returns></returns>
        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return Connection.BeginTransaction(il);
        }
        /// <summary>
        /// 修改数据库
        /// </summary>
        /// <param name="databaseName"></param>
        public void ChangeDatabase(string databaseName)
        {
            Connection.ChangeDatabase(databaseName);
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            Connection.Close();
        }

        /// <summary>
        /// 创建命令
        /// </summary>
        /// <returns></returns>
        public IDbCommand CreateCommand()
        {

            return Connection.CreateCommand();
        }
        /// <summary>
        /// 打开连接
        /// </summary>
        public void Open()
        {
            Connection.Open();
        }
    }


    /// <summary>
    /// 数据库驱动类型
    /// </summary>
    public enum DBProvider
    {
        /// <summary>
        /// oracle客户端驱动
        /// </summary>
        [Description("System.Data.OracleClient")]
        OracleClient,// = "System.Data.OracleClient",
        /// <summary>
        /// MySqlClient 客户端驱动
        /// </summary>
        [Description("MySql.Data.MySqlClient")]
        MySqlClient,// "MySql.Data.MySqlClient",
        /// <summary>
        /// SqlClient 客户端驱动
        /// </summary>
        [Description("System.Data.SqlClient")]
        SqlClient,//"System.Data.SqlClient",
        /// <summary>
        /// SQLite 客户端驱动
        /// </summary>
        [Description("System.Data.SQLite")]
        SQLite
    }
}

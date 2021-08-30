/***********************************************************
**项目名称:	                                                                  				   
**功能描述:	  的摘要说明
**作    者: 	易栋梁                                         			   
**版 本 号:	1.0                                             			   
**创建日期： 2015/12/29 16:15:32
**修改历史：
************************************************************/

namespace NPlatform.Domains.Service
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using log4net;
    using log4net.Config;
    using log4net.Repository;
    using ServiceStack;
    using NPlatform.Config;
    using NPlatform.Domains.Entity;
    using NPlatform.Domains.IRepositories;
    using NPlatform.Domains.Service;
    using NPlatform.Infrastructure;
    using NPlatform.Infrastructure.Loger;
    using NPlatform.Result;

    /// <summary>
    /// 日志记录类
    /// 使用前先加载配置 。
    /// </summary>
    /// <summary> 
    ///    Loger  仓储操作
    /// </summary> 
    public partial class LogerService : ILogerService
    {
        /// <summary>
        /// loger reposityory
        /// </summary>
     //   public ILoggerRepository Log4netRepository { get; } = LogerHelper.LogRepository;

        /// <summary>
        /// 日志级别字典
        /// </summary>
        public Dictionary<int, string> LogLeves
        {
            get {
                return EnumExtend.GetDictionary<LogLevel>();
            }
        }
        /// <summary>
        /// The loger.
        /// </summary>
        private ILog Log { get; set; }

        private IPlatformHttpContext platformHttpContext;
        /// <summary>
        /// 平台配置
        /// </summary>
        private static NPlatformConfig platConfig = new ConfigFactory<NPlatformConfig>().Build();
        /// <summary>
        /// Initializes static members of the <see cref="LogerHelper"/> class.
        /// 构造
        /// </summary>
        public LogerService(IPlatformHttpContext httpContext)
        {
            Log = LogerHelper.Log;
            //// 配置log4net
            //log4net.Config.XmlConfigurator.Configure(Log4netRepository, new System.IO.FileInfo(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Config\\log4net.config")));

            //// 创建log实例
            //Log = LogManager.GetLogger(Log4netRepository.Name, AppDomain.CurrentDomain.FriendlyName);
            //var errors = Log.Logger.Repository.ConfigurationMessages;
            //if (errors.Count > 0)
            //{
            //    StringBuilder str = new StringBuilder();
            //    foreach (var error in errors)
            //    {
            //        str.AppendLine(error.ToString());
            //    }
            //    throw new Exception(str.ToString());
            //}
            platformHttpContext = httpContext;

        }
        private BusinessLog SetLog(BusinessLog log)
        {

            var save = platConfig.PlatformConfigs.ContainsKey("EnableDetail") ? platConfig.PlatformConfigs["EnableDetail"] : string.Empty;
            if (!save.IsNullOrEmpty() && !bool.Parse(save))
            {
                return log;
            }
            if (log == null) return log;
            if (platformHttpContext == null || platformHttpContext.Claims==null)
            {
                log.Creator = "未获取到登录信息";
                return log;
            }
            var claims = platformHttpContext.Claims;
            AuthInfoVO authInfo = new AuthInfoVO();
            if (claims.Any(t => t.Type.Contains("givenname")))
            {
                // Claims.FirstOrDefault(t => t.Subject.Name);
                authInfo.CnName = claims.FirstOrDefault(t => t.Type.Contains("givenname")).Value;
            }
            if (claims.Any(t => t.Type == "name"))
            {
                authInfo.Account = claims.FirstOrDefault(t => t.Type == "name").Value;
            }

            if (claims.Any(t => t.Type == "client_id"))
            {
                authInfo.ClientId = claims.FirstOrDefault(t => t.Type == "client_id").Value;
            }
            var path = $"{platformHttpContext.Context.Request.Method} {platformHttpContext.Context.Request.Path.Value} query: {platformHttpContext.Context.Request.QueryString}";

            log.Message = $"ClientID:{authInfo.ClientId}, {System.Environment.NewLine} Message:{ log.Message}";
            log.Creator = $"{authInfo.Account}:{authInfo.CnName}";
            log.LogLocation = path;
            log.LogBrowser = platformHttpContext.Context.Request.Headers["User-Agent"];
            return log;
        }

        /// <summary>
        /// 记录业务日志
        /// </summary>
        public void BLLSaveLog<T>(T entity, string message = "", LogType logType = LogType.Business) where T : IEntity
        {
            var save = platConfig.PlatformConfigs.ContainsKey("EnableBLLSaveLog") ? platConfig.PlatformConfigs["EnableBLLSaveLog"] : string.Empty;
            if (!save.IsNullOrEmpty()&&!bool.Parse(save))
            {
                return;
            }
            Type type = typeof(T);
            var val = type.CustomAttributes.FirstOrDefault(t => t.AttributeType == typeof(Domains.TableName));
            string moduleName = "";
            if (val != null)
                moduleName = val.NamedArguments.First().TypedValue.Value.ToString();
            else
                moduleName = type.Name;

            var business = new BusinessLog(entity.GetID(), moduleName.Split('_')[0], $"{moduleName}数据{message}", logType.ToString(), "");
            Log.Info(SetLog(business));
        }
        /// <summary>
        /// 记录业务日志
        /// </summary>
        public void BLLDeleteLog<T>(string Id, string message = "", LogType logType = LogType.Business) where T :IEntity
        {
            var save = platConfig.PlatformConfigs.ContainsKey("EnableBLLDeleteLog") ? platConfig.PlatformConfigs["EnableBLLDeleteLog"] : string.Empty;
            if (!save.IsNullOrEmpty() && !bool.Parse(save))
            {
                return;
            }
            Type type = typeof(T);
            var val = type.CustomAttributes.FirstOrDefault(t => t.AttributeType == typeof(Domains.TableName));
            string moduleName = "";
            if (val != null)
                moduleName = val.NamedArguments.First().TypedValue.Value.ToString();
            else
                moduleName = type.Name;

            var business = new BusinessLog(Id, moduleName.Split('_')[0], $"删除了{moduleName}表的{Id}数据,{message}", logType.ToString(), "");
            Log.Info(SetLog(business));
        }

        /// <summary>
        /// 记录
        /// </summary>
        /// <param name="msg">
        /// The msg.
        /// </param>
        /// <param name="module">
        /// The log module.
        /// </param>
        public void Info(string msg, string module = "system")
        {
            var log = SetLog(new BusinessLog("", module, msg, LogType.Trace.ToString(), ""));
            Log.Info(log);
        }


        /// <summary>
        /// 调试日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="module">业务模块</param>
        public void Debug(string msg, string module = "system")
        {

            Log.Debug(SetLog(new BusinessLog("", module, msg, LogType.Trace.ToString(), "")));
        }
        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="ex">异常</param>
        /// <param name="module">模块</param>
        public void Error(string msg, Exception ex, string module = "system")
        {
            Log.Error(SetLog(new BusinessLog("", module, msg, LogType.Trace.ToString(), "")), ex);
        }

        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="msg">消息</param>
        /// <param name="module">模块</param>
        public void Fatal(string msg, Exception ex, string module = "system")
        {
            Log.Fatal(SetLog(new BusinessLog("", module, msg, LogType.Trace.ToString(), "")), ex);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="module">模块</param> 
        public void Warn(string msg, string module = "system")
        {
            Log.Warn(SetLog(new BusinessLog("", module, msg, LogType.Trace.ToString(), "")));
        }

        /// <summary>
        /// 查询日志列表
        /// </summary>
        /// <param name="key">查询关键字</param>
        /// <param name="moduleName">模块名</param>
        /// <param name="logLeve">日志级别</param>
        /// <param name="account">账号</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public virtual IListResult<Loger> GetLogers(string key, string moduleName, string logLeve, string account, DateTime? beginDate, DateTime? endDate, int page, int pageSize)
        {
            throw new NotImplementedException("子类未实现此方法");
        }
    }
}
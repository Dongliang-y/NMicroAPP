/***********************************************************
**项目名称:	                                                                  				   
**功能描述:	  的摘要说明
**作    者: 	易栋梁                                         			   
**版 本 号:	1.0                                             			   
**创建日期： 2015/12/29 16:15:32
**修改历史：
************************************************************/

namespace NPlatform.Infrastructure.Loger
{
    using System;
    using System.IO;
    using System.Text;
    using log4net;
    using log4net.Config;
    using log4net.Repository;

    /// <summary>
    /// 日志记录类
    /// 使用前先加载配置 。
    /// </summary>
   [Obsolete("LogerHelper已过期，后续请使用各基类中的Loger对象进行日志记录。", false)]
    public static class LogerHelper
    {
        /// <summary>
        /// loger reposityory
        /// </summary>
        public static ILoggerRepository LogRepository { get; } = LogManager.CreateRepository("NETCoreRepository");

        /// <summary>
        /// The loger.
        /// </summary>
        public static ILog Log{ get; set; }

        /// <summary>
        /// Initializes static members of the <see cref="LogerHelper"/> class.
        /// 构造
        /// </summary>
        static LogerHelper()
        {
            // 配置log4net
            log4net.Config.XmlConfigurator.Configure(LogRepository, new System.IO.FileInfo(System.IO.Path.Combine( System.IO.Directory.GetCurrentDirectory(),"Config\\log4net.config")));

            // 创建log实例
           Log = LogManager.GetLogger(LogRepository.Name, AppDomain.CurrentDomain.FriendlyName);
            var errors = Log.Logger.Repository.ConfigurationMessages;
            if(errors.Count>0)
            {
                StringBuilder str = new StringBuilder();
                foreach(var error in errors)
                {
                    str.AppendLine(error.ToString());
                }
                throw new Exception(str.ToString());
            }
        }

        /// <summary>
        /// 记录
        /// </summary>
        /// <param name="msg">
        /// The msg.
        /// </param>
        /// <param name="logType">
        /// The log Type.
        /// </param>
        public static void Info(string msg, string logType = "")
        {
            Log.Info(new BusinessLog("", "system", msg,logType,""));
        }

        /// <summary>
        /// 记录业务日志
        /// </summary>
        /// <param name="log">业务数据对象</param>
        [Obsolete("BusinessLog 已停用，后续请使用各基类中的Loger对象进行日志记录。", true)]
        public static void BusinessLog(BusinessLog log)
        {
            Log.Info(log);
        }

        /// <summary>
        /// 调试日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="logType">消息类型</param>
        public static void Debug(string msg, string logType = "")
        {
            Log.Debug(new BusinessLog("", "system", msg, logType, ""));
        }

        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="logType">消息类型</param>
        public static void Error(string msg, string logType)
        {
            Log.Error(new BusinessLog("", "system", msg, logType, ""));
        }

        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="logType">消息类型</param>
        public static void Error(Exception ex, string logType="error")
        {
            Log.Error(new BusinessLog("", "system", ex.Message + ex.ToString(), logType, ""), ex);
        }

        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="ex">异常</param>
        /// <param name="logType">消息类型</param>
        public static void Error(string msg, string logType, Exception ex)
        {
            Log.Error(new BusinessLog("", "system", msg+ex.ToString(), logType, ""),ex);
        }

        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="log">业务消息</param>
        public static void Error(BusinessLog log, Exception ex)
        {
            Log.Error(log, ex);
        }

        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="msg">消息</param>
        /// <param name="module">模块</param>
        public static void Fatal(string msg, string module, Exception ex)
        {
            Log.Fatal(new BusinessLog("", module, msg, "Fatal", "system"), ex);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="logType">消息类型</param> 
        public static void Warn(string msg, string logType)
        {
            Log.Warn(new BusinessLog("", "", msg, logType, ""));
        }
        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="log">业务消息</param>
        public static void Warn(BusinessLog log)
        {
            Log.Warn(log);
        }
    }
}
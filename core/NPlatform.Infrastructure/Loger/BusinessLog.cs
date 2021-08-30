/*************************************************************************************
  * CLR版本：       4.0.30319.42000
  * 类 名 称：       BusinessLog
  * 机器名称：       DESKTOP123
  * 命名空间：       NPlatform.Infrastructure.Loger
  * 文 件 名：       BusinessLog
  * 创建时间：       2020-5-13 9:16:30
  * 作    者：          xxx
  * 说   明：。。。。。
  * 修改时间：
  * 修 改 人：
*************************************************************************************/
using log4net.Core;
using log4net.Layout;
using log4net.Layout.Pattern;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NPlatform.Infrastructure
{
    /// <summary>
    /// 业务日志
    /// </summary>
    public class BusinessLog
    {
        /// <summary>
        /// 业务数据ID
        /// </summary>
        public string BusinessID { get; set; }
        /// <summary>
        /// 模块
        /// </summary>
        public string Module { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 类型，默认为操作日志记录
        /// </summary>
        public string LogType { get; set; }
        /// <summary>
        /// 类型，默认为操作日志记录
        /// </summary>
        public string LogLocation { get; set; }
        /// <summary>
        /// 类型，默认为操作日志记录
        /// </summary>
        public string LogBrowser { get; set; }

        /// <summary>
        /// 机器
        /// </summary>
        public string LogMachineName { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 业务数据
        /// </summary>
        /// <param name="businessID">数据ID</param>
        /// <param name="module">模块</param>
        /// <param name="message">消息</param>
        /// <param name="logType">类型</param>
        /// <param name="creator">创建者</param>
        public BusinessLog( string businessID,string module,string message,string logType, string creator)
        {
            this.BusinessID = businessID;
            this.Module = module;
            this.Message = message;
            this.LogType = logType;
            this.Creator = creator;
        }
    }

    /// <summary>
    /// 业务数据ID
    /// </summary>
    public class BusinessIDPatternConvert : PatternLayoutConverter
    {
        /// <summary>
        /// 业务数据ID
        /// </summary>
        ///  <param name="writer">writer</param>
        /// <param name="loggingEvent">loggingEvent</param>
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            var business = loggingEvent.MessageObject as BusinessLog;
            if (business == null) return;

            writer.Write(business.BusinessID);
        }
    }
    /// <summary>
    ///  Creator
    /// </summary>
    public class CreatorPatternConvert : PatternLayoutConverter
    {
        /// <summary>
        ///  Creator
        /// </summary>
        ///  <param name="writer">writer</param>
        /// <param name="loggingEvent">loggingEvent</param>
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            var business = loggingEvent.MessageObject as BusinessLog;
            if (business == null) return;

            writer.Write(business.Creator);
        }
    }
    /// <summary>
    ///  Creator
    /// </summary>
    public class MessagePatternConvert : PatternLayoutConverter
    {
        /// <summary>
        ///  Message
        /// </summary>
        ///  <param name="writer">writer</param>
        /// <param name="loggingEvent">loggingEvent</param>
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            var business = loggingEvent.MessageObject as BusinessLog;
            if (business == null) return;

            writer.Write(business.Message);
        }
    }
    /// <summary>
    ///   Module
    /// </summary>
    public class ModulePatternConvert : PatternLayoutConverter
    {
        /// <summary>
        ///   Module
        /// </summary>
        ///  <param name="writer">writer</param>
        /// <param name="loggingEvent">loggingEvent</param>
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            var business = loggingEvent.MessageObject as BusinessLog;
            if (business == null) return;

            writer.Write(business.Module);
        }
    }
    /// <summary>
    ///  Type
    /// </summary>
    public class TypePatternConvert : PatternLayoutConverter
    {
        /// <summary>
        ///  Type
        /// </summary>
        ///  <param name="writer">writer</param>
        /// <param name="loggingEvent">loggingEvent</param>/// </summary>
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            var business = loggingEvent.MessageObject as BusinessLog;
            if (business == null) return;

            writer.Write(business.LogType);
        }
    }

    /// <summary>
    /// 业务数据ID
    /// </summary>
    public class LogLocationPatternConvert : PatternLayoutConverter
    {
        /// <summary>
        /// 业务数据ID
        /// </summary>
        ///  <param name="writer">writer</param>
        /// <param name="loggingEvent">loggingEvent</param>
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            var business = loggingEvent.MessageObject as BusinessLog;
            if (business == null) return;

            writer.Write(business.LogLocation);
        }
    }
    /// <summary>
    /// 业务数据ID
    /// </summary>
    public class LogBrowserPatternConvert : PatternLayoutConverter
    {
        /// <summary>
        /// 业务数据ID
        /// </summary>
        ///  <param name="writer">writer</param>
        /// <param name="loggingEvent">loggingEvent</param>
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            var business = loggingEvent.MessageObject as BusinessLog;
            if (business == null) return;

            writer.Write(business.LogBrowser);
        }
    }

}

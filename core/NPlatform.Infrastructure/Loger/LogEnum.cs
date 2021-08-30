/*************************************************************************************
  * CLR版本：       4.0.30319.42000
  * 类 名 称：       LogLevel
  * 机器名称：       DESKTOP123
  * 命名空间：       NPlatform.Infrastructure.Loger
  * 文 件 名：       LogLevel
  * 创建时间：       2020-5-20 17:13:37
  * 作    者：          xxx
  * 说   明：。。。。。
  * 修改时间：
  * 修 改 人：
*************************************************************************************/
using System;
using System.Collections;

using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NPlatform.Infrastructure.Loger
{
    /// <summary>
    /// 日志级别的keyvalue 字典描述
    /// </summary>
    public class LogLeveDic
    {
        /// <summary>
        /// 日志级别的keyvalue 字典描述
        /// </summary>
        public Dictionary<string,string> LogLeveDics
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { "Debug","调试信息" },
                    { "Info","操作记录" },
                    { "Warn","警告信息" },
                    { "Error","异常信息" },
                    { "Fatal", "致命错误" },
                };
            }
        }
    } 

    /// <summary>
    /// 日志级别
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 调试
        /// </summary>
        [Description("调试信息")]
        Debug,

        /// <summary>
        /// 信息记录
        /// </summary>
        [Description("操作记录")]
        Info,

        /// <summary>
        /// 警告
        /// </summary>
        [Description("警告信息")]
        Warn,

        /// <summary>
        /// 异常
        /// </summary>
        [Description("异常信息")]
        Error,

        /// <summary>
        /// 致命错误
        /// </summary>
        [Description("致命错误")]
        Fatal
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 业务日志
        /// </summary>
        [Description("业务日志")]
        Business,
        /// <summary>
        /// 系统跟踪日志。
        /// </summary>
        [Description("系统跟踪日志")]
        Trace
    }
}

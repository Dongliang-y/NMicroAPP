/***********************************************************
**项目名称:	                                                                  				   
**功能描述:	  的摘要说明
**作    者: 	易栋梁                                         			   
**版 本 号:	1.0                                             			   
**创建日期： 2015/12/29 16:15:32
**修改历史：
************************************************************/

using log4net.Repository;
using System;
using System.Collections.Generic;
using NPlatform.Domains.Entity;
using NPlatform.Infrastructure.Loger;
using NPlatform.Result;

namespace NPlatform.Domains.Service
{
    /// <summary>
    /// 日志服务
    /// </summary>
    public interface ILogerService
    {
        /// <summary>
        /// 日志级别定义字典
        /// </summary>
        Dictionary<int, string> LogLeves { get; }
        /// <summary>
        /// 业务日志的删除日志
        /// </summary>
        /// <typeparam name="T">业务实体类型</typeparam>
        /// <param name="Id">业务数据ID</param>
        /// <param name="message">消息</param>
        /// <param name="logType">日志类型</param>
        void BLLDeleteLog<T>(string Id, string message = "", LogType logType = LogType.Business) where T : IEntity;
        /// <summary>
        /// 报错业务数据
        /// </summary>
        /// <typeparam name="T">业务实体类型</typeparam>
        /// <param name="entity">业务对象</param>
        /// <param name="message">消息</param>
        /// <param name="logType">日志类型</param>
        void BLLSaveLog<T>(T entity, string message = "", LogType logType = LogType.Business) where  T : IEntity;
        /// <summary>
        /// 调试日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="module">模块，可选</param>
        void Debug(string msg, string module = "system");
        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="ex">异常</param>
        /// <param name="module">模块，可选</param>
        void Error(string msg, Exception ex, string module = "system");
        /// <summary>
        /// 致命的系统错误
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="ex">异常</param>
        /// <param name="module">模块，可选</param>
        void Fatal(string msg, Exception ex, string module = "system");
        /// <summary>
        /// 事件记录
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="module">模块，可选</param>
        void Info(string msg, string module = "system");
        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="module">模块，可选</param>
        void Warn(string msg, string module = "system");
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
        IListResult<Loger> GetLogers(string key, string moduleName, string logLeve, string account, DateTime? beginDate, DateTime? endDate, int page, int pageSize);
    }
}
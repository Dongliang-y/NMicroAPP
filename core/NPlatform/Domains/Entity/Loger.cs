/***********************************************************
**项目名称:EFrame.Entity                                                                				   
**功能描述:	 Loger    的摘要说明
**作    者: 	易栋梁                                         			   
**版 本 号:	1.0                                                  
**修改历史：
************************************************************/

namespace NPlatform.Domains.Entity
{
    using System;

    using NPlatform;
    using NPlatform.Domains;
    using NPlatform.Domains.Entity;
        using NPlatform.Domains;
    /// <summary>
    ///Loger数据实体
    /// </summary>
    [Serializable()]
    [TableName(TabName = "Sys_Loger")]
    public partial class Loger : AggregationBase<string>
    {
        ///<summary>
        /// 浏览器
        ///</summary>
        public string LogBrowser { get; set; }

        ///<summary>
        /// 记录时间
        ///</summary>
        public DateTime LogDate { get; set; }

        ///<summary>
        /// 异常信息
        ///</summary>
        public string LogException { get; set; }

        ///<summary>
        /// IP
        ///</summary>
        public string LogIP { get; set; }

        ///<summary>
        /// 日志级别
        ///</summary>
        public string LogLevel { get; set; }

        ///<summary>
        /// 请求地址
        ///</summary>
        public string LogLocation { get; set; }

        ///<summary>
        /// 记录人
        ///</summary>
        public string LogLogger { get; set; }

        ///<summary>
        /// 机器名
        ///</summary>
        public string LogMachineName { get; set; }

        ///<summary>
        /// 内容
        ///</summary>
        public string LogMessage { get; set; }

        ///<summary>
        /// 操作人
        ///</summary>
        public string LogOperator { get; set; }

        ///<summary>
        /// 线程
        ///</summary>
        public string LogThread { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }
    }
}
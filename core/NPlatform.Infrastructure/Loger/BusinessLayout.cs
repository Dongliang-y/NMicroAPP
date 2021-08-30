/*************************************************************************************
  * CLR版本：       4.0.30319.42000
  * 类 名 称：       BusinessLayout
  * 机器名称：       DESKTOP123
  * 命名空间：       NPlatform.Infrastructure.Loger
  * 文 件 名：       BusinessLayout
  * 创建时间：       2020-5-13 10:52:36
  * 作    者：          xxx
  * 说   明：。。。。。
  * 修改时间：
  * 修 改 人：
*************************************************************************************/
using log4net.Layout;
using System;
using System.Collections.Generic;
using System.Text;

namespace NPlatform.Infrastructure
{
    /// <summary>
    /// 业务日志格式化
    /// </summary>
    public class BusinessLayout : PatternLayout
    {
        /// <summary>
        /// 业务日志格式化
        /// </summary>
        public BusinessLayout()
        {
            this.AddConverter("businessID", typeof(BusinessIDPatternConvert));
            this.AddConverter("creator", typeof(CreatorPatternConvert));
            this.AddConverter("message", typeof(MessagePatternConvert));
            this.AddConverter("module", typeof(ModulePatternConvert));
            this.AddConverter("logType", typeof(TypePatternConvert));
            this.AddConverter("logBrowser", typeof(LogBrowserPatternConvert));
            this.AddConverter("logLocation", typeof(LogLocationPatternConvert));
        }
    }
}

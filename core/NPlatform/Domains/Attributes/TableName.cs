/********************************************************************************

** auth： DongliangYi

** date： 2017/11/3 15:27:06

** desc： class Attribute for Set TableName

** Ver.:  V1.0.0

*********************************************************************************/

namespace NPlatform.Domains
{
    using System;

    /// <summary>
    /// class Attribute for Set TableName
    /// </summary>
    public class TableName : Attribute
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TableName"/> class. 
        /// 表名特性
        /// </summary>
        public TableName()
        {
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string TabName { get; set; }
    }
}
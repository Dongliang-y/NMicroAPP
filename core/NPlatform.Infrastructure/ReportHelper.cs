/********************************************************************************

** auth： Dongliang YI

** desc： 报表控制器基类
*********************************************************************************/

using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPlatform.Infrastructure
{
    /// <summary>
    /// 报表 BaseController
    /// </summary>
    public class ReportHelper
    {
        /// <summary>
        /// 把对象序列化为帆软报表要求的数据源格式。
        /// </summary>
        /// <param name="t">需要转换的对象</param>
        /// <param name="descriptionAsHeard">是否使用字段Description特性作为表头</param>
        public static string ObjectToTabString<T>(T t, bool descriptionAsHeard = false)
        {
            try
            {
                System.Reflection.PropertyInfo[] properties = typeof(T)
                        .GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                if (properties.Length <= 0)
                {
                    return string.Empty;
                }

                StringBuilder str = new StringBuilder();
                for (int i = 0; i < properties.Length; i++)
                {
                    var item = properties[i];
                    var col = item.Name;
                    if (descriptionAsHeard)
                    {
                        var desc = item.GetDescription();
                        if (!desc.IsNullOrEmpty())
                        {
                            col = desc;
                        }
                    }

                    str.AppendFormat("{0},", col);
                }

                str.AppendLine();

                if (t != null)
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        var item = properties[i];
                        object value = item.GetValue(t, null);
                        var val = value == null ? " " : value.ToString().Trim();

                        val = ReplaceSepcialStr(val);
                        if (val.IsNullOrEmpty())
                        {
                            val = " ";
                        }

                        str.AppendFormat("{0},", val);
                    }

                    str.AppendLine();
                }

                return str.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// 把数据集合序列化为帆软报表要求的数据源格式。
        /// </summary>
        /// <param name="datas">需要转换的集合</param>
        /// <param name="descriptionAsHeard">是否使用字段Description特性作为表头</param>
        public static string EnumerableToTabString<T>(IEnumerable<T> datas, bool descriptionAsHeard = false)
        {
            System.Reflection.PropertyInfo[] properties = (datas?.FirstOrDefault()?.GetType() ?? typeof(T))
                    .GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return string.Empty;
            }

            StringBuilder str = new StringBuilder();
            for (int i = 0; i < properties.Length; i++)
            {
                var item = properties[i];

                var col = item.Name;
                if (descriptionAsHeard)
                {
                    var desc = item.GetDescription();
                    if (!desc.IsNullOrEmpty())
                    {
                        col = desc;
                    }
                }

                str.AppendFormat("{0},", col);
            }

            str.AppendLine();

            if (datas == null)
            {
                return str.ToString();
            }

            foreach (var item in datas)
            {
                System.Reflection.PropertyInfo[] itemPr = (item?.GetType() ?? typeof(T))
                   .GetProperties(System.Reflection.BindingFlags.Instance |
                                  System.Reflection.BindingFlags.Public);

                if (item == null || itemPr.Length <= 0)
                {
                    continue;
                }

                foreach (var it in itemPr)
                {
                    object value = it.GetValue(item, null);
                    var val = value == null ? " " : value.ToString().Trim();

                    val = ReplaceSepcialStr(val);
                    if (val.IsNullOrEmpty())
                    {
                        val = " ";
                    }

                    str.AppendFormat("{0},", val);
                }

                str.AppendLine();
            }

            return str.ToString();
        }

        /// <summary>
        /// 替换换行、回车、制表符、逗号，这四种会导致帆软报表异常的字符。
        ///  oldStr.Replace("\r\n", "&at;").Replace("\t", " ").Replace("\n", "&at;").Replace("\r", "&at;").Replace(",", " ");
        /// </summary>
        /// <param name="oldStr">替换前的字符串</param>
        /// <returns>替换后的字符串</returns>
        public static string ReplaceSepcialStr(string oldStr)
        {
            if (!string.IsNullOrEmpty(oldStr))
            {
                return oldStr.Replace("\r\n", "&at;").Replace("\t", " ").Replace("\n", "&at;").Replace("\r", "&at;").Replace(",", "，");
            }
            else
            {
                return " ";
            }
        }
    }
}
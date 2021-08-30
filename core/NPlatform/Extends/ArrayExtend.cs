using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NPlatform.Extends
{
    /// <summary>
    /// 集合扩展类
    /// </summary>
    public static class ArrayExtend
    {
        /// <summary>
        /// 集合为空或者长度为0
        /// </summary>
        /// <param name="strs">字符串集合</param>
        /// <returns>是否为空集合</returns>
        public static bool IsNullOrEmpty(this string[] strs)
        {
            return strs == null|| strs.Length == 0;
        }
        /// <summary>
        /// 集合为空或者长度为0
        /// </summary>
        /// <param name="strs">字符串集合</param>
        /// <returns>是否为空集合</returns>
        public static bool IsNullOrEmpty(this List<string> strs)
        {
            return strs == null || strs.Count() == 0;
        }

        /// <summary>
        /// 集合为空或者长度为0
        /// </summary>
        /// <param name="datas">集合</param>
        /// <returns>是否为空集合</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> datas)
        {
            return datas == null || datas.Count() == 0;
        }
    }
}

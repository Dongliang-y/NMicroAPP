/***********************************************************
**项目名称:	                                     
**类    名:	                                 				   
**功能描述:	                                       					   
**作    者: 	易栋梁                                         			   
**版 本 号:	1.0                                             			   
**创建日期：2015/3/21 
**修改历史：
************************************************************/

namespace NPlatform
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using NPlatform.Infrastructure;

    /// <summary>
    ///     处理枚举
    /// </summary>
    public static class EnumExtend
    {
        /// <summary>
        ///     获取描述信息
        /// </summary>
        /// <param name="en">枚举</param>
        /// <returns></returns>
        public static string GetEnumDes(this Enum en)
        {
            var type = en.GetType();
            var memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return en.ToString();
        }

        /// <summary>
        /// 指定枚举的枚举项的字典集合,value对应 key，描述对应value。
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetDictionary<TEnum>() where TEnum : struct, Enum
        {
            var etype = typeof(TEnum);
          
            var values = Enum.GetValues(etype);

            Dictionary<int, string> items = new Dictionary<int, string>();
            foreach (var val in values)
            {
                TEnum em;
                if (Enum.TryParse<TEnum>(val.ToString(), out em))
                {
                    items.Add(Convert.ToInt32(val), em.GetEnumDes());
                }
            }
            return items;
        }

        /// <summary>
        ///     把int 值转为枚举
        /// </summary>
        /// <param name="val">枚举</param>
        /// <returns></returns>
        public static T ToEnum<T>(this int val)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), val.ToString());
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        ///     获取描述信息
        /// </summary>
        /// <param name="en">枚举</param>
        /// <returns></returns>
        public static int ToInt(this Enum en)
        {
            try
            {
                return Convert.ToInt32(en);
            }
            catch
            {
                return int.MinValue;
            }
        }
    }
}
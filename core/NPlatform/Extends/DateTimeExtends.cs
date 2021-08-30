/********************************************************************************

** auth： DongliangYi

** date： 2018-8-31 17:29:27

** desc： 尚未编写描述

** Ver.:  V1.0.0

*********************************************************************************/

namespace NPlatform
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// 时间扩展类
    /// </summary>
    public static class DateTimeExtends
    {
        /// <summary>
        /// 是日期否
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>是/否</returns>
        public static bool IsDateTime(this string input)
        {
            // string pet = @"^(?:(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(\/|-|\.)(?:0?2\1(?:29))$)|(?:(?:1[6-9]|[2-9]\d)?\d{2})(\/|-|\.)(?:(?:(?:0?[13578]|1[02])\2(?:31))|(?:(?:0?[1,3-9]|1[0-2])\2(29|30))|(?:(?:0?[1-9])|(?:1[0-2]))\2(?:0?[1-9]|1\d|2[0-8]))$";
            string pet =
                @"^(?=\d)(?:(?!(?:1582(?:\.|-|\/)10(?:\.|-|\/)(?:0?[5-9]|1[0-4]))|(?:1752(?:\.|-|\/)0?9(?:\.|-|\/)(?:0?[3-9]|1[0-3])))(?=(?:(?!000[04]|(?:(?:1[^0-6]|[2468][^048]|[3579][^26])00))(?:(?:\d\d)(?:[02468][048]|[13579][26]))\D0?2\D29)|(?:\d{4}\D(?!(?:0?[2469]|11)\D31)(?!0?2(?:\.|-|\/)(?:29|30))))(\d{4})([-\/.])(0?\d|1[012])\2((?!00)[012]?\d|3[01])(?:$|(?=\x20\d)\x20))?((?:(?:0?[1-9]|1[012])(?::[0-5]\d){0,2}(?:\x20[aApP][mM]))|(?:[01]?\d|2[0-3])(?::[0-5]\d){1,2})?$";
            return input.IsMatch(pet);
        }

        /// <summary>
        /// 将字符串转成日期时间型
        /// </summary>
        public static DateTime ToDateTime(this string dateTimeStr, string formatStr = "yyyy/MM/dd HH:mm:ss")
        {
            var rst = DateTime.ParseExact(
                dateTimeStr,
                formatStr,
                new System.Globalization.CultureInfo("zh-CN", true),
                System.Globalization.DateTimeStyles.AllowInnerWhite);
            return rst;
        }

        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ToUnixTimestamp(this DateTime time)
        {
            // 转成国际标准时间，再获unix时间点
            var t = (time.ToUniversalTime().Ticks - 621355968000000000) / 10000;
            return t;
        }

        /// <summary>        
        /// 时间戳转为C#格式时间        
        /// </summary>        
        /// <param name="timeStamp"></param>        
        /// <returns></returns>        

        public static DateTime UnixTimestampToDateTime(this string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
    }
}
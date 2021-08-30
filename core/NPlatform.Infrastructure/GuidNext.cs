/********************************************************************************

** auth： DongliangYi

** date： 2017/11/3 16:40:46

** desc： 尚未编写描述

** Ver.:  V1.0.0

*********************************************************************************/

namespace NPlatform.Infrastructure
{
    using System;

    /// <summary>
    /// 获取一个GUID，通过使用new guid +当前时刻
    /// </summary>
    public class GuidNext
    {
        /// <summary>
        /// 获取下一个GUID
        /// </summary>
        /// <returns></returns>
        public static Guid Next()
        {
            byte[] b = Guid.NewGuid().ToByteArray();
            DateTime dateTime = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;
            TimeSpan timeSpan = new TimeSpan(now.Ticks - dateTime.Ticks);
            TimeSpan timeOfDay = now.TimeOfDay;
            byte[] bytes1 = BitConverter.GetBytes(timeSpan.Days);
            byte[] bytes2 = BitConverter.GetBytes((long)(timeOfDay.TotalMilliseconds / 3.333333));
            Array.Reverse(bytes1);
            Array.Reverse(bytes2);
            Array.Copy(bytes1, bytes1.Length - 2, b, b.Length - 6, 2);
            Array.Copy(bytes2, bytes2.Length - 4, b, b.Length - 4, 4);
            return new Guid(b);
        }
    }
}
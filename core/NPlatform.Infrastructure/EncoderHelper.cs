/***********************************************************
**项目名称:	                                     
**类    名:	                                 				   
**功能描述:	                                       					   
**作    者: 	易栋梁                                         			   
**版 本 号:	1.0                                             			   
**创建日期：2015/3/21 
**修改历史：
************************************************************/

namespace NPlatform.Infrastructure
{
    using System;
    using System.Text;

    /// <summary>
    /// 编码助手
    /// </summary>
    public static class EncoderHelper
    {
        /// <summary>
        /// 将Base64字符串解码为普通字符串
        /// </summary>
        /// <param name="str">要解码的字符串</param>
        public static string Base64Decode(string str)
        {
            byte[] barray;
            barray = Convert.FromBase64String(str);
            return Encoding.Default.GetString(barray);
        }

        /// <summary>
        /// 将字符串编码为Base64字符串
        /// </summary>
        /// <param name="str">要编码的字符串</param>
        public static string Base64Encode(string str)
        {
            byte[] barray;
            barray = Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(barray);
        }

        /// <summary>
        /// 模拟Javascript中escape的串编码实现
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Escape(string str)
        {
            if (str == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            int len = str.Length;

            for (int i = 0; i != len; i++)
            {
                char c = str[i];

                // everything other than the optionally escaped chars _must_ be escaped 
                if (char.IsLetterOrDigit(c) || c == '-' || c == '_' || c == '/' || c == '\\' || c == '.')
                    sb.Append(c);
                else
                    sb.Append(Uri.HexEscape(c));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 模拟Javascript中unescape的解码实现
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UnEscape(string str)
        {
            if (str == null)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            int len = str.Length;
            int i = 0;
            while (i != len)
            {
                if (Uri.IsHexEncoding(str, i))
                    sb.Append(Uri.HexUnescape(str, ref i));
                else
                    sb.Append(str[i++]);
            }

            return sb.ToString();
        }
    }
}
// <copyright file="HttpHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/********************************************************************************

** auth： DongliangYi

** date： 2018-6-4 20:40:21

** desc： 尚未编写描述

** Ver.:  V1.0.0

*********************************************************************************/

namespace NPlatform.Infrastructure
{
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Reflection.Metadata.Ecma335;
    using System.Text;
    /// <summary>
    /// http 处理
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// http get 请求
        /// </summary>
        /// <param name="strUrl"></param>
        /// <param name="strParams"></param>
        /// <param name="encoding"></param>
        /// <param name="contentType"></param>
        /// <param name="heards"></param>
        public static string HttpGet(string strUrl, string strParams, string contentType = "application/json")
        {
            return HttpRequest(strUrl, strParams, "GET", Encoding.Default, contentType, null);
        }

        /// <summary>
        /// http post 请求
        /// </summary>
        /// <param name="strUrl"></param>
        /// <param name="strParams"></param>
        /// <param name="encoding"></param>
        /// <param name="contentType"></param>
        /// <param name="heards"></param>
        public static string HttpPost(string strUrl, string strParams, string contentType = "application/json")
        {
            return HttpRequest(strUrl, strParams, "POST", Encoding.Default, contentType, null);
        }

        /// <summary>
        /// http 请求
        /// </summary>
        /// <param name="strUrl">请求地址 </param>
        /// <param name="strParams">Post参数 json格式的请求报文,例如：{"key1":"value1","key2":"value2"}</param>
        /// <param name="method">请求方法</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="contentType"> contentType </param>
        /// <param name="heards">请求头 </param>
        /// <returns>string</returns>
        public static string HttpRequest(string strUrl, string strParams, string method, Encoding encoding = default(Encoding), string contentType = "application/json", params KeyValuePair<string, string>[] heards)
        {
            // 以上是POST数据的写入
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
            request.Method = method; // POST 提交
            request.ContentType = contentType; // 以上信息在监听请求的时候都有的直接复制过来
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Accept = "*/*";
            request.Headers.Add("Accept-Language", "zh-Hans-CN,zh-Hans;q=0.7,ja;q=0.3");
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
            request.KeepAlive = true;
            request.Headers.Add("Access-Control-Allow-Origin", "*"); // 可跨域
            request.Headers.Add("Pragma", "no-cache");
            request.Headers.Add("DNT", "1");
            if (heards != null && heards.Length > 0)
            {
                foreach (var val in heards)
                {
                    request.Headers.Add(val.Key, val.Value);
                }
            }

            request.ServicePoint.Expect100Continue = false;

            byte[] bt = Encoding.UTF8.GetBytes(strParams);
            string responseData = string.Empty;
            request.ContentLength = bt.Length;


            if (bt != null && bt.Length != 0)
            {
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bt, 0, bt.Length);
                    reqStream.Close();
                }
            }

            // 获得服务端响应
            string retString = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream stream = response.GetResponseStream();
                using (StreamReader streamReader = new StreamReader(stream, encoding))
                {
                    retString = streamReader.ReadToEnd().ToString();
                }
            }

            return retString;
        }

        /// <summary>
        /// Post方法以文件流方式上传文件
        /// </summary>
        /// <param name="strUrl">一般应用程序请求地址</param>
        /// <param name="inputStream">文件流</param>
        public static void HttpPostStremFile(string strUrl, Stream inputStream)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
            request.Method = "POST"; // POST 提交
            request.ContentType = "application/x-www-form-urlencoded"; // 以上信息在监听请求的时候都有的直接复制过来
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Accept = "*/*";
            request.Headers.Add("Accept-Language", "zh-Hans-CN,zh-Hans;q=0.7,ja;q=0.3");
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
            request.KeepAlive = true;
            request.Headers.Add("Access-Control-Allow-Origin", "*"); // 可跨域
            request.Headers.Add("Pragma", "no-cache");
            request.Headers.Add("DNT", "1");
            request.ServicePoint.Expect100Continue = false;

            byte[] bt = new byte[inputStream.Length];

            inputStream.Read(bt, 0, (int)inputStream.Length);

            request.ContentLength = bt.Length;

            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(bt, 0, bt.Length);
                reqStream.Close();
            }
        }
    }
}
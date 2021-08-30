#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：NPlatform.Infrastructure
* 项目描述 ：
* 类 名 称 ：HttpClientHelper
* 类 描 述 ：
* 所在的域 ：DONGJIANHU
* 命名空间 ：NPlatform.Infrastructure
* 机器名称 ：DONGJIANHU 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：MACHENIKE
* 创建时间 ：2019-8-5 16:40:21
* 更新时间 ：2019-8-5 16:40:21
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ MACHENIKE 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NPlatform.Infrastructure
{
    /// <summary>
    /// http client 工具类型
    /// </summary>
    public static class HttpClientHelper
    {
        /// <summary>
        /// post 文件
        /// </summary>
        /// <param name="url">文件地址</param>
        /// <param name="fileName">文件名</param>
        /// <param name="fileByteArray">文件的字节数组</param>
        /// <param name="formParams">表单参数</param>
        /// <returns></returns>
        public static string PostFile(string url, string fileName, byte[] fileByteArray, params KeyValuePair<string, string>[] formParams)
        {

            using (var client = new HttpClient())
            {
                using (var multipartFormDataContent = new MultipartFormDataContent())
                {
                    foreach (var keyValuePair in formParams)
                    {
                        multipartFormDataContent.Add(new StringContent(keyValuePair.Value),
                            String.Format("\"{0}\"", keyValuePair.Key));
                    }

                    multipartFormDataContent.Add(new ByteArrayContent(fileByteArray), "\"FormFile\"", $"\"{fileName}\"");
                    var html = client.PostAsync(url, multipartFormDataContent).Result.Content.ReadAsStringAsync().Result;
                    return html;
                }
            }
        }
        /// <summary>
        /// post 文件
        /// </summary>
        /// <param name="url">文件地址</param>
        /// <param name="fileName">文件名</param>
        /// <param name="fileByteArray">文件的字节数组</param>
        /// <param name="formParams">表单参数</param>
        /// <returns></returns>
        public static string PostFile(string url, string fileName, byte[] fileByteArray, KeyValuePair<string, string>[] formParams, KeyValuePair<string, string>[] heards)
        {

            using (var client = new HttpClient())
            {
                using (var multipartFormDataContent = new MultipartFormDataContent())
                {
                    foreach (var keyValuePair in formParams)
                    {
                        multipartFormDataContent.Add(new StringContent(keyValuePair.Value),
                            String.Format("\"{0}\"", keyValuePair.Key));
                    }

                    multipartFormDataContent.Add(new ByteArrayContent(fileByteArray), "\"FormFile\"", $"\"{fileName}\"");
                    if(heards!=null)
                    {
                        foreach(var kv in heards)
                        {
                            client.DefaultRequestHeaders.Add(kv.Key, kv.Value);
                        }
                    }
                    var html = client.PostAsync(url, multipartFormDataContent).Result.Content.ReadAsStringAsync().Result;
                    return html;
                }
            }
        }
        /// <summary>
        /// 发起GET同步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string HttpGet(string url, Dictionary<string, string> headers = null)
        {
            using (HttpClient client = new HttpClient())
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
                HttpResponseMessage response = client.GetAsync(url).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        /// <summary>
        /// 发起GET异步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static async Task<string> HttpGetAsync(string url, Dictionary<string, string> headers = null)
        {
            using (HttpClient client = new HttpClient())
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
                HttpResponseMessage response = await client.GetAsync(url);
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Jiguang.JPush;
using Jiguang.JPush.Model;
using NPlatform.Config;
using Message = ServiceStack.Messaging.Message;
using System.Net;
using System.IO;

namespace NPlatform.Infrastructure.Push
{

    /// <summary>
    /// 发布消息
    /// </summary>
   public  class PushHelper
    {
        /// <summary>
        /// 全局配置信息
        /// </summary>
        private static NPlatformConfig _config = new ConfigFactory<NPlatform.Config.NPlatformConfig>().Build();
        ///  private static JPushClient client = new JPushClient("8a01a3705d9dbdc5ba963488", "f6e046ded25bcd736a07b40b");
        /// <summary>
        /// 全局配置信息
        /// </summary>
        public static NPlatformConfig Config
        {
            get { return _config; }

        }


        /// <summary>
        /// 极光推送
        /// </summary>
        /// <param name="title">消息标题</param>
        /// <param name="alert">消息内容</param>
        /// <param name="registrationIds">推送用户的注册id</param>
        public void SendMessage(string  title,string  alert,List<string> registrationIds)
        {
            try
            {
                List<string> registration_id = new List<string>();
                var AppKey = Config.UserConfigs["JGAppKey"];
                var AppSecret = Config.UserConfigs["JGAppSecret"];
                JPushClient client = new JPushClient(AppKey, AppSecret);
                PushPayload pushPayload = new PushPayload();
                pushPayload.Platform = new List<string> { "android", "ios" };
                //   Audience = "all",
                // Audience = "{" + "registration_id" + ":" + Newtonsoft.Json.JsonConvert.SerializeObject(registration_id) + "}",
                //"registration_id":[
                //"160a3797c8737975686"
                //]
                pushPayload.Notification = new Notification
                {

                    Alert = "hello jpush",
                    Android = new Android
                    {
                        Alert = alert,
                        Title = title,

                    },
                    IOS = new IOS
                    {
                        Alert = "ios alert",
                        Badge = "+1"
                    }
                };
                pushPayload.Message = new Jiguang.JPush.Model.Message
                {
                    Title = "message title",
                    Content = "message content",
                    Extras = new Dictionary<string, string>
                    {
                        ["key1"] = "value1"
                    }
                };
                pushPayload.Options = new Jiguang.JPush.Model.Options
                {
                    IsApnsProduction = true // 设置 iOS 推送生产环境。不设置默认为开发环境。
                };

                Audience audience = new Audience();
                // 3. 设置RegistrationId发送
                audience.RegistrationId = registrationIds;
                pushPayload.Audience = audience;

                var response = client.SendPush(pushPayload);
            }
            catch { }
        }

        /// <summary>
        /// 短信推送
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content"></param>
        public static string SendSmsMessage(string mobile, string content)
        {
            try
            {
                //Dictionary<string, string> headers = new Dictionary<string, string>();
                //headers.Add("Client_Id", "08d67a0e-522f-50c5-045b-9d4638000002");
                //string url = string.Format("http://39.106.163.216:9999/api/SendMessage/sendMsg?mobiles={0}&content={1}",
                //    mobile, content);
                //var result = HttpClientHelper.HttpGetAsync(url, headers);

                string SendState = "";
                string isSendSuccess = GetHttpData("http://utf8.sms.webchinese.cn/?Uid=ZJJW&Key=coq210rf0f3frklqsjwd&smsMob=" + mobile.ToString().Trim() + "&smsText=【" + Config.UserConfigs["ProjectName"] + "】" + content);
                if (int.Parse(isSendSuccess) < 0)
                {
                    switch (isSendSuccess)
                    {
                        case "-99":
                        case "-100":
                            SendState = "因网络原因的短信通知没有发送成功！请重新发送通知给这些用户！";
                            break;
                        case "-1":
                            SendState += " 没有该用户账户";
                            break;
                        case "-2":
                            SendState += "短信发送密钥不正确，请与管理员联系！";
                            break;
                        case "-3":
                            SendState += "发送短信内容为空！";
                            break;
                        case "-11":
                            SendState += " 该用户被禁用！";
                            break;
                        case "-14":
                            SendState += " 短信内容出现非法字符！";
                            break;
                        case "-4":
                            SendState += " 手机号格式不正确！";
                            break;
                        case "-41":
                            SendState += " 手机号码为空！";
                            break;
                        case "-42":
                            SendState += " 短信内容为空！";
                            break;
                        case "-51":
                            SendState += " 短信签名格式不正确！请与管理员联系！";
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    SendState += " 发送成功！";
                }
                return SendState;
            }
            catch (Exception ex)
            {
                Loger.LogerHelper.Error(ex.Message, "SendSmsMessage");
                return null;
            }
        }

        /// <summary>
        /// 获取发送结果
        /// </summary>
        /// <param name="url">地址</param>
        /// <returns></returns>
        public static string GetHttpData(string url)
        {
            string sException = null;
            string sRslt = null;
            WebResponse oWebRps = null;
            WebRequest oWebRqst = WebRequest.Create(url);
            oWebRqst.Timeout = 30000;
            try
            {
                oWebRps = oWebRqst.GetResponse();
            }
            catch (WebException e)
            {
                sException = e.Message.ToString();
                sRslt = "-99";
            }
            catch (Exception ex)
            {
                sException = ex.ToString();
                sRslt = "-100";
            }
            finally
            {
                if (oWebRps != null)
                {
                    StreamReader oStreamRd = new StreamReader(oWebRps.GetResponseStream(), Encoding.Default);
                    sRslt = oStreamRd.ReadToEnd();
                    oStreamRd.Close();
                    oWebRps.Close();
                }
            }
            return sRslt;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using NPlatform.Result;
using NPlatform.Config;
using Microsoft.AspNetCore.Authorization;
using NPlatform.Infrastructure.Redis;
using System.Web;
using NPlatform.Infrastructure.Loger;

namespace NPlatform.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Primitives;
    using ServiceStack;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using NPlatform.Infrastructure;
    using NPlatform.IOC;
    using NPlatform.Repositories;

    /// <summary>
    /// controler 基类
    /// </summary>
    // [AllowAnonymous]
    [ApiController]
    [Authorize]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// 全局配置信息
        /// </summary>
        private static NPlatformConfig _Config = new ConfigFactory<NPlatform.Config.NPlatformConfig>().Build();

        /// <summary>
        /// 全局配置信息
        /// </summary>
        public static NPlatformConfig Config
        {
            get { return _Config; }
            
        }

        /// <summary>
        /// 获取UI传递的js 数组参数 'Content-Type':'application/json' params:JSON.stringify(Array)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetRequestArrayParams<T>()
        {
            using (var ms = new MemoryStream())
            {
                Request.Body.CopyTo(ms);
                var b = ms.ToArray();
                var postParamsString = Encoding.UTF8.GetString(b);
                return NPlatform.Infrastructure.SerializerHelper.FromJson<T>(postParamsString);
            }
        }

        /// <summary>
        /// 获取UI传递的js 数组参数 'Content-Type':'application/json' params:JSON.stringify(Array)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected virtual string GetRequestStringParams()
        {
            using (var ms = new MemoryStream())
            {
                Request.Body.CopyTo(ms);
                var b = ms.ToArray();
                var postParamsString = Encoding.UTF8.GetString(b);
                return postParamsString;
            }
        }


        private AuthInfoVO authInfo = null;

        /// <summary>
        /// 获取认证的身份信息
        /// </summary>
        protected virtual AuthInfoVO AuthInfo
        {
            get
            {
                try
                {
                    if (this.authInfo == null)
                    {
                        RedisHelper redis = new RedisHelper();
                        authInfo = redis.StringGet<AuthInfoVO>(CommonRedisConst.GetOtherPhoneKey(AuthInfo.AccessToken));
                        if (authInfo == null)
                        {
                            var Claims = User.Claims;
                            authInfo = new AuthInfoVO();
                            authInfo.AccessToken = this.Request.Headers["Authorization"];
                            if (Claims.Any(t => t.Type == "id"))
                            {
                                authInfo.Id = Claims.FirstOrDefault(t => t.Type == "id").Value;
                            }

                            if (Claims.Any(t => t.Type == "client_id"))
                            {
                                authInfo.ClientId = Claims.FirstOrDefault(t => t.Type == "client_id").Value;
                            }

                            if (Claims.Any(t => t.Type == "roleid"))
                            {
                                authInfo.CurrentRoleId = Claims.FirstOrDefault(t => t.Type == "roleid").Value;
                            }
                            if (Claims.Any(t => t.Type.Contains("givenname")))
                            {
                                // Claims.FirstOrDefault(t => t.Subject.Name);
                                authInfo.CnName = Claims.FirstOrDefault(t => t.Type.Contains("givenname")).Value;
                            }
                            if (Claims.Any(t => t.Type == "picture"))
                            {
                                authInfo.SignPic = Claims.FirstOrDefault(t => t.Type == "picture").Value;
                            }

                            if (Claims.Any(t => t.Type == "name"))
                            {
                                authInfo.Account = Claims.FirstOrDefault(t => t.Type == "name").Value;
                            }
                        }
                        return authInfo;
                    }
                    else
                    {
                        return authInfo;
                    }
                }
                catch (Exception ex)
                {
                    LogerHelper.Error(ex.Message, "SYS", ex);
                    return new AuthInfoVO();
                }
            }
        }

        /// <summary>
        ///  返回SuccessResult
        /// </summary>
        protected virtual EPResult Success(string msg)
        {
            return new EPResult(msg);
        }
        /// <summary>
        ///  返回SuccessResult
        /// </summary>
        protected virtual EPResult Success()
        {
            return new EPResult(string.Empty);
        }

        /// <summary>
        ///  返回SuccessResult
        /// </summary>
        protected virtual EPResult<T> DTOResult<T>(T obj)
        {
            return new EPResult<T>("", obj);
        }

        /// <summary>
        ///  返回SuccessResult<T/>
        /// </summary>
        protected virtual EPResult<T> DTOResult<T>(string msg, T obj) 
        {
            return new EPResult<T>(msg, obj);
        }

        /// <summary>
        /// 返回错误信息
        /// </summary>
        protected virtual IEPResult  Error(string msg)
        {
            var rst= new EPResult(msg);
            rst.Success = false;
            return rst;
        }

        /// <summary>
        /// 返回错误信息
        /// </summary>
        protected virtual IEPResult Error(NPlatformException ex)
        {
            return  Error<IDTO>(ex.Message,ex,"");
        }
        /// <summary>
        /// 返回错误信息
        /// </summary>
        protected virtual ErrorResult<T> Error<T>(string msg)
        {
            return new ErrorResult<T>(msg);
        }
        /// <summary>
        /// 返回错误信息
        /// </summary>
        protected virtual ErrorResult<T> Error<T>(NPlatformException ex) 
        {
            return Error<T>(ex.Message, ex, "");
        }
        /// <summary>
        /// 返回错误信息
        /// </summary>
        protected virtual ErrorResult<T> Error<T>(string msg, NPlatform.NPlatformException ex, string modelName)
        {
            if (ex.GetType().IsSubclassOf(typeof(LogicException)))
            {
                LogerHelper.Error(ex.Message, modelName, ex);
                return Error<T>(ex.Message);
            }
            else if (ex.GetType().IsSubclassOf(typeof(ConfigException)))
            {
                LogerHelper.Error("系统配置加载异常!", modelName, ex);
                return Error<T>("系统配置加载异常");
            }
            else
            {
                LogerHelper.Error(ex.Message,  modelName, ex);
                return Error<T>(ex.Message);
            }
        }
        /// <summary>
        /// 树格式节点
        /// </summary>
        /// <typeparam name="T">TreeNode 类型</typeparam>
        /// <param name="nodes">树节点</param>
        /// <returns></returns>
        protected TreeResult<T> TreeData<T>(IEnumerable<T> nodes) where T: class
        {
            var trees = new TreeResult<T>();
            trees.AddRange(nodes);
            return trees;
        }

        /// <summary>
        /// 返回数据集合
        /// </summary>
        protected virtual ListResult<T> PageData<T>(IEnumerable<T> list, long total)
        {
            var content = new ListResult<T>(list, total);
            return content;
        }

        /// <summary>
        /// 返回数据集合
        /// </summary>
        protected virtual ListResult<T> ListData<T>(IEnumerable<T> list, long total)
        {
            var content = new ListResult<T>(list, total);
            return content;
        }
        /// <summary>
        /// 返回数据集合
        /// </summary>
        protected virtual ListResult<T> ListData<T>(IEnumerable<T> list)
        {
            var content = new ListResult<T>(list, -1);
            return content;
        }
        /// <summary>
        ///  返回SuccessResult
        /// </summary>
        protected virtual EPResult StrData(string msg)
        {
            return new EPResult(msg);
        }

        /// <summary>
        /// 生成一个ID，基于平台的默认算法
        /// </summary>
        /// <returns>ID</returns>
        protected string IDGenerate()
        {
            return new NPlatform.Infrastructure.IdGenerators.IdGenerator().GenerateId().ToString();
        }
    }
}
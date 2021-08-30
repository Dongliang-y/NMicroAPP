using Castle.DynamicProxy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NPlatform.Domains.Service;
using NPlatform.Infrastructure;

namespace NPlatform.Filters
{

    /// <summary>
    /// 取得缓存配置
    /// </summary>
    public delegate CacheSetting[] ResetRedisSettingDelegate();

    /// <summary>
    /// Redis 缓存拦截，当缓存中存在数据时，则不进行方法操作，直接返回缓存内容 
    /// </summary>
    public class RedisCacheInterceptor : IInterceptor
    {

        /// <summary>
        /// redis缓存设置在 缓存中的键值
        /// </summary>

        const string RedisCacheSettingKey = "Redis_Cache_Setting";

        /// <summary>
        /// 初始化redisHelper
        /// </summary>
        RedisHelper redisHelper;

        /// <summary>
        /// 是否开启缓存方法拦截
        /// </summary>
        bool enableCacheInterceptor;

        /// <summary>
        /// 是否正在加载
        /// </summary>
        bool isLoading = false;
        /// <summary>
        /// Redis 缓存拦截，当缓存中存在数据时，则不进行方法操作，直接返回缓存内容 
        /// </summary>
        /// <param name="redisHelper"></param>
        /// <param name="enableCacheInterceptor">是否开始缓存拦截</param>
        public RedisCacheInterceptor(RedisHelper redisHelper, bool enableCacheInterceptor)
        {
            this.redisHelper = redisHelper;
            this.enableCacheInterceptor = enableCacheInterceptor;

            GetRedisSettingDelegate = GetCacheSetting;
        }



        /// <summary>
        /// 取得缓存的委托
        /// </summary>
        public ResetRedisSettingDelegate GetRedisSettingDelegate;

        /// <summary>
        /// 取得缓存配置项
        /// </summary>
        /// <returns></returns>
        public CacheSetting[] GetCacheSetting()
        {
            if (!NPlatformStartup.AutoMapperInitialized)
            {
                return null;
            }
            List<CacheSetting> list = new List<CacheSetting>();
            #region 全量取得KEY和关系
            var cacheKeyService = new CacheService(); //NPlatform.IOC.IOCManager.BuildService<ICacheKeyOpenService>();


            var cacheKeyDTOs = cacheKeyService.FullGetCacheKey();

            var relationsDTOs = cacheKeyService.FullGetCacheKeyRelations();

            #endregion

            #region 对关系进行分割，形成相应的结果，并返回
            foreach (var key in cacheKeyDTOs.Where(m => !m.IsDelete))
            {


                list.Add(new CacheSetting()
                {
                    AffectRelations = relationsDTOs.Where(m => m.Relations.Split(',').Contains("1") && m.RelationKey == key.MethodFullName).Select(m => new CacheRelation()
                    {
                        Key = m.RelationKey,
                        //Value = m.Value
                    }).ToArray(),
                    DependedRelations = relationsDTOs.Where(m => m.Relations.Split(',').Contains("1") && m.Key == key.MethodFullName).Select(m => new CacheRelation()
                    {
                        Key = m.RelationKey,
                        //Value = m.Value
                    }).ToArray(),
                    Interval = key.Interval ?? 0,
                    TransformationMethod = key.TransformationMethod,
                    ReturnType = key.ReturnType,
                    //Key = key.MethodFullName,
                    //Id = key.Id,
                    MethodFullName = key.MethodFullName
                });
            }
            #endregion
            return list.ToArray();
            //return new CacheSetting[]
        }




        /// <summary>
        /// 缓存设置信息
        /// </summary>
        static CacheSetting[] cacheSettings = null;

        /// <summary>
        /// 程序 集
        /// </summary>
        private Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();




        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {

            if (!enableCacheInterceptor)
            {
                invocation.Proceed();
                return;
            }

            if (!redisHelper.KeyExists(RedisCacheSettingKey) && !isLoading)
            {
                ReadCacheSettings();
            }

            if (redisHelper.KeyExists(RedisCacheSettingKey) && RedisCacheInterceptor.cacheSettings == null)
            {
                RedisCacheInterceptor.cacheSettings = redisHelper.StringGet<CacheSetting[]>(RedisCacheSettingKey);
            }

            if (RedisCacheInterceptor.cacheSettings == null)
            {
                invocation.Proceed();
                return;
            }

            // 将数组COPY一份，防止其他程序更新
            var setting = new CacheSetting[RedisCacheInterceptor.cacheSettings.Length];
            Array.Copy(RedisCacheInterceptor.cacheSettings, setting, setting.Length);

            var methodFullName = invocation.Method.DeclaringType.FullName +
                        "." + invocation.Method.Name
                        ;

            //var returnType = invocation.Method.ReturnType;

            // 不存在方法的配置时，直接执行。不进行任何处理
            if (!setting.Any(m => m.MethodFullName == methodFullName))
            {
                invocation.Proceed();
                return;
            }
            var method = setting.First(m => m.MethodFullName == methodFullName);

            #region 将对象转化为JObject,并取得关键值
            string cacheKey = method.MethodFullName;

            var affectRelations = method.AffectRelations;
            // 用于存放参数对象的变量 
            //JObject parsJObject = GetParsJObject(method, invocation);

            if (invocation.Arguments != null && invocation.Arguments.Length != 0)
            {
                cacheKey = cacheKey + ":" + MD5S.GetSHA256(Newtonsoft.Json.JsonConvert.SerializeObject(invocation.Arguments));
            }
            //cacheKey = cacheKey + ":" +  MD5S.GetSHA256(Newtonsoft.Json.JsonConvert.SerializeObject(  parsJObject?.GetHashCode(); 
            //TransformKeyByPars(cacheKey, parsJObject);
            // 通过路径取得关键值
            //var pars1 = parsJObject.SelectToken("CustomChildCls.pars")?.Value<string>();
            #endregion

            #region 判断缓存中是否有值，有值时直接返回
            // 如果存在本方法的缓存，并且缓存中可以取到值。
            // 那么，就直接返回信息

            if (redisHelper.KeyExists(cacheKey))
            {
                try
                {
                    var returnType = invocation.MethodInvocationTarget.ReturnType;
                    if (!returnType.IsInterface && string.IsNullOrEmpty(method.TransformationMethod))
                    {
                        // 如果是具体实现类，则直接可以取
                        invocation.ReturnValue = redisHelper.StringGet(cacheKey, returnType);
                        return;
                    }
                    else if (!string.IsNullOrEmpty(method.ReturnType))
                    {
                        // Scanfer  怎么取到自己要实现的类型？使用以下方法
                        // (new System.Collections.Generic.List< WebTest.DTO.ReturnService>()).GetType()
                        // 示例："System.Collections.Generic.List`1[[WebTest.DTO.ReturnService, WebTest.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]"
                        // 示例： "WebTest.DTO.ReturnService[], WebTest.DTO" 或者 "WebTest.DTO.ReturnService[], WebTest.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                        var json = redisHelper.StringGet(cacheKey);
                        //var data = Newtonsoft.Json.JsonConvert.DeserializeObject(json, Type.GetType(method.ReturnType));
                        var data = Newtonsoft.Json.JsonConvert.DeserializeObject(json, Type.GetType(method.ReturnType), new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        });
                        invocation.ReturnValue = data;
                        return;

                    }
                    else if (!string.IsNullOrEmpty(method.TransformationMethod))
                    {
                        // 如果有转换方法，通过转换方法取得对象
                        var transEntity = method.TransformationMethod;
                        var transPars = transEntity.Split(';');
                        var assemblyName = transPars[1];
                        var methodPars = transPars[0];
                        var className = methodPars.Substring(0, methodPars.LastIndexOf("."));
                        var methodName = methodPars.Substring(methodPars.LastIndexOf(".") + 1);

                        Assembly assembly;

                        if (!assemblies.ContainsKey(assemblyName))
                        {
                            assembly = Assembly.Load(assemblyName);
                            if (assembly != null)
                            {
                                assemblies.Add(assemblyName, assembly);
                            }
                        }
                        if (assemblies.ContainsKey(assemblyName))
                        {
                            assembly = assemblies[assemblyName];
                            var p = assembly.GetType(className);
                            var m = p.GetMethod(methodName);
                            var json = redisHelper.StringGet(cacheKey);
                            var data = m.Invoke(p, new object[] { json });
                            //var o = assembly.CreateInstance("System.Collections.Generic.List<ZJJW.WEBTest.Domain.Service.ReturnService>");
                            //var t = o.GetType();

                            //var returnVal = Newtonsoft.Json.JsonConvert.DeserializeObject(json, t);
                            //var s = ass.CreateInstance(method.ReturnType);
                            invocation.ReturnValue = data;

                            return;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            #endregion


            // 没有值的情况下，对原有方法进行执行处理
            invocation.Proceed();



            #region 将关系键值组合起来
            // 不再进行精细化管理，因为无论是配置还是使用都相当的累。
            //if (affectRelations != null && affectRelations.Length != 0)
            //{
            //    foreach (var rel in affectRelations)
            //    {
            //        rel.Value = TransformKeyByPars(rel.Value, parsJObject);
            //        #region 将关系表中的键值对组合起来
            //        //if (!string.IsNullOrEmpty(rel.Value))
            //        //{
            //        //    var matchs = regex.Matches(rel.Value);
            //        //    foreach (Match match in matchs)
            //        //    {
            //        //        rel.Value = rel.Value.Replace(match.Value, parsJObject.SelectToken(match.Groups["val"].Value)?.Value<string>());
            //        //    }
            //        //}
            //        #endregion
            //    }
            //}
            #endregion

            #region 如果其他的KEY有包括关系键，则应当清除
            // 影响项，即当我发生变更时，清除所有相关的缓存
            if (affectRelations != null && affectRelations.Length != 0)
            {
                foreach (var rel in affectRelations)
                {
                    // 找到集合中包含的所有缓存KEY
                    var removeKeys = redisHelper.SetMembers(rel.ComputedValue).Select(m => m.ToString()).ToList();
                    if (removeKeys != null && removeKeys.Count() != 0)
                    {
                        // 删除键
                        redisHelper.KeyDelete(removeKeys);
                        //从当前关系列表中删除
                        redisHelper.SetRemove(rel.ComputedValue, removeKeys.ToArray());
                    }

                }
            }
            #endregion

            #region 如果当前方法需要缓存，则根据规则写入缓存

            if (!string.IsNullOrEmpty(cacheKey))
            {
                Task.Run(() =>
                {
                    var result = invocation.ReturnValue;
                    if (method.Interval > 0 && result != null)
                    {
                        redisHelper.StringSetWithType<object>(cacheKey, result, TimeSpan.FromSeconds(method.Interval));
                    }
                    var dependedRelations = method.DependedRelations;
                    #region 将方法的外部关系 建立起来
                    // 将方法的关系加入到关系SET中
                    // 依赖项， 新建一个缓存的属性表，用于记录方法的依赖项。
                    // 当其他方法被调用时，则需要清空本缓存实例
                    if (dependedRelations != null && dependedRelations.Length != 0)
                    {
                        foreach (var rel in dependedRelations)
                        {
                            if (method.Interval > 0)
                            {
                                //rel.Value = TransformKeyByPars(rel.Value, parsJObject);
                                redisHelper.SetAdd(rel.ComputedValue, cacheKey);
                                // 设置过期时间
                                redisHelper.KeyExpire(rel.ComputedValue, TimeSpan.FromSeconds(method.Interval));
                            }
                        }
                    }
                });
                #endregion
            }
            #endregion

        }




        /// <summary>
        /// 通过参数转换Key
        /// </summary>
        /// <returns></returns>
        public string TransformKeyByPars(string key, JObject parsJObject)
        {
            Regex regex = new Regex("{(?<val>.+?)}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            if (!string.IsNullOrEmpty(key))
            {
                var matchs = regex.Matches(key);
                foreach (Match match in matchs)
                {
                    key = key.Replace(match.Value, parsJObject.SelectToken(match.Groups["val"].Value)?.Value<string>());
                }
            }
            return key;
        }


        /// <summary>
        /// 从缓存配置中读取信息
        /// </summary>
        public void ReadCacheSettings()
        {
            isLoading = true;
            if (redisHelper.KeyExists(RedisCacheSettingKey))
            {
                cacheSettings = redisHelper.StringGet<CacheSetting[]>(RedisCacheSettingKey);
                return;
            }
            if (GetRedisSettingDelegate != null)
            {
                cacheSettings = GetRedisSettingDelegate();
            }

            //cacheSettings = new CacheSetting[] {
            //    // 添加方法
            //     new CacheSetting(){

            //          Id = Guid.NewGuid().ToString(),
            //           Interval = 360,
            //            Key = "GetCustomCls:{CustomChildCls.pars}",
            //             MethodFullName = "WebAPP.Repository.ITestRepository.AddCustomCls",
            //              AffectRelations = new CacheRelation[]{
            //                 new CacheRelation(){
            //                     Key = "pars1",
            //                      Value = "{CustomChildCls.pars}"
            //                 },
            //                  new CacheRelation(){
            //                    Key = "ClsListGrid"
            //                  }
            //              },
            //              DependedRelations = new CacheRelation[]{
            //                 new CacheRelation(){
            //                     Key = "pars1",
            //                      Value = "{CustomChildCls.pars}"
            //                 }
            //              }
            //     },
            //     // 清空列表方法
            //     new CacheSetting(){

            //          Id = Guid.NewGuid().ToString(),
            //           Interval = 360,
            //            Key ="GetClsList",
            //             MethodFullName = "WebAPP.Repository.ITestRepository.GetClsList",
            //              AffectRelations = new CacheRelation[]{
            //                  new CacheRelation(){
            //                    Key = "ClsListGrid"
            //                  }
            //              },
            //              DependedRelations = new CacheRelation[]{
            //                 new CacheRelation(){
            //                    Key = "ClsListGrid"
            //                  }
            //              }
            //     }
            //};
            // 将缓存信息存入到redis中去
            // 默认设置他的有效时间为5分钟更新一次
            if (cacheSettings != null && cacheSettings.Length != 0)
            {
                redisHelper.StringSet<CacheSetting[]>(RedisCacheSettingKey, cacheSettings, TimeSpan.FromSeconds(600));
            }
            isLoading = false;
        }
    }

    /// <summary>
    /// 缓存设置信息
    /// </summary>
    public class CacheSetting
    {

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Id { set; get; }

        /// <summary>
        /// 方法全量名称 
        /// 格式：命名空间.类名.方法名
        /// 示例：NPlatform.Repositories.Sys.ClientRepository
        /// </summary>
        public string MethodFullName { set; get; }

        /// <summary>
        /// 缓存键值
        /// 如果需要加入参数，使用{0}{1}
        /// 在动态参数中一定要按相同顺序排列
        /// </summary>
        //public string Key { set; get; }

        /// <summary>
        /// 动态参数  ,分隔数组
        /// 通过解析传入的参数对Key进行组装。使用,分隔多个值
        /// 格式：参数名.父级对象.子级对象.子级属性，如果是值，只输入参数名就好
        /// 示例:pars1.classname.property,pars2
        /// </summary>
        //public string DynamicPars { set; get; }

        /// <summary>
        /// 缓存间隔 单位：秒
        /// </summary>
        public int Interval { set; get; }

        /// <summary>
        /// 如果返回实体类型处理不了的，使用转换方法。高级用法
        /// </summary>
        public string TransformationMethod { set; get; }

        /// <summary>
        /// 返回实体类型
        /// </summary>
        public string ReturnType { set; get; }



        /// <summary>
        /// 影响 缓存关系 
        /// 如果方法执行时，需要更新的方法对象
        /// </summary>
        public CacheRelation[] AffectRelations { set; get; }

        /// <summary>
        /// 依赖关系 
        /// 如果其他方法更新时，需要清除的依赖关系 
        /// </summary>
        public CacheRelation[] DependedRelations { set; get; }

    }

    /// <summary>
    /// 缓存关系 
    /// 当有缓存关系时，会在清空时自动清空相关数据
    /// </summary>
    public class CacheRelation
    {
        /// <summary>
        /// 关键键值
        /// </summary>
        public string Key { set; get; }

        /// <summary>
        /// 计算出来的值 
        /// </summary>
        //public string Value { set; get; }

        /// <summary>
        /// 计算出来的值,将会自动带上REL:
        /// </summary>
        public string ComputedValue
        {
            get
            {
                return "REL:" + Key;
            }
        }

    }
}

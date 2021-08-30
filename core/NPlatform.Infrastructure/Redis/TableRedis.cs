#region << 版 本 注 释 >>

/*----------------------------------------------------------------
* 项目名称 ：NPlatform.Infrastructure.Redis
* 类 名 称 ：TabRedisEngine
* 类 描 述 ：表Redis公共辅助类库
* 命名空间 ：NPlatform.Infrastructure.Redis
* CLR 版本 ：4.0.30319.42000
* 作    者 ：YanZhou
* 创建时间 ：2018-12-21 16:36:58
* 更新时间 ：2018-12-21 16:36:58
* 版 本 号 ：v1.0.0.0
//----------------------------------------------------------------*/

#endregion

namespace NPlatform.Infrastructure.Redis
{
    using NPlatform.Config;

    /// <summary>
    /// 表Redis缓存
    /// </summary>
    public class TableRedis
    {
        /// <summary>
        /// 表Redis单例
        /// </summary>
        public static TableRedis inst;

        private static RedisHelper redis;

        private TableRedis()
        {
            redis = new RedisHelper(12);
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>对象</returns>
        public static TableRedis Instance
        {
            get
            {
                // lock (engineLock)
                // {
                // if (engine == null)
                // {
                inst = new TableRedis();

                // }
                // else
                // {
                // return engine;
                // }
                return inst;

                // }
            }
        }

        /// <summary>
        /// 将缓存对象以json形式存储
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="module">模块类型</param>
        /// <param name="t">值对象</param>
        public void Add<T>(string module, T t)
            where T : class
        {
            redis.StringSet<T>(module, t);
        }

        /// <summary>
        /// 表是否有缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>bool</returns>
        public bool Exists(string key)
        {
            return redis.KeyExists(key);
        }

        /// <summary>
        /// 获取泛型对象信息
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="module">模块类型</param>
        /// <returns>对象</returns>
        public T Get<T>(string module)
            where T : class
        {
            return redis.StringGet<T>(module);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="module">模块类型</param>
        public void Remove(string module)
        {
            redis.KeyDelete(module);
        }
    }
}
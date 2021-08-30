using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NPlatform.Config;

namespace NPlatform.Infrastructure
{
    /// <summary>
    /// Redis操作
    /// </summary>
    public class RedisHelper
    {
        private int DbNum { get; }
        private readonly ConnectionMultiplexer _conn;
        private NPlatformConfig config = new ConfigFactory<NPlatformConfig>().Build();
        private string prefix;

        #region 构造函数

        /// <summary>
        /// RedisHelper
        /// </summary>
        /// <param name="dbNum">库序号</param>
        public RedisHelper(int dbNum = 0)
        {
            DbNum = dbNum;
            _conn = RedisConnection.Instance;
            prefix = config.MachineID + config.ServiceID.ToString();
        }

        #endregion 构造函数

        #region String

        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            key = SetPrefix(key);
            return await Do(db => db.StringSetAsync(key, value, expiry));
        }
        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool StringSet(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            key = SetPrefix(key);
            return Do(db => db.StringSet(key, value, expiry));
        }

        /// <summary>
        /// 保存多个key value
        /// </summary>
        /// <param name="keyValues">键值对</param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync(List<KeyValuePair<RedisKey, RedisValue>> keyValues)
        {
            List<KeyValuePair<RedisKey, RedisValue>> newkeyValues =
                keyValues.Select(p => new KeyValuePair<RedisKey, RedisValue>(SetPrefix(p.Key), p.Value)).ToList();
            return await Do(db => db.StringSetAsync(newkeyValues.ToArray()));
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?))
        {
            key = SetPrefix(key);
            string json = SerializerHelper.ToJson(obj);
            return await Do(db => db.StringSetAsync(key, json, expiry));
        }
        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool StringSet<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?))
        {
            key = SetPrefix(key);
            string json = SerializerHelper.ToJson(obj);
            return Do(db => db.StringSet(key, json, expiry));
        }

        /// <summary>
        /// 带类型的存储
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool StringSetWithType<T>(string key, T obj, TimeSpan expiry)
        {
            key = SetPrefix(key);
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new IPAddressConverter());
            settings.Converters.Add(new IPEndPointConverter());
            settings.Formatting = Formatting.Indented;
            settings.TypeNameHandling = TypeNameHandling.Auto;
            var json = JsonConvert.SerializeObject(obj, settings);

            return Do(db => db.StringSet(key, json, expiry));
        }

        public object StringGet(string key, Type type)
        {
            key = SetPrefix(key);
            string result = Do(db => db.StringGet(key));
            return result == null ? null : JsonConvert.DeserializeObject(result, type);
        }

        public string StringGet(string key)
        {
            key = SetPrefix(key);
            string result = Do(db => db.StringGet(key));
            return result;
        }

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        public async Task<string> StringGetAsync(string key)
        {
            key = SetPrefix(key);
            return await Do(db => db.StringGetAsync(key));
        }

        /// <summary>
        /// 获取多个Key
        /// </summary>
        /// <param name="listKey">Redis Key集合</param>
        /// <returns></returns>
        public async Task<RedisValue[]> StringGetAsync(List<string> listKey)
        {
            List<string> newKeys = listKey.Select(SetPrefix).ToList();
            return await Do(db => db.StringGetAsync(ConvertRedisKeys(newKeys)));
        }

        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> StringGetAsync<T>(string key)
        {
            key = SetPrefix(key);
            string result = await Do(db => db.StringGetAsync(key));
            return result == null ? default(T) : SerializerHelper.FromJson<T>(result);
        }
        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T StringGet<T>(string key)
        {
            key = SetPrefix(key);
            string result = Do(db => db.StringGet(key));
            return result == null ? default(T) : SerializerHelper.FromJson<T>(result);
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public async Task<double> StringIncrementAsync(string key, double val = 1)
        {
            key = SetPrefix(key);
            return await Do(db => db.StringIncrementAsync(key, val));
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public async Task<double> StringDecrementAsync(string key, double val = 1)
        {
            key = SetPrefix(key);
            return await Do(db => db.StringDecrementAsync(key, val));
        }


        #endregion String


        #region Sort 集合
        /// <summary>
        /// 给集合添加一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool SetAdd(string key, string obj)
        {
            key = this.SetPrefix(key);
            return this.Do(db => db.SetAdd(key, obj));
        }

        /// <summary>
        /// 取得集合下面所有的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public RedisValue[] SetMembers(string key)
        {
            key = this.SetPrefix(key);
            return this.Do(db => db.SetMembers(key));
        }

        /// <summary>
        /// 从集合中移除一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool SetRemove<T>(string key, T obj)
        {
            key = this.SetPrefix(key);
            return this.Do(db => db.SetRemove(key, Newtonsoft.Json.JsonConvert.SerializeObject(obj)));
        }

        /// <summary>
        /// 从集合中清除一系列的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public long SetRemove(string key, string[] obj)
        {
            key = this.SetPrefix(key);
            var keys = Array.ConvertAll<string, RedisValue>(obj, (p) => p);
            return this.Do(db => db.SetRemove(key, keys));
        }
        #endregion


        #region Hash


        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public async Task<bool> HashExistsAsync(string key, string dataKey)
        {
            key = SetPrefix(key);
            return await Do(db => db.HashExistsAsync(key, dataKey));
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<bool> HashSetAsync<T>(string key, string dataKey, T t)
        {
            key = SetPrefix(key);
            return await Do(db =>
            {
                string json = SerializerHelper.ToJson(t);
                return db.HashSetAsync(key, dataKey, json);
            });
        }

        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public async Task<bool> HashDeleteAsync(string key, string dataKey)
        {
            key = SetPrefix(key);
            return await Do(db => db.HashDeleteAsync(key, dataKey));
        }

        /// <summary>
        /// 移除hash中的多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKeys"></param>
        /// <returns></returns>
        public async Task<long> HashDeleteAsync(string key, List<RedisValue> dataKeys)
        {
            key = SetPrefix(key);
            //List<RedisValue> dataKeys1 = new List<RedisValue>() {"1","2"};
            return await Do(db => db.HashDeleteAsync(key, dataKeys.ToArray()));
        }

        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public async Task<T> HashGeAsync<T>(string key, string dataKey)
        {
            key = SetPrefix(key);
            string value = await Do(db => db.HashGetAsync(key, dataKey));
            return value == null ? default(T) : SerializerHelper.FromJson<T>(value);
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public async Task<double> HashIncrementAsync(string key, string dataKey, double val = 1)
        {
            key = SetPrefix(key);
            return await Do(db => db.HashIncrementAsync(key, dataKey, val));
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public async Task<double> HashDecrementAsync(string key, string dataKey, double val = 1)
        {
            key = SetPrefix(key);
            return await Do(db => db.HashDecrementAsync(key, dataKey, val));
        }

        /// <summary>
        /// 获取hashkey所有Redis key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> HashKeysAsync<T>(string key)
        {
            key = SetPrefix(key);
            RedisValue[] values = await Do(db => db.HashKeysAsync(key));
            return ConvetList<T>(values);
        }

        #endregion Hash

        #region List


        /// <summary>
        /// 移除指定ListId的内部List的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<long> ListRemoveAsync<T>(string key, T value)
        {
            key = SetPrefix(key);
            return await Do(db => db.ListRemoveAsync(key, SerializerHelper.ToJson(value)));
        }

        /// <summary>
        /// 获取指定key的List
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> ListRangeAsync<T>(string key)
        {
            key = SetPrefix(key);
            var values = await Do(redis => redis.ListRangeAsync(key));
            return ConvetList<T>(values);
        }

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<long> ListRightPushAsync<T>(string key, T value)
        {
            key = SetPrefix(key);
            return await Do(db => db.ListRightPushAsync(key, SerializerHelper.ToJson(value)));
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> ListRightPopAsync<T>(string key)
        {
            key = SetPrefix(key);
            var value = await Do(db => db.ListRightPopAsync(key));
            return !value.HasValue ? default(T) : SerializerHelper.FromJson<T>(value);
        }

        /// <summary>
        /// 入栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public long ListLeftPushs<T>(string key, List<T> values)
        {
            key = SetPrefix(key);
            return Do(db =>
            {
                var batch = db.CreateBatch();
                batch.KeyDeleteAsync(key);
                long listCount = 0;
                foreach (var value in values)
                {
                    batch.ListLeftPushAsync(key, SerializerHelper.ToJson(value));
                }
                batch.Execute();
                return 0;
            }
            );
        }

        /// <summary>
        /// 入栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<long> ListLeftPushAsync<T>(string key, T value)
        {
            key = SetPrefix(key);
            return await Do(db => db.ListLeftPushAsync(key, SerializerHelper.ToJson(value)));
        }

        /// <summary>
        /// 出栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> ListLeftPopAsync<T>(string key)
        {
            key = SetPrefix(key);
            var value = await Do(db => db.ListLeftPopAsync(key));
            return !value.HasValue ? default(T) : SerializerHelper.FromJson<T>(value);
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> ListLengthAsync(string key)
        {
            key = SetPrefix(key);
            return await Do(redis => redis.ListLengthAsync(key));
        }

        #endregion List

        #region SortedSet 有序集合


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
        public async Task<bool> SortedSetAddAsync<T>(string key, T value, double score)
        {
            key = SetPrefix(key);
            return await Do(redis => redis.SortedSetAddAsync(key, SerializerHelper.ToJson(value), score));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<bool> SortedSetRemoveAsync<T>(string key, T value)
        {
            key = SetPrefix(key);
            return await Do(redis => redis.SortedSetRemoveAsync(key, SerializerHelper.ToJson(value)));
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> SortedSetRangeByRankAsync<T>(string key)
        {
            key = SetPrefix(key);
            var values = await Do(redis => redis.SortedSetRangeByRankAsync(key));
            return ConvetList<T>(values);
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> SortedSetLengthAsync(string key)
        {
            key = SetPrefix(key);
            return await Do(redis => redis.SortedSetLengthAsync(key));
        }

        #endregion SortedSet 有序集合

        #region key

        /// <summary>
        /// 删除单个key
        /// </summary>
        /// <param name="key">redis key</param>
        /// <returns>是否删除成功</returns>
        public bool KeyDelete(string key)
        {
            key = SetPrefix(key);
            return Do(db => db.KeyDelete(key));
        }

        /// <summary>
        /// 删除多个key
        /// </summary>
        /// <param name="keys">rediskey</param>
        /// <returns>成功删除的个数</returns>
        public long KeyDelete(List<string> keys)
        {
            List<string> newKeys = keys.Select(SetPrefix).ToList();
            return Do(db => db.KeyDelete(ConvertRedisKeys(newKeys)));
        }

        /// <summary>
        /// 根据前缀删除
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public long KeyDeletePattern(string pattern)
        {
            pattern = SetPrefix(pattern) + "*";

            return Do(db =>

              {
                  var rst = db.ScriptEvaluate(
                      LuaScript.Prepare(

                              // Redis的keys模糊查询：
                              " local res = redis.call('KEYS', @keypattern) " +
                              " return res "), new { @keypattern = pattern });
                  return db.KeyDelete(this.ConvertRedisKeys((string[])rst));
              }
                            );
        }
        /// <summary>
        /// 判断key是否存储
        /// </summary>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public bool KeyExists(string key)
        {
            key = SetPrefix(key);
            return Do(db => db.KeyExists(key));
        }

        /// <summary>
        /// 重新命名key
        /// </summary>
        /// <param name="key">就的redis key</param>
        /// <param name="newKey">新的redis key</param>
        /// <returns></returns>
        public bool KeyRename(string key, string newKey)
        {
            key = SetPrefix(key);
            return Do(db => db.KeyRename(key, newKey));
        }

        /// <summary>
        /// 设置Key的时间
        /// </summary>
        /// <param name="key">redis key</param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool KeyExpire(string key, TimeSpan? expiry = default(TimeSpan?))
        {
            key = SetPrefix(key);
            return Do(db => db.KeyExpire(key, expiry));
        }

        #endregion key

        #region 发布订阅

        /// <summary>
        /// Redis发布订阅  订阅
        /// </summary>
        /// <param name="subChannel"></param>
        /// <param name="handler"></param>
        public async void Subscribe(string subChannel, Action<RedisChannel, RedisValue> handler = null)
        {
            ISubscriber sub = _conn.GetSubscriber();
            await sub.SubscribeAsync(subChannel, (channel, message) =>
            {
                if (handler == null)
                {
                    Console.WriteLine(subChannel + " 订阅收到消息：" + message);
                }
                else
                {
                    handler(channel, message);
                }
            });
        }

        /// <summary>
        /// Redis发布订阅  发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task<long> Publish<T>(string channel, T msg)
        {
            ISubscriber sub = _conn.GetSubscriber();
            return await sub.PublishAsync(channel, SerializerHelper.ToJson(msg));
        }

        /// <summary>
        /// Redis发布订阅  取消订阅
        /// </summary>
        /// <param name="channel"></param>
        public void Unsubscribe(string channel)
        {
            ISubscriber sub = _conn.GetSubscriber();
            sub.Unsubscribe(channel);
        }

        /// <summary>
        /// Redis发布订阅  取消全部订阅
        /// </summary>
        public void UnsubscribeAll()
        {
            ISubscriber sub = _conn.GetSubscriber();
            sub.UnsubscribeAll();
        }

        #endregion 发布订阅

        #region 其他
        /// <summary>
        /// 创建事务
        /// </summary>
        /// <returns></returns>
        public ITransaction CreateTransaction()
        {
            return GetDatabase().CreateTransaction();
        }
        /// <summary>
        /// 获取DB
        /// </summary>
        /// <returns></returns>

        public IDatabase GetDatabase()
        {
            return _conn.GetDatabase(DbNum);
        }

        /// <summary>
        /// 获取服务器
        /// </summary>
        /// <param name="hostAndPort"></param>
        /// <returns></returns>
        public IServer GetServer(string hostAndPort)
        {
            return _conn.GetServer(hostAndPort);
        }

        /// <summary>
        /// 设置前缀
        /// </summary>
        /// <param name="prefix"></param>
        public void SetSysCustomKey(string prefix)
        {
            this.prefix = prefix;
        }

        #endregion 其他

        #region 辅助方法

        private string SetPrefix(string oldKey)
        {
            return $"{prefix}_{oldKey}";
        }

        private T Do<T>(Func<IDatabase, T> func)
        {
            var database = _conn.GetDatabase(DbNum);
            return func(database);
        }


        private List<T> ConvetList<T>(RedisValue[] values)
        {
            List<T> result = new List<T>();
            foreach (var item in values)
            {
                var model = SerializerHelper.FromJson<T>(item);
                result.Add(model);
            }
            return result;
        }

        private RedisKey[] ConvertRedisKeys(List<string> redisKeys)
        {
            return redisKeys.Select(redisKey => (RedisKey)redisKey).ToArray();
        }
        private RedisKey[] ConvertRedisKeys(string[] redisKeys)
        {
            return redisKeys.Select(redisKey => (RedisKey)redisKey).ToArray();
        }

        #endregion 辅助方法

        #region DB 方法
        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public void ClearAll()
        {
            var endpoints = _conn.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = _conn.GetServer(endpoint);
                server.FlushAllDatabases();
            }
        }
        #endregion


        #region 分布式锁...

        /// <summary>
        /// 获取锁。
        /// </summary>
        /// <param name="key">锁名称。</param>
        /// <param name="seconds">过期时间（秒）。</param>
        /// <returns>是否已锁。</returns>
        public bool Lock(string key, int seconds)
        {
            key = SetPrefix(key);
            return Do(db => db.LockTake(key, prefix, new TimeSpan(0, 0, seconds)));
        }

        /// <summary>
        /// 释放锁。
        /// </summary>
        /// <param name="key">锁名称。</param>
        /// <returns>是否成功。</returns>
        public bool UnLock(string key)
        {
            key = SetPrefix(key);
            return Do(db => db.LockRelease(key, prefix));
        }

        /// <summary>
        /// 异步获取锁。
        /// </summary>
        /// <param name="key">锁名称。</param>
        /// <param name="seconds">过期时间（秒）。</param>
        /// <returns>是否成功。</returns>
        public async Task<bool> LockAsync(string key, int seconds)
        {
            key = SetPrefix(key);
            var database= _conn.GetDatabase(DbNum);
            return await database.LockTakeAsync(key, prefix, TimeSpan.FromSeconds(seconds));
        }

        /// <summary>
        /// 异步释放锁。
        /// </summary>
        /// <param name="key">锁名称。</param>
        /// <returns>是否成功。</returns>
        public async Task<bool> UnLockAsync(string key)
        {
            key = SetPrefix(key);
            var database = _conn.GetDatabase(DbNum);
            return await database.LockReleaseAsync(key, prefix);
        }

        #endregion
    }
}
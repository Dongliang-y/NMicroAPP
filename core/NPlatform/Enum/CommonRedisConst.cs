using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NPlatform
{
    public static class CommonRedisConst
    {
        /// <summary>
        /// 获取第三方登录时缓存手机号码的key
        /// </summary>
        /// <param name="token">token</param>
        /// <returns></returns>
        public static string GetOtherPhoneKey(string token)
        {
            return "phone" + token;
        }
    }
}

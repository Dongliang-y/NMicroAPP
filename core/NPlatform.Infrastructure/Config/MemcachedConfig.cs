/********************************************************************************

** auth： DongliangYi

** date： 2016/8/31 15:07:14

** desc： Memcached 配置文件实例

** Ver.:  V1.0.0

*********************************************************************************/

namespace NPlatform.Config
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Net;
    using System.Web;

    /// <summary>
    /// Memcached 管理
    /// </summary>
    public class MemcachedConfig : IConfig
    {
        private int _defaultCacheHours = 1;

        private int _maxPoolSize = 200;

        private string _memcachedProtocol = "Text";

        private int _minPoolSize = 5;

        private string _passWord = string.Empty;

        private List<IPEndPoint> _servers = new List<IPEndPoint>();

        private string _userName = string.Empty;

        /// <summary>
        /// 默认缓存时间，单位为小时~
        /// </summary>
        public int DefaultCacheHours
        {
            get
            {
                return _defaultCacheHours;
            }

            set
            {
                _defaultCacheHours = value;
            }
        }

        /// <summary>
        /// 最大连接数
        /// </summary>
        public int MaxPoolSize
        {
            get
            {
                return _maxPoolSize;
            }

            set
            {
                _maxPoolSize = value;
            }
        }

        /// <summary>
        /// 协议  Binary/Text
        /// </summary>
        public string MemcachedProtocol
        {
            get
            {
                return _memcachedProtocol;
            }

            set
            {
                _memcachedProtocol = value;
            }
        }

        /// <summary>
        /// 最小连接数
        /// </summary>
        public int MinPoolSize
        {
            get
            {
                return _minPoolSize;
            }

            set
            {
                _minPoolSize = value;
            }
        }

        /// <summary>
        /// 登陆密码
        /// </summary>
        public string PassWord
        {
            get
            {
                return _passWord;
            }

            set
            {
                _passWord = value;
            }
        }

        /// <summary>
        /// 服务列表
        /// </summary>
        public List<IPEndPoint> Servers
        {
            get
            {
                return _servers;
            }

            set
            {
                _servers = value;
            }
        }

        /// <summary>
        /// 登陆用户
        /// </summary>
        public string UserName
        {
            get
            {
                return _userName;
            }

            set
            {
                _userName = value;
            }
        }
    }
}
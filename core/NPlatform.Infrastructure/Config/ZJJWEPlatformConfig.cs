/********************************************************************************

** auth： DongliangYi

** date： 2016/8/31 15:07:14

** desc： Memcached 配置文件实例

** Ver.:  V1.0.0

*********************************************************************************/

namespace NPlatform.Config
{
    using Com.Ctrip.Framework.Apollo;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel;
    using NPlatform.Config;

    /// <summary>
    /// 通用配置管理
    /// </summary>
    public class NPlatformConfig : IConfig
    {
        /// <summary>
        /// 机器ID ，集群部署时的设备编号。
        /// </summary>
        public long MachineID { get; set; } =1;

        /// <summary>
        /// 分布式部署时服务ID
        /// </summary>
        public long ServiceID { get; set; } = 1;

        /// <summary>
        /// 是否为调试模式
        /// </summary>
        public bool IsDebug { get; set; } = true;

        private string company="";
        private string companyUrl;
        private string dbProvider;
        private string platformVersion;
        private string iCP;
        private string[] iocAssemblys;
        private dynamic dynamicConfigs;
        private dynamic apiEndPoints;
        private string systemUrl;
        private string systemName;
        private string support;
        private string team;

        /// <summary>
        /// 公司
        /// </summary>
        public string Company
        {
            get
            {
                return ApolloConfiguration.GetConfig(nameof(NPlatformConfig), nameof(Company), this.company);
            }
            set
            {
                this.company = value;
            }
        }

        /// <summary>
        /// CompanyUrl
        /// </summary>
        public string CompanyUrl
        {
            get
            {
                return ApolloConfiguration.GetConfig(nameof(NPlatformConfig), nameof(CompanyUrl), this.companyUrl);
            }
            set
            {
                this.companyUrl = value;
            }
        }

        /// <summary>
        /// 链接字符串
        /// </summary>
        public CustomDic ConnectionString
        {
            get;set;
        }
        /// <summary>
        /// 数据 提供程序
        /// </summary>
        public string DBProvider
        {
            get
            {
                return ApolloConfiguration.GetConfig(nameof(NPlatformConfig), nameof(DBProvider), this.dbProvider);
            }
            set
            {
                this.dbProvider = value;
            }
        }

        /// <summary>
        /// 版本号
        /// </summary>
        public string PlatformVersion
        {
            get
            {
                return ApolloConfiguration.GetConfig(nameof(NPlatformConfig), nameof(PlatformVersion), this.platformVersion);
            }
            set
            {
                this.platformVersion = value;
            }
        }

        /// <summary>
        /// ICP备案号
        /// </summary>
        public string ICP
        {
            get
            {
                return ApolloConfiguration.GetConfig(nameof(NPlatformConfig), nameof(ICP), this.iCP);
            }
            set
            {
                this.iCP = value;
            }
        }

        /// <summary>
        /// 需要注入的dll
        /// </summary>
        public string[] IOCAssemblys
        {
            get
            {
                var conns = ApolloConfiguration.GetConfig(nameof(NPlatformConfig), nameof(IOCAssemblys), "");
                if (string.IsNullOrEmpty(conns))
                {
                    return iocAssemblys;
                }
                else
                {
                    return conns.Split(",",System.StringSplitOptions.RemoveEmptyEntries);
                }
            }
            set
            {
                this.iocAssemblys = value;
            }
        }

        /// <summary>
        /// 授权配置
        /// </summary>
        public AuthServerConfig Authinfo { get; set; } = new AuthServerConfig();

        /// <summary>
        /// 配置信息
        /// </summary>
        public CustomDic PlatformConfigs { get; set; }

        /// <summary>
        /// 可变类型的配置项
        /// </summary>
        public dynamic DynamicConfigs
        {
            get
            {
                var result = ApolloConfiguration.GetConfig(nameof(NPlatformConfig), nameof(DynamicConfigs), "");
                if (!string.IsNullOrEmpty(result))
                {
                    return JsonConvert.DeserializeObject<dynamic>(result);
                }
                else
                {
                    return this.dynamicConfigs;
                }
            }
            set
            {
                this.dynamicConfigs = value;
            }
        }
        /// <summary>
        /// 技术支持
        /// </summary>
        public string Support
        {
            get
            {
                return ApolloConfiguration.GetConfig(nameof(NPlatformConfig), nameof(Support), this.support);
            }
            set
            {
                this.support = value;
            }
        }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// 系统地址
        /// </summary>
        public string SystemUrl
        {
            get
            {
                return ApolloConfiguration.GetConfig(nameof(NPlatformConfig), nameof(SystemUrl), this.systemUrl);
            }
            set
            {
                this.systemUrl = value;
            }
        }

        /// <summary>
        /// 开发团队
        /// </summary>
        public string Team
        {
            get
            {
                return ApolloConfiguration.GetConfig(nameof(NPlatformConfig), nameof(Team), this.team);
            }
            set
            {
                this.team = value;
            }
        }

        /// <summary>
        /// 配置信息JGAppSecret
        /// </summary>
        public CustomDic UserConfigs { get; set; }

        /// <summary>
        /// API调用端口,配置的格式   {QCM:{xxget:'',xxxDelete:''},MPM:{zzzAdd:'',zzzDelete:''}}
        /// </summary>
        public dynamic APIEndPoints
        {
            get
            {
                var result = ApolloConfiguration.GetConfig(nameof(NPlatformConfig), nameof(APIEndPoints), "");
                if (!string.IsNullOrEmpty(result))
                {
                    return JsonConvert.DeserializeObject<dynamic>(result);
                }
                else
                {
                    return this.apiEndPoints;
                }
            }
            set
            {
                this.apiEndPoints = value;
            }
        }
    }
}
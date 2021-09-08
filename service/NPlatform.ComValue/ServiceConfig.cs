using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPlatform.ComValue
{
    /// <summary>
    /// 本地服务配置
    /// </summary>
    [Serializable]
    public class ServiceConfig
    {
        public string MachineID { get; set; }
        public string ServiceID { get; set; }
        public string ServiceName { get; set; }
        public string ListenIP { get; set; }
        public int Port { get; set; }
    }
}

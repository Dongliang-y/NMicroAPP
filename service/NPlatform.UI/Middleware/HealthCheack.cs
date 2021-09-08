using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NPlatform.UI.Middleware
{
    public class MyHealthChecks : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var kvs = new Dictionary<string, object>();
            kvs.Add("userName", "admin");
            var dic = new ReadOnlyDictionary<string, object>(kvs);
           // HealthCheckResult healthCheckResult = HealthCheckResult.Unhealthy("test", new Exception("测试检查失败的"), dic);
            HealthCheckResult healthCheckResult = HealthCheckResult.Healthy("test", dic);
            // 这里可以去检查下 数据库链接、redis等情况
            return Task.FromResult(healthCheckResult);
        }

    }
}

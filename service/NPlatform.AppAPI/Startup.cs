using Autofac;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NPlatform.Infrastructure.Config;
using NPlatform.Middleware;
using NPlatform.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NPlatform.AppAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
            var svcConfig = Configuration.GetServiceConfig();
            services.AddHealthChecks().AddCheck<NHealthChecks>(svcConfig.ServiceName); ;
          
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NPlatform.AppAPI", Version = "v1" });
            });

#region NPlatform代码
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            NPlatformStartup.Configure(builder, new Repositories.RepositoryOptions()
            {
                DBProvider = DBProvider.MySqlClient,
                MainConection = Configuration.GetConnectionString("MainConection"), //使用简单的数据库集群
                MinorConnection = Configuration.GetConnectionString("MinorConnection")
            }, Configuration);
        }
        #endregion
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.Extensions.Hosting.IHostApplicationLifetime aft)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NPlatform.AppAPI v1"));
            }
            aft.ApplicationStarted.Register(() =>

            {

                Console.WriteLine("Started");
                RegisterConsul();
            });

            aft.ApplicationStopped.Register(() =>

            {
                DeregisterConsul();
                Console.WriteLine("ApplicationStopped");

            });

            app.UseHealthChecks("/healthChecks");
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }



        public static string RegistrationID = "";

        public void DeregisterConsul()
        {

            //请求注册的 Consul 地址 ConsulClient 地址//这里的这个ip 就是本机的ip，这个端口8500 这个是默认注册服务端口 
            ConsulClient consulClient = new ConsulClient(p => { p.Address = new Uri(Configuration.GetValue<string>("ConsulClient")); });
            consulClient.Agent.ServiceDeregister(RegistrationID);
        }
        public void RegisterConsul()
        {
            //请求注册的 Consul 地址 ConsulClient 地址//这里的这个ip 就是本机的ip，这个端口8500 这个是默认注册服务端口 

            ConsulClient consulClient = new ConsulClient(p => { p.Address = new Uri(Configuration.GetValue<string>("ConsulClient")); });
            var svcConfig = Configuration.GetServiceConfig();
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10),//间隔固定的时间访问一次，https://localhost:44308/api/Health
                HTTP = $"{svcConfig.Address}:{svcConfig.Port}/healthChecks",//健康检查地址 44308是visualstudio启动的端口
                Timeout = TimeSpan.FromSeconds(5)
            };

            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = $"{svcConfig.ServiceName}{svcConfig.DataCenterID}_{svcConfig.ServiceID}",
                Name = svcConfig.ServiceName,
                Address = svcConfig.Address,
                Port = svcConfig.Port,

            };
            RegistrationID = registration.ID;
            consulClient.Agent.ServiceRegister(registration).Wait();//注册服务 

            //consulClient.Agent.ServiceDeregister(registration.ID).Wait();//registration.ID是guid
            //当服务停止时需要取消服务注册，不然，下次启动服务时，会再注册一个服务。
            //但是，如果该服务长期不启动，那consul会自动删除这个服务，大约2，3分钟就会删了 
        }
    }
}

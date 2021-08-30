using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NPlatform.ComValue;
using NPlatform.UI.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NPlatform.UI
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

            string serviceName = Configuration.GetValue<string>("ServiceConfig:ServiceName");
            services.AddHealthChecks().AddCheck<MyHealthChecks>(serviceName); ;
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.Extensions.Hosting.IApplicationLifetime aft)
        {
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

            aft.ApplicationStopping.Register(() =>

            {

                Console.WriteLine("ApplicationStopping");

            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHealthChecks("/healthChecks");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCnblogsHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
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

            var serviceConfig = Configuration.GetSection("ServiceConfig");
            ServiceConfig svcConfig = new ServiceConfig();
            svcConfig.ListenIP = serviceConfig.GetValue<string>("ListenIP");
            svcConfig.Port = serviceConfig.GetValue<int>("Port");
            svcConfig.ServiceName = serviceConfig.GetValue<string>("ServiceName");
            svcConfig.MachineID = serviceConfig.GetValue<string>("MachineID");
            svcConfig.ServiceID = serviceConfig.GetValue<string>("ServiceID");

            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10),//间隔固定的时间访问一次，https://localhost:44308/api/Health
                HTTP = $"{svcConfig.ListenIP}:{svcConfig.Port}/ healthChecks",//健康检查地址 44308是visualstudio启动的端口
                Timeout = TimeSpan.FromSeconds(5)
            };

            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = svcConfig.MachineID+":"+svcConfig.ServiceID,
                Name = svcConfig.ServiceName,
                Address = svcConfig.ListenIP ,
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

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
using System.Net;
using System.Threading.Tasks;

namespace NPlatform.API
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
            // 1.替换controller管理对象。
            // 2.注入 API.DLL 并注入 controller。
            // controller 内就可以使用属性注入了。
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            var serviceConfig = Configuration.GetServiceConfig();
            string serviceName = serviceConfig.ServiceName;
            services.AddHealthChecks().AddCheck<NHealthChecks>(serviceName); ;
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc($"{ serviceConfig.ServiceName}_{ serviceConfig.ServiceVersion }", new OpenApiInfo { Title = serviceConfig.ServiceName, Version = serviceConfig.ServiceVersion });
            });
        }

        public void ConfigureContainer(ContainerBuilder builder) => builder.Configure(Configuration);

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, 
            Microsoft.Extensions.Hosting.IHostApplicationLifetime aft, ILoggerFactory loggerFactory)
        {
            var serviceConfig = Configuration.GetServiceConfig();
            // 添加日志Provider
            loggerFactory.AddLog4Net();

            if (env.IsDevelopment())
            {
                 app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/"+ $"{ serviceConfig.ServiceName}_{ serviceConfig.ServiceVersion }" + "/swagger.json", serviceConfig.ServiceName));
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

            //  app.UseCnblogsHttpsRedirection();
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

            var serviceConfig = Configuration.GetServiceConfig();
            var urls = Configuration.GetValue<string>("Urls");
            if(string.IsNullOrEmpty(urls))
            {
                throw new Exception("未配置服务监听地址");
            }

            var urlArray = urls.Split(";", StringSplitOptions.RemoveEmptyEntries);
            if(urlArray.Length<1)
            {
                throw new Exception("未配置服务监听地址");
            }

            var uri = new Uri(urlArray[0].Replace("*","localhost"));
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10),//间隔固定的时间访问一次，https://localhost:44308/api/Health
                HTTP = $"{uri.Scheme}://{uri.Authority}/healthChecks",//健康检查地址 44308是visualstudio启动的端口
                Timeout = TimeSpan.FromSeconds(5)
            };

            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID =$"{serviceConfig.ServiceName}{serviceConfig.DataCenterID}_{serviceConfig.ServiceID}",
                Name = serviceConfig.ServiceName,
                Address = $"{uri}",
                Port = uri.Port,
            };
            RegistrationID = registration.ID;
            consulClient.Agent.ServiceRegister(registration).Wait();//注册服务 

            //consulClient.Agent.ServiceDeregister(registration.ID).Wait();//registration.ID是guid
            //当服务停止时需要取消服务注册，不然，下次启动服务时，会再注册一个服务。
            //但是，如果该服务长期不启动，那consul会自动删除这个服务，大约2，3分钟就会删了 
        }
    }
}

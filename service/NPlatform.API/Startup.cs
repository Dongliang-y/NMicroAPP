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
            // 1.�滻controller�������
            // 2.ע�� API.DLL ��ע�� controller��
            // controller �ھͿ���ʹ������ע���ˡ�
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
            // �����־Provider
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

            //����ע��� Consul ��ַ ConsulClient ��ַ//��������ip ���Ǳ�����ip������˿�8500 �����Ĭ��ע�����˿� 
            ConsulClient consulClient = new ConsulClient(p => { p.Address = new Uri(Configuration.GetValue<string>("ConsulClient")); });
            consulClient.Agent.ServiceDeregister(RegistrationID);
        }

        public void RegisterConsul()
        {
            //����ע��� Consul ��ַ ConsulClient ��ַ//��������ip ���Ǳ�����ip������˿�8500 �����Ĭ��ע�����˿� 
            ConsulClient consulClient = new ConsulClient(p => { p.Address = new Uri(Configuration.GetValue<string>("ConsulClient")); });

            var serviceConfig = Configuration.GetServiceConfig();
            var urls = Configuration.GetValue<string>("Urls");
            if(string.IsNullOrEmpty(urls))
            {
                throw new Exception("δ���÷��������ַ");
            }

            var urlArray = urls.Split(";", StringSplitOptions.RemoveEmptyEntries);
            if(urlArray.Length<1)
            {
                throw new Exception("δ���÷��������ַ");
            }

            var uri = new Uri(urlArray[0].Replace("*","localhost"));
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//����������ú�ע��
                Interval = TimeSpan.FromSeconds(10),//����̶���ʱ�����һ�Σ�https://localhost:44308/api/Health
                HTTP = $"{uri.Scheme}://{uri.Authority}/healthChecks",//��������ַ 44308��visualstudio�����Ķ˿�
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
            consulClient.Agent.ServiceRegister(registration).Wait();//ע����� 

            //consulClient.Agent.ServiceDeregister(registration.ID).Wait();//registration.ID��guid
            //������ֹͣʱ��Ҫȡ������ע�ᣬ��Ȼ���´���������ʱ������ע��һ������
            //���ǣ�����÷����ڲ���������consul���Զ�ɾ��������񣬴�Լ2��3���Ӿͻ�ɾ�� 
        }
    }
}

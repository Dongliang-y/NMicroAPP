#region << 版 本 注 释 >>

/*----------------------------------------------------------------
* 项目名称 ：ZJJWEPlatform.IOC
* 类 描 述 ：
* 命名空间 ：ZJJWEPlatform.IOC
* CLR 版本 ：4.0.30319.42000
* 作    者 ：DongliangYi
* 创建时间 ：2018-12-11 9:27:28
* 更新时间 ：2018-12-11 9:27:28
* 版 本 号 ：v1.0.0.0
//----------------------------------------------------------------*/

#endregion

namespace ZJJWEPlatform.IOC
{
    using Autofac;
    using Autofac.Core;
    using Autofac.Extras.DynamicProxy;
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Autofac.Extras.DynamicProxy;
    using Microsoft.Extensions.DependencyInjection;
    using NPOI.SS.Formula.Functions;
    using ZJJWEPlatform.Filters;
    using ZJJWEPlatform.Infrastructure;
    using ServiceStack;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using ZJJWEPlatform.Applications;
    // using ZJJWEPlatform.API.Controllers;
    using ZJJWEPlatform.Config;
    using ZJJWEPlatform.Domains.IRepositories;
    using ZJJWEPlatform.Filters;
    using ZJJWEPlatform.Infrastructure;
    using ZJJWEPlatform.Infrastructure.Loger;
    using ZJJWEPlatform.Repositories;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// ioc 管理类
    /// </summary>
    public class IOCManager
    {
        /// <summary>
        /// 异步锁
        /// </summary>
        private static readonly object SyncRoot = new object();

        private static Config.RedisConfig redisConfig = new ConfigFactory<RedisConfig>().Build();

        /// <summary>
        /// IOC 容器对象
        /// </summary>
        private static ILifetimeScope container;

        /// <summary>
        /// Prevents a default instance of the <see cref="IOCManager"/> class from being created. 
        ///  IOC管理
        /// </summary>
        private IOCManager()
        {
        }
        /// <summary>
        /// Prevents a default instance of the <see cref="IOCManager"/> class from being created. 
        ///  IOC管理
        /// </summary>
        public static void InitContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            Install(null, builder);
            container = builder.Build().BeginLifetimeScope();
        }
        /// <summary>
        /// IOC容器
        /// </summary>
        public static ILifetimeScope Container
        {
            get
            {
                lock (SyncRoot)
                {
                    if (container == null)
                    {
                        throw new ZJJWEPlatformException("IOC 容器未初始化", "IOCManager.Container");
                    }

                    return container;
                }
            }
            set
            {
                container = value;
            }
        }

        /// <summary>
        /// 默认配置
        /// </summary>
        private static IRepositoryOptions DefaultOption { get; set; }
        /// <summary>
        /// 获取通用的服务实现，所有基于 “接口-实现”注入进来的都可以使用此方法。
        /// </summary>
        /// <typeparam name="TService">仓储接口定义</typeparam>
        /// <param name="name">类名</param>
        /// <returns>返回服务</returns>
        public static T BuildByName<T>(string name) where T : class
        {
            object rst;
            if (Container.TryResolveNamed(name, typeof(T), out rst))
            {
                return rst as T;
            }
            else
            {
                Console.WriteLine($"TryResolveNamed:{name}--false");
            }
            return null;
        }

        /// <summary>
        /// 获取仓储
        /// </summary>
        /// <param name="rspOption">
        /// The rsp Option.
        /// </param>
        /// <typeparam name="TRepository">
        /// 仓储接口
        /// </typeparam>
        /// <typeparam name="TEntity">
        /// 实体类型
        /// </typeparam>
        /// <returns>
        /// 仓储
        /// </returns>
        public static TRepository BuildRepository<TRepository, TEntity>(IRepositoryOptions rspOption = null)
        {
            if (rspOption == null)
            {
                rspOption = DefaultOption;
            }
            return Container.Resolve<TRepository>((new NamedParameter("options", rspOption)));
        }

        /// <summary>
        /// 获取通用的服务实现，所有基于 “接口-实现”注入进来的都可以使用此方法。
        /// </summary>
        /// <typeparam name="TService">仓储接口定义</typeparam>
        /// <param name="arguments">服务构造参数</param>
        /// <returns>返回服务</returns>
        public static TService BuildService<TService>(params object[] arguments)
        {
            Console.WriteLine(typeof(TService).FullName);
            if (arguments == null || arguments.Length == 0)
            {
                return Container.Resolve<TService>();
            }
            return Container.Resolve<TService>(new NamedParameter("arguments", arguments));
        }

        /// <summary>
        /// 获取工作单元
        /// </summary>
        /// <param name="option">
        /// The option.
        /// </param>
        /// <returns>
        /// 返回工作单元
        /// </returns>
        public static IUnitOfWork BuildUnitOfWork(IContextOptions option = null)
        {
            if (option == null)
            {
                option = new Repositories.ContextOptions { IsTransactional = true };
            }

            //  IDictionary parameters = new Hashtable { { "option", option } };
            return Container.Resolve<IUnitOfWork>((new NamedParameter("option", option)));
        }

        /// <summary>
        /// 注入中间件
        /// </summary>
        /// <param name="rspOptions">
        /// The rsp Options.
        /// </param>
        /// <param name="builder"></param>
        public static void Install(IRepositoryOptions rspOptions, ContainerBuilder builder)
        {
            DefaultOption = rspOptions ?? new Repositories.RepositoryOptions();
            var cfg = new ConfigFactory<ZJJWEPlatformConfig>().Build();
            if (cfg.IOCAssemblys.Length == 0)
            {
                throw new ZJJWEPlatformException("ZJJWEPlatformConfig IOCAssemblys is null", "IOC Install");
            }

            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().AsImplementedInterfaces().PropertiesAutowired().InstancePerLifetimeScope().InstancePerDependency();
            builder.RegisterType<PlatformHttpContext>().As<IPlatformHttpContext>().AsImplementedInterfaces().PropertiesAutowired().InstancePerLifetimeScope().InstancePerDependency();
            //builder.RegisterType<RedisHelper>().PropertiesAutowired().InstancePerLifetimeScope().InstancePerDependency().SingleInstance();
            //builder.RegisterType<RedisCacheInterceptor>().PropertiesAutowired().InstancePerLifetimeScope().InstancePerDependency().SingleInstance();

            var path = System.IO.Directory.GetCurrentDirectory();
            List<Assembly> assemblys = new List<Assembly>();
            foreach (var tmp in cfg.IOCAssemblys)
            {
                try
                {
                    var assPath = Path.Combine(path, tmp);
                    Assembly service = Assembly.LoadFrom(assPath);
                    assemblys.Add(service);
                    //    builder.RegisterGeneric(typeof(IClassMapperConfig<>)).InstancePerLifetimeScope();
                    //    builder.re builder.RegisterGeneric(typeof(List<>)).As(typeof(IList<>)).InstancePerLifetimeScope();
                    //IClassMapperConfig<IMapperConfigurationExpression>  
                }
                catch (FileNotFoundException ex)
                {
                    throw new ZJJWEPlatformException(
                        $"没找到配置指定注入的{ex.FileName}，请确认dll是否已正确复制，名称是否与配置的相同。",
                        "IOCManager.Install.FileNotFoundException");
                }
            }

            //            InstancePerLifetimeScope：同一个Lifetime生成的对象是同一个实例
            //SingleInstance：单例模式，每次调用，都会使用同一个实例化的对象；每次都用同一个对象；
            //InstancePerDependency：默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；



            // 读取redisCache配置内容
            
            var redisHelper = new RedisHelper(redisConfig.Pipe);
            // 注册一个redisCache拦截器
            var instance = new RedisCacheInterceptor(redisHelper, redisConfig.EnableCacheInterceptor);
            builder.RegisterInstance(instance)
               ;


            var typeConfig = typeof(IClassMapperConfig);
            var regBuilder = builder.RegisterAssemblyTypes(assemblys.ToArray()).Where(t =>
             t.Name.EndsWith("Service")
             || t.Name.EndsWith("Application")
             || t.Name.EndsWith("Configs")
             )
            .AsImplementedInterfaces()//
            .PropertiesAutowired()

            .InstancePerLifetimeScope()
            // 加入缓存注入
          //  .InterceptedBy(typeof(RedisCacheInterceptor))
            .EnableInterfaceInterceptors();

            builder.RegisterAssemblyTypes(assemblys.ToArray()).Where(t =>
             t.Name.EndsWith("Reflect"))
            .AsImplementedInterfaces()//
            .PropertiesAutowired()
            .InstancePerLifetimeScope()
            .Named<IApplication>(t => t.Name);

            builder.RegisterAssemblyTypes(assemblys.ToArray()).Where(t =>
             t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces()//
            .PropertiesAutowired()
            .InstancePerDependency();

            //.OnRegistered(e => LogerHelper.Debug($"Registered：{e.ComponentRegistration.Activator.LimitType}", LogType.Trace.ToString()))
            //.OnActivated(e => LogerHelper.Debug($"Registered：{e.Component.Activator.LimitType}", LogType.Trace.ToString()));

            //if(redisCOnfig.EnableCacheInterceptor)
            //{
            //    regBuilder.InterceptedBy(typeof(RedisCacheInterceptor))
            //    .EnableInterfaceInterceptors();
            //}
            // regBuilder.InstancePerDependency();

            builder.RegisterType<RepositoryOptions>().As<IRepositoryOptions>().AsImplementedInterfaces();
            builder.RegisterType<ContextOptions>().As<IContextOptions>().AsImplementedInterfaces();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().AsImplementedInterfaces();

            //var execType = typeof(IActionResultExecutor<EPContent>);
            //builder.RegisterAssemblyTypes(assemblys.ToArray())
            //    .Where(t => execType.IsAssignableFrom(t)).PropertiesAutowired().InstancePerDependency();

            // .AsImplementedInterfaces()//表示注册的类型，以接口的方式注册不包括IDisposable接口
            ///        .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy,使用接口的拦截器，在使用特性 [Attribute] 注册时，注册拦截器可注册到接口(Interface)上或其实现类(Implement)上。使用注册到接口上方式，所有的实现类都能应用到拦截器。
            // .InstancePerLifetimeScope();//即为每一个依赖或调用创建一个单一的共享的实例
        }

        /// <summary>
        /// 获取automapper配置
        /// </summary>
        /// <returns>返回map配置类型</returns>
        public static IEnumerable<IClassMapperConfig> ResolveAutoMapper()
        {
            return Container.Resolve<IEnumerable<IClassMapperConfig>>();
        }
    }
}
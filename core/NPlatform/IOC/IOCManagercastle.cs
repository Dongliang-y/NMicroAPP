#region << 版 本 注 释 >>

/*----------------------------------------------------------------
* 项目名称 ：ZJJWEPlatform.IOC
* 类 名 称 ：CastleInstall
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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using AutoMapper;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using ZJJWEPlatform.API.Controllers;
    using ZJJWEPlatform.Config;
    using ZJJWEPlatform.Domains.IRepositories;
    using ZJJWEPlatform.Repositories;

    /// <summary>
    /// ioc 管理类
    /// </summary>
    public class IOCManager
    {
        /// <summary>
        /// 异步锁
        /// </summary>
        private static readonly object SyncRoot = new object();

        /// <summary>
        /// IOC 容器对象
        /// </summary>
        private static IWindsorContainer container;

        /// <summary>
        /// Prevents a default instance of the <see cref="IOCManager"/> class from being created. 
        ///  IOC管理
        /// </summary>
        private IOCManager()
        {
        }

        /// <summary>
        /// IOC容器
        /// </summary>
        public static IWindsorContainer Container
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
        }

        /// <summary>
        /// 默认配置
        /// </summary>
        private static IRepositoryOptions DefaultOption { get; set; }

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
          //  IDictionary parameters = new Hashtable { {, rspOption } };
            Arguments arg = new Arguments();
            arg.Add("option", rspOption);
            return Container.Resolve<TRepository>(arg);
        }

        /// <summary>
        /// 获取通用的服务实现，所有基于 “接口-实现”注入进来的都可以使用此方法。
        /// </summary>
        /// <typeparam name="TService">仓储接口定义</typeparam>
        /// <param name="rspOption">服务构造参数</param>
        /// <returns>返回服务</returns>
        public static TService BuildService<TService>(params object[] rspOption)
        {
            if (rspOption == null|| rspOption.Length==0)
            {
               return Container.Resolve<TService>();
            }
            Arguments arg = new Arguments();
            arg.Add("arguments", rspOption);
            return Container.Resolve<TService>(arg);
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

            Arguments arg = new Arguments();
            arg.Add("option", option);

            return Container.Resolve<IUnitOfWork>(arg);
        }

        /// <summary>
        /// 注入中间件
        /// </summary>
        /// <param name="rspOptions">
        /// The rsp Options.
        /// </param>
        public static void Install(IRepositoryOptions rspOptions)
        {
            if (rspOptions == null)
                rspOptions = new RepositoryOptions();
            DefaultOption = rspOptions;
            var cfg = new ConfigFactory<ZJJWEPlatformConfig>().Build();
            if (cfg.IOCAssemblys.Length == 0)
            {
                throw new ZJJWEPlatformException("ZJJWEPlatformConfig IOCAssemblys is null", "Install.Instance");
            }

            IList<IWindsorInstaller> installers = new List<IWindsorInstaller>();

            var path = System.IO.Directory.GetCurrentDirectory();
            foreach (var tmp in cfg.IOCAssemblys)
            {
                try
                {
                    installers.Add(FromAssembly.Named(Path.Combine(path,tmp)));
                }
                catch (FileNotFoundException ex)
                {
                    throw new ZJJWEPlatformException(
                        $"没找到配置指定注入的{ex.FileName}，请确认dll是否已正确复制，名称是否与配置的相同。",
                        "IOCManager.Install.FileNotFoundException");
                }
            }



            container = new WindsorContainer().Install(installers.ToArray());

            //////注入配置类型
            container.Register(Classes.FromAssembly(Assembly.GetExecutingAssembly())
                .BasedOn(typeof(IActionResultExecutor<EPContent>))
                .WithService.AllInterfaces()
               // .DefaultInterfaces()    //使用默认的I+ServiceName的方式来取Service
                .LifestyleTransient()); // like new
                                        ////注入配置类型
            container.Register(Classes.FromAssembly(Assembly.GetExecutingAssembly())
                .BasedOn(typeof(IUnitOfWork))
                .WithService
                .DefaultInterfaces()    //使用默认的I+ServiceName的方式来取Service
                .LifestyleTransient()); // like new

            //container.Register(Classes.FromAssembly(Assembly.GetExecutingAssembly())
            //.BasedOn(typeof(IContextOptions))
            //.WithService
            //.DefaultInterfaces()    //使用默认的I+ServiceName的方式来取Service
            //.LifestyleTransient()); // like new
        }

        /// <summary>
        /// 获取automapper配置
        /// </summary>
        /// <typeparam name="TMapperConfig">配置文件类</typeparam>
        /// <returns>返回map配置类型</returns>
        public static IClassMapperConfig<IMapperConfigurationExpression>[] ResolveAutoMapper()
        {
            var mappers = Container.ResolveAll<IClassMapperConfig<IMapperConfigurationExpression>>();
            return mappers;
        }

        /// <summary>
        /// 注销
        /// </summary>
        public static void UnInstall()
        {
            if (Container != null)
            {
                Container.Dispose();
            }
        }
    }
}
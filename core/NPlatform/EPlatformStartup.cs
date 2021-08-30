#region << 版 本 注 释 >>

/*----------------------------------------------------------------
* 项目名称 ：NPlatform.Domains
* 类 名 称 ：NPlatformStartup
* 类 描 述 ：
* 命名空间 ：NPlatform.Domains
* CLR 版本 ：4.0.30319.42000
* 作    者 ：DongliangYi
* 创建时间 ：2018-11-20 16:37:16
* 更新时间 ：2018-11-20 16:37:16
* 版 本 号 ：v1.0.0.0
//----------------------------------------------------------------*/

#endregion

namespace NPlatform
{
    using AutoMapper;
   // using Lincence.Verify;
    using System.IO;
    using NPlatform.Config;
    using NPlatform.Filters;
    using NPlatform.IOC;

    /// <summary>
    /// 平台初始化对象. IOC容器加载、缓存初始化。
    /// </summary>
    public class NPlatformStartup
    {
        /// <summary>
        /// 定义一个标识确保线程同步
        /// </summary>
        private static readonly object locker = new object();

        /// <summary>
        /// 平台配置项
        /// </summary>
        public NPlatformConfig Config { get; set; }

        /// <summary>
        /// 定义一个静态变量来保存类的实例
        /// </summary>
        private static NPlatformStartup uniqueInstance;

        /// <summary>
        /// 是否加载完成.
        /// </summary>
        public static bool AutoMapperInitialized { set; get; } = false;

        /// <summary>
        /// Prevents a default instance of the <see cref="NPlatformStartup"/> class from being created. 
        /// 定义私有构造函数，使外界不能创建该类实例
        /// </summary>
        private NPlatformStartup()
        {
            // 加载配置
            Config = new ConfigFactory<NPlatformConfig>().Build();
            AutoMapperInit();
        }

        /// <summary>
        /// 全局唯一的,NPlatformStartup 对象.
        /// </summary>
        /// <returns>NPlatformStartup</returns>
        public static NPlatformStartup Start()
        {
//#if DEBUG
//#else
//            var licencePath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "license.lic");
//            var fi = new FileInfo(licencePath);
//            var expDays = VerifySimple.Verify(licencePath, "<RSAKeyValue><Modulus>vqfwWBxJV1g+izDCAQeHMD3rIE7SB4wl27DogMtnRsoPavBDZb/AA+nn0wHf2DBvp2YLr/OrguZCMAhLqf4pXDgJJTJADxqkqtZWjTnrNK93xueVO4isLkoWv4NUlXJy+b9MHhQcj7R8tmXLEExZe/ouDx/jVW24ZwnUIGdcfBebZayo9N+UMVqyBsvKhfj2Gp/0qY5Ba3gdm0jzTaAbjIsW7wJPR4GCYeua4588CEgyR0SKYBiDQBsyP/qPMJVH+pf8XgC963ery5d8MElmaqx3Svu+Ntc6mVvTH1E0aol4JtEsTcivBxw+b+hL3XmvzEhLaznOfxpYBDCLVamoSQ==</Modulus><Exponent>AQAB</Exponent><P>3qVuUbYT5/WmRvD4YlrIrHLbzn545uML+91qS5be82SCOOW+wQUTu7a5zmfVgMnD1Rxc3RPFVmK2lygFx4wUBmszrJx4WjI1IaS05cMMrIkUHtRrNJQNLLT34HihET/l5y655KTZidNIRZJhU4aCN/Fa1geftlhZ6W3jHe4EUXM=</P><Q>2zer0B7kV8t+C5CPaSdEH2t14bLZsw6SqrZYiIVEkvf7AfKXTvovJ/EyI5wARonYdrT0PCEU745QFF6JobfbD0KljYHKKqojPtCs3YjVuzSwHgU9Ad8saq6vyXbxYD9Ub4JHSdNwcCScnyMy2A5v5JgntvDmu2xee8Z0hJ7fwFM=</Q><DP>EUyBOdAP0+H+PVzTr7CmDuANvAmPk0Do2XTmoWDjib2xcIJJQ+4FDnywCx1+NTd4A8LkZDj5CDE7RptRd6OmmqkRpsOxhjCIeEi31XJGNdGgwpR2j+tA6sxUxhgZS6HPVxsGToGQdWj5II6zCS5WR8p06FRHUH+k3MIw905a/4k=</DP><DQ>KDg2laCVVGZJYeXL6gcCQKF+p0IhHvD1h8ycwa9xvWUx8FGJ4TuVm0ZtZIsioc54oI7ioIWRScJafuI5bipFTq9zoFrHtLwyazvt+1c3n3kzNs7POqCvjYMvTU89SeNUooDMg4b/ghO1iJz0nx9G+DHSV4YrTwe67gqjJhHgC3E=</DQ><InverseQ>SNDKS8+q3neSbQoS67j7liWLLykborZCVh4vQh5Yeri+1BH2rrHK0E4eWXagW9G4qELEEjL68GWIU4cKc5pW+rFqyXxeco6YGpZDtO2jZcsUBL1pVMxZbkBQYDbsvHc0qjIAaEhAC1DkIy666wBuH1Qy0zXhJw74UWZpW4UqIXc=</InverseQ><D>lAmEZnX4QJjlX2CJkxEMWFoLordw/6lDkBUKQ08srtugIxHR00STTLanacEP1Sau25Uxx/p1FxvBEtiuH52Y8eEUwMwGz3OJnpj09Y2eYvdIEqqoxmQ2BW9DwIqx96S5P9DYwpYtIywqldnuVYZjCQ/WCaph82hRgbgw/MQlHLhEua3Eu/H8L3EYtDp63cnXdaH0KkNvo/5GHRJ4UA2DLZ/lX7l5HZh52MR5lOIw+0RqmvVezwvxwOCYR4q6FMlrTwnjfGkwltxeu97U5IxQfWOIVYxdY3eYTc/JFyiQ4vM7cOYCG27aGamJw1MlCgwzPOqcGSXJ/3KkfdzXy2UYnQ==</D></RSAKeyValue>");
//#endif
            // 当第一个线程运行到这里时，此时会对locker对象 "加锁"，
            // 当第二个线程运行该方法时，首先检测到locker对象为"加锁"状态，该线程就会挂起等待第一个线程解锁
            // lock语句运行完之后（即线程运行完之后）会对该对象"解锁"
            // 双重锁定只需要一句判断就可以了
            if (uniqueInstance == null)
                {
                    lock (locker)
                    {
                        // 如果类的实例不存在则创建，否则直接返回
                        if (uniqueInstance == null)
                        {
                            uniqueInstance = new NPlatformStartup();
                        }
                    }
                }
                return uniqueInstance;
        }

        /// <summary>
        /// AutoMapper初始化
        /// </summary>
        public void AutoMapperInit()
        {
            Mapper.Initialize(cfg =>
                {
                    AutoMapperInitialized = false;
                    var maps = IOCManager.ResolveAutoMapper();
                    foreach (var map in maps)
                    {
                        map.Config(cfg);
                    }
                    AutoMapperInitialized = true;
                });
        }
    }
}
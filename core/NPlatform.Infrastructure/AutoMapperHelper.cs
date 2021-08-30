#region << 版 本 注 释 >>

/*----------------------------------------------------------------
* 项目名称 ：NPlatform.Domains.AutoMapConfig
* 类 名 称 ：AutoMapHelper
* 类 描 述 ：
* 命名空间 ：NPlatform.Domains.AutoMapConfig
* CLR 版本 ：4.0.30319.42000
* 作    者 ：DongliangYi
* 创建时间 ：2018-12-13 16:36:58
* 更新时间 ：2018-12-13 16:36:58
* 版 本 号 ：v1.0.0.0
//----------------------------------------------------------------*/

#endregion << 版 本 注 释 >>

namespace NPlatform
{
    using System.Linq;

    using AutoMapper;

    /// <summary>
    /// 对象映射工具类
    /// </summary>
    public class AutoMapperHelper
    {
        /// <summary>
        /// 映射1-N个对象。
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="sources">源对象</param>
        /// <returns>map的结果对象</returns>
        public static T Map<T>(params object[] sources)
            where T : class
        {
            // If there are no sources just return the destination object
            if (!sources.Any())
            {
                return default(T);
            }

            // Get the inital source and map it
            var initialSource = sources[0];
            var mappingResult = Map<T>(initialSource);

            // Now map the remaining source objects
            if (sources.Count() > 1)
            {
                Map(mappingResult, sources.Skip(1).ToArray());
            }

            // return the destination object
            return mappingResult;
        }

        ///<summary>
        ///map 源对象到目标对象。
        ///</summary>
        ///<param name="destination">目标.</param>
        ///<param name="sources">源.</param>
        public static void Map(object destination, params object[] sources)
        {
            // If there are no sources just return the destination object
            if (!sources.Any())
            {
                return;
            }

            // Get the destination type
            var destinationType = destination.GetType();

            // Itereate through all of the sources...
            foreach (var source in sources)
            {
                if (source == null) continue;

                // ... get the source type and map the source to the destination
                var sourceType = source.GetType();
                Mapper.Map(source, destination, sourceType, destinationType);
            }
        }

        ///<summary>
        ///Maps 单个对象到目标对象。
        ///</summary>
        ///<typeparam name="T">type of teh destination</typeparam>
        ///<param name="source">The source.</param>
        ///<returns></returns>
        private static T Map<T>(object source)
            where T : class
        {
            if (source == null) return null;

            // Get thr source and destination types
            var destinationType = typeof(T);
            var sourceType = source.GetType();

            // Get the destination using AutoMapper's Map
            var mappingResult = Mapper.Map(source, sourceType, destinationType);

            // Return the destination
            return mappingResult as T;
        }
    }
}
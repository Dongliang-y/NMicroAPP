namespace NPlatform
{
    using System;
    using System.IO;

    /// <summary>
    /// 平台的Server 助手类
    /// </summary>
    public class EPServer
    {
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string MapPath(string strPath)
        {
            var rootdir =Directory.GetCurrentDirectory();
            return $"{rootdir}\\{strPath}";
        }
    }
}
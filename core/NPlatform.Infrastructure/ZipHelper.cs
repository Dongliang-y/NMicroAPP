using Ionic.Zip;
using System;
using System.Collections.Generic;

namespace NPlatform.Infrastructure
{
    public class ZipHelper
    {
        /// <summary>
        /// 获取压缩后的文件路径
        /// </summary>
        /// <param name="dirPath">压缩的文件路径</param>
        /// <param name="filesPath">多个文件路径</param>
        /// <returns>string</returns>
        public static string GetCompressPath(string dirPath, List<string> filesPath)
        {
            var zipPath = string.Empty; // 返回压缩后的文件路径
            var filePath = string.Empty;
            using (ZipFile zip = new ZipFile(System.Text.Encoding.Default)) // System.Text.Encoding.Default设置中文附件名称乱码，不设置会出现乱码
            {
                foreach (var file in filesPath)
                {
                    zip.AddFile(file, "");
                    // 第二个参数为空，说明压缩的文件不会存在多层文件夹。比如C:\test\a\b\c.doc 压缩后解压文件会出现c.doc
                    // 如果改成zip.AddFile(file);则会出现多层文件夹压缩，比如C:\test\a\b\c.doc 压缩后解压文件会出现test\a\b\c.doc
                }
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                filePath = string.Format("{0}.zip", fileName);
                zipPath = string.Format("{0}\\{1}.zip", dirPath, fileName);
                zip.Save(zipPath);
            }
            return filePath;
        }
    }
}

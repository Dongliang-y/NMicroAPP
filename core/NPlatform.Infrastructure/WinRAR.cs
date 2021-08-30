namespace NPlatform.Infrastructure
{
    using System.Diagnostics;
    using System.Text;

    /// <summary>
    /// WinRAR 操作类
    /// </summary>
    public class WinRAR
    {
        private string _cmdA = " a -k -r -s -m1 ";

        private string _cmdX = " x -k -r -s -m1 ";

        private string rarSetupPath = "\"c:\\Program Files\\WinRAR\\rar.exe\"";

        /// <summary>
        /// 构造器
        /// </summary>
        public WinRAR()
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="rarSetupPath">WinRAR路径</param>
        public WinRAR(string rarSetupPath)
        {
            RarSetupPath = rarSetupPath;
        }

        /// <summary>
        /// WinRAR路径
        /// </summary>
        public string RarSetupPath
        {
            set
            {
                rarSetupPath = InitRarSetupPath(value);
            }
        }

        /// <summary>
        /// 压缩一个或多个文件
        /// </summary>
        /// <param name="fileList">一个或多个文件</param>
        /// <param name="rarFile">压缩到指定的RAR文件中</param>
        /// <returns>是否操作成功</returns>
        /// <example>
        /// <code>
        /// WinRAR _rar = new WinRAR();
        /// WinRAR.File _rarFile = new WinRAR.File();
        /// _rarFile.Add(Server.MapPath("~/default.aspx"));
        /// _rarFile.Add(Server.MapPath("~/Register.aspx"));
        /// if (_rar.RARAFile(_rarFile.FileList, Server.MapPath("~/") + "aa.rar")) Response.Write("OK"); else Response.Write("NO") ;
        /// </code>
        /// </example>
        public bool RARAFile(string fileList, string rarFile)
        {
            return Run(rarSetupPath, _cmdA + rarFile + " " + fileList, ProcessWindowStyle.Hidden);
        }

        /// <summary>
        /// 压缩目录
        /// </summary>
        /// <param name="folderList">目录列表</param>
        /// <param name="rarFile">压缩到指定的RAR文件中</param>
        /// <returns>是否操作成功</returns>
        /// <example>
        /// <code>
        /// WinRAR _rar = new WinRAR();
        /// WinRAR.Folder _rarFolder = new WinRAR.Folder();
        /// _rarFolder.Add(Server.MapPath("~/App_Code/"));
        /// _rarFolder.Add(Server.MapPath("~/log/"));
        /// if (_rar.RARAFolder(_rarFolder.FolderList, Server.MapPath("~/") + "aaa.rar")) Response.Write("OK"); else Response.Write("NO") ;
        /// </code>
        /// </example>
        public bool RARAFolder(string folderList, string rarFile)
        {
            return Run(rarSetupPath, _cmdA + rarFile + " " + folderList, ProcessWindowStyle.Hidden);
        }

        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="rarFile">压缩RAR文件</param>
        /// <param name="fileList">压缩文件可以是*.*/*.txt或空</param>
        /// <param name="outFolder">输出到指定目录</param>
        /// <returns>是否操作成功</returns>
        /// <example>
        /// <code>
        /// WinRAR _rar = new WinRAR();
        /// WinRAR.Folder _rarFolder = new WinRAR.Folder();
        /// if (_rar.RARXFile(Server.MapPath("~/aa.rar"),"",Server.MapPath("~/") + "aa\\")) Response.Write("OK"); else Response.Write("NO") ;
        /// </code>
        /// </example>
        public bool RARXFile(string rarFile, string fileList, string outFolder)
        {
            return Run(rarSetupPath, _cmdX + rarFile + " " + fileList + " " + outFolder, ProcessWindowStyle.Hidden);
        }

        /// <example>
        /// <code>
        /// WinRAR _rar = new WinRAR();
        /// WinRAR.Folder _rarFolder = new WinRAR.Folder();
        /// if (_rar.RARXFolder(Server.MapPath("~/aaa.rar"),"",Server.MapPath("~/") + "aaa\\")) Response.Write("OK"); else Response.Write("NO") ;
        /// </code>
        /// </example>
        public bool RARXFolder(string rarFile, string folderList, string outFolder)
        {
            return Run(
                rarSetupPath,
                _cmdX + rarFile + " " + folderList + " " + outFolder,
                ProcessWindowStyle.Hidden);
        }

        /// <summary>
        /// 初始化WinRAR路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string InitRarSetupPath(string path)
        {
            if (path.Trim().Equals(string.Empty)) path = rarSetupPath;
            return path;
        }

        /// <summary>
        /// 运行指定的可执行文件
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="arguments"></param>
        /// <param name="winStyle"></param>
        /// <returns></returns>
        private bool Run(string cmd, string arguments, ProcessWindowStyle winStyle)
        {
            bool _isTrue = false;
            if (cmd.Trim().Equals(string.Empty)) return false;
            try
            {
                Process pScore = new Process();
                pScore.StartInfo.FileName = cmd;
                pScore.StartInfo.Arguments = arguments;
                pScore.StartInfo.ErrorDialog = false;
                pScore.StartInfo.UseShellExecute = true;
                pScore.StartInfo.RedirectStandardOutput = false;
                pScore.StartInfo.WindowStyle = winStyle;
                pScore.Start();
                pScore.Close();
                _isTrue = true;
            }
            catch
            {
            }

            return _isTrue;
        }

        /// <summary>
        /// 文件列表类
        /// </summary>
        public class File
        {
            private StringBuilder _sb = new StringBuilder();

            /// <summary>
            /// 返回文件列表字符串
            /// </summary>
            public string FileList
            {
                get
                {
                    return _sb.ToString();
                }
            }

            /// <summary>
            /// 添加指定的文件名要绝对路径
            /// </summary>
            /// <param name="file"></param>
            public void Add(string file)
            {
                _sb.Append(file.Trim() + " ");
            }
        }

        /// <summary>
        /// 目录列表类
        /// </summary>
        public class Folder
        {
            private StringBuilder _sb = new StringBuilder();

            /// <summary>
            /// 返回目录列表字符串
            /// </summary>
            public string FolderList
            {
                get
                {
                    return _sb.ToString();
                }
            }

            /// <summary>
            /// 添加指定的目录名要绝对路径
            /// </summary>
            /// <param name="folder"></param>
            public void Add(string folder)
            {
                _sb.Append(folder.Trim() + " ");
            }
        }
    }
}
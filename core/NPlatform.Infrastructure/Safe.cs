namespace NPlatform.Infrastructure
{
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// 安全操作类
    /// </summary>
    public class Safe
    {
        /// <summary>
        /// 杀死进程
        /// </summary>
        /// <param name="processName">进程名</param>
        public static void DoKill(string processName)
        {
            System.Diagnostics.Process myproc = new System.Diagnostics.Process();
            System.Diagnostics.Process[]
                procs = System.Diagnostics.Process.GetProcessesByName(processName); // '得到所有打开的进程
            try
            {
                foreach (System.Diagnostics.Process proc in procs)
                {
                    if (!proc.CloseMainWindow())
                    {
                        proc.Kill();
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 特殊字符过滤
        /// </summary>
        /// <param name="strchar"></param>
        /// <returns></returns>
        public static string FilterBadChar(string strchar)
        {
            string input = "";
            if (string.IsNullOrEmpty(strchar))
            {
                return "";
            }

            string str = strchar;
            string[] strArray = new string[]
                                    {
                                        "+", "'", "%", "^", "&", "?", "(", ")", "<", ">", "[", "]", "{", "}", "/", "\"",
                                        ";", ":", "Chr(34)", "Chr(0)", "--"
                                    };
            StringBuilder builder = new StringBuilder(str);
            for (int i = 0; i < strArray.Length; i++)
            {
                input = builder.Replace(strArray[i], "").ToString();
            }

            return Regex.Replace(input, "@+", "@");
        }

        /// <summary>
        /// 反格式化字符串
        /// </summary>
        /// <param name="str">要反格式化的字符串</param>
        /// <returns>反格式化完成的字符串</returns>
        public static string ShowHtml(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("<", "& lt;");
                str = str.Replace(">", "& gt;");
                str = str.Replace(" ", "& nbsp;");
                str = str.Replace("'", "& #39;");
                str = str.Replace("\"", "& quot;");
                str = str.Replace("\r\n", "<br>");
                str = str.Replace("\n", "<br>");
            }

            str = new Regex("<script", RegexOptions.IgnoreCase).Replace(str, "<_script");
            str = new Regex("<object", RegexOptions.IgnoreCase).Replace(str, "<_object");
            str = new Regex("javascript:", RegexOptions.IgnoreCase).Replace(str, "_javascript:");
            str = new Regex("vbscript:", RegexOptions.IgnoreCase).Replace(str, "_vbscript:");
            str = new Regex("expression", RegexOptions.IgnoreCase).Replace(str, "_expression");
            str = new Regex("@import", RegexOptions.IgnoreCase).Replace(str, "_@import");
            str = new Regex("<iframe", RegexOptions.IgnoreCase).Replace(str, "<_iframe");
            str = new Regex("<frameset", RegexOptions.IgnoreCase).Replace(str, "<_frameset");
            str = new Regex(@" (on[a-zA-Z ]+)=", RegexOptions.IgnoreCase).Replace(str, " _$1=");
            return str;
        }

        /// <summary>
        /// SQL注入过滤
        /// </summary>
        /// <param name="strchar"></param>
        /// <returns></returns>
        public static string SqlFilterKeyword(string strchar)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(strchar))
            {
                return string.Empty;
            }

            strchar = strchar.ToLower();
            string[] strArray = new string[]
                                    {
                                        "select", "update", "insert", "delete", "declare", "@", "exec", "dbcc", "alter",
                                        "drop", "create", "backup", "if", "else", "end", "and", "or", "add", "set",
                                        "open", "close", "use", "begin", "retun", "as", "go", "exists", "kill", "%",
                                        "chr("
                                    };
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strchar.Contains(strArray[i]))
                {
                    strchar = strchar.Replace(strArray[i], "");
                    flag = true;
                }
            }

            if (flag)
            {
                return SqlFilterKeyword(strchar);
            }

            return strchar;
        }

        /// <summary>
        /// 去掉特殊字符--用于台账，规则等导入
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>string</returns>
        public static string RepStr(string str)
        {
            string newStr = str.Replace("\n", "");
            newStr = newStr.Replace("\r", "");
            newStr = newStr.Replace("\r\n", "");
            newStr = newStr.Replace("'", "’");
            newStr = newStr.Replace("\"", "”");
            newStr = newStr.Replace("（", "(");
            newStr = newStr.Replace("）", ")");
            return newStr;
        }

        /// <summary>
        /// 去掉特殊字符--用于帆软报表输出
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>string</returns>
        public static string RepReportStr(string str)
        {
            string newStr = str.Replace("\n", "&at;");
            newStr = newStr.Replace("\r", "&at;");
            newStr = newStr.Replace("\r\n", "&at;");
            newStr = newStr.Replace("\t", "&at;");
            return newStr;
        }
    }
}
/***********************************************************
**项目名称:	                                     
**类    名:	                                 				   
**功能描述:	                                       					   
**作    者: 	易栋梁                                         			   
**版 本 号:	1.0                                             			   
**创建日期：2015/3/21 
**修改历史：
************************************************************/

namespace NPlatform
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// 字符串操作类
    /// </summary>
    public static class StrExtend
    {
        /// <summary>
        /// 根据阿拉伯数字返回月份的名称(可更改为某种语言)
        /// </summary>	
        public static string[] Monthes
        {
            get
            {
                return new string[]
                           {
                               "January", "February", "March", "April", "May", "June", "July", "August", "September",
                               "October", "November", "December"
                           };
            }
        }

        /// <summary>
        /// 清理字符串
        /// </summary>
        public static string CleanInput(this string strIn)
        {
            return Regex.Replace(strIn.Trim(), @"[^\w\.@-]", "");
        }

        /// <summary>
        /// 清除给定字符串中的回车及换行符
        /// </summary>
        /// <param name="str">要清除的字符串</param>
        /// <returns>清除后返回的字符串</returns>
        public static string ClearBR(this string str)
        {
            Regex r = null;
            Match m = null;
            r = new Regex(@"(\r\n)", RegexOptions.IgnoreCase);
            for (m = r.Match(str); m.Success; m = m.NextMatch())
                str = str.Replace(m.Groups[0].ToString(), "");
            return str;
        }

        /// <summary>
        /// 返回中文字符的长度
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回中文字符的长度</returns>
        public static int CnLength(this string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }

        /// <summary>
        /// 截取多少行的字符串
        /// </summary>
        /// <param name="strInput">内容</param>
        /// <param name="intlen">取多少行</param>
        /// <param name="flg">在尾部加上字符串</param>
        /// <returns>多少行的字符串</returns>
        public static string CutStr(this string strInput, int intlen, string flg) //截取字符串
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            int intLength = 0;
            string strString = "";
            byte[] s = ascii.GetBytes(strInput);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    intLength += 2;
                }
                else
                {
                    intLength += 1;
                }

                try
                {
                    strString += strInput.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (intLength > intlen)
                {
                    break;
                }
            }

            byte[] mybyte = Encoding.Default.GetBytes(strInput);
            if (mybyte.Length > intlen)
            {
                strString += flg;
            } //如果截过则加上半个字符串

            return strString;
        }

        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        public static string CutString(this string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else startIndex = startIndex - length;
                }

                if (startIndex > str.Length) return "";
            }
            else
            {
                if (length < 0) return "";
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else return "";
                }
            }

            if (str.Length - startIndex < length) length = str.Length - startIndex;
            try
            {
                return str.Substring(startIndex, length);
            }
            catch
            {
                return str;
            }
        }

        /// <summary>
        /// 从字符串的指定位置开始截取到字符串结尾的了符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <returns>子字符串</returns>
        public static string CutString(this string str, int startIndex)
        {
            return CutString(str, startIndex, str.Length);
        }

        /// <summary>
        /// 获得Assembly产品版权
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyCopyright()
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            FileVersionInfo myFileVersion = FileVersionInfo.GetVersionInfo(myAssembly.Location);
            return myFileVersion.LegalCopyright;
        }

        /// <summary>
        /// 获得Assembly产品名称
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyProductName()
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            FileVersionInfo myFileVersion = FileVersionInfo.GetVersionInfo(myAssembly.Location);
            return myFileVersion.ProductName;
        }

        /// <summary>
        /// 获得Assembly版本号
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyVersion()
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            FileVersionInfo myFileVersion = FileVersionInfo.GetVersionInfo(myAssembly.Location);
            return string.Format(
                "{0}.{1}.{2}",
                myFileVersion.FileMajorPart,
                myFileVersion.FileMinorPart,
                myFileVersion.FileBuildPart);
        }

        /// <summary>
        /// 取文件扩展名
        /// </summary>
        /// <param name="filename">文件URL</param>
        /// <returns>文件扩展名</returns>
        public static string GetFileExtends(this string filename)
        {
            string ext = null;
            if (filename.IndexOf('.') > 0)
            {
                string[] fs = filename.Split('.');
                ext = fs[fs.Length - 1];
            }

            return ext;
        }

        /// <summary>
        /// 获取页面的链接正则 GetHref(HtmlCode);
        /// </summary>
        /// <param name="HtmlCode"></param>
        /// <returns></returns>
        public static string GetHref(this string HtmlCode)
        {
            string MatchVale = "";
            string Reg = @"(h|H)(r|R)(e|E)(f|F) *= *('|"")?((\w|\\|\/|\.|:|-|_)+)('|""| *|>)?";
            foreach (Match m in Regex.Matches(HtmlCode, Reg))
            {
                MatchVale += (m.Value).ToLower().Replace("href=", "").Trim() + "||";
            }

            return MatchVale;
        }

        /// <summary>
        /// 匹配&lt;img src="" />中的图片路径实际链接
        /// </summary>
        /// <param name="ImgString">Html字符串</param>
        /// <param name="imgHttp">前面URL</param>
        /// <returns></returns>
        public static string GetImg(this string ImgString, string imgHttp)
        {
            string MatchVale = "";
            string Reg = @"src=.+\.(bmp|jpg|gif|png|)";
            foreach (Match m in Regex.Matches(ImgString.ToLower(), Reg))
            {
                MatchVale += (m.Value).ToLower().Trim().Replace("src=", "");
            }

            return (imgHttp + MatchVale);
        }

        /// <summary>
        /// 匹配页面的图片地址 GetImgSrc(HtmlCode,"http://www.baidu.com/");当比如:&lt;img src="bb/x.gif">则要补充http://www.baidu.com/,当包含http信息时,则可以为空
        /// </summary>
        /// <param name="HtmlCode"></param>
        /// <param name="imgHttp">要补充的http://路径信息</param>
        /// <returns></returns>
        public static string GetImgSrc(this string HtmlCode, string imgHttp)
        {
            string MatchVale = "";
            string Reg = @"<img.+?>";
            foreach (Match m in Regex.Matches(HtmlCode, Reg))
            {
                MatchVale += GetImg((m.Value).ToLower().Trim(), imgHttp) + "|";
            }

            return MatchVale.Replace("\"", "");
        }

        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>
        public static int GetInArrayID(this string strSearch, string[] stringArray, bool caseInsensetive)
        {
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (caseInsensetive)
                {
                    if (strSearch.ToLower() == stringArray[i].ToLower()) return i;
                }
                else
                {
                    if (strSearch == stringArray[i]) return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>		
        public static int GetInArrayID(this string strSearch, string[] stringArray)
        {
            return GetInArrayID(strSearch, stringArray, true);
        }

        /// <summary>
        /// 执行正则提取出值 GetRegValue("<title>.+?</title>",HtmlCode)
        /// </summary>
        /// <param name="RegexString">正则表达式</param>
        /// <param name="HtmlCode">HTML代码</param>
        /// <returns></returns>
        public static string GetRegexValue(this string HtmlCode, string RegexString)
        {
            string MatchVale = "";
            Regex r = new Regex(RegexString);
            Match m = r.Match(HtmlCode);
            if (m.Success)
            {
                MatchVale = m.Value;
            }

            return MatchVale;
        }

        /// <summary>
        /// 截取指定长度的字符串
        /// </summary>
        /// <param name="str">要截取的字符串</param>
        /// <param name="length">字符串长度</param>
        /// <param name="flg">在尾部加上字符串</param>
        /// <returns>指定长度的字符串</returns>
        public static string GetStr(this string str, int length, string flg)
        {
            int i = 0, j = 0;
            foreach (char chr in str)
            {
                if ((int)chr > 127)
                {
                    i += 2;
                }
                else
                {
                    i++;
                }

                if (i > length)
                {
                    str = str.Substring(0, j);
                    str += flg;
                    break;
                }

                j++;
            }

            return str;
        }

        /// <summary>
        /// 字符截取
        /// </summary>
        public static string GetSubString(object obj, int pLength, string pTailString)
        {
            if (obj == null) return "";
            return obj.ToString().GetSubString(pLength, pTailString);
        }

        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="pLength">指定长度</param>
        /// <param name="pTailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(this string p_SrcString, int pLength, string pTailString)
        {
            string myResult = p_SrcString;
            if (pLength >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);
                if (bsSrcString.Length > pLength)
                {
                    int nRealLength = pLength;
                    int[] anResultFlag = new int[pLength];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = 0; i < pLength; i++)
                    {
                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3) nFlag = 1;
                        }
                        else nFlag = 0;

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[pLength - 1] > 127) && (anResultFlag[pLength - 1] == 1))
                        nRealLength = pLength + 1;
                    bsResult = new byte[nRealLength];
                    Array.Copy(bsSrcString, bsResult, nRealLength);
                    myResult = Encoding.Default.GetString(bsResult);
                    myResult = myResult + pTailString;
                }
            }

            return myResult;
        }

        /// <summary>
        /// 返回URL中结尾的文件名
        /// </summary>		
        public static string GetUrlFileName(this string url)
        {
            if (url == null) return "";
            string[] strs1 = url.Split(new char[] { '/' });
            return strs1[strs1.Length - 1].Split(new char[] { '?' })[0];
        }

        /// <summary>
        /// 是日期否
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>是/否</returns>
        public static bool IsDateTime(this string input)
        {
            //string pet = @"^(?:(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(\/|-|\.)(?:0?2\1(?:29))$)|(?:(?:1[6-9]|[2-9]\d)?\d{2})(\/|-|\.)(?:(?:(?:0?[13578]|1[02])\2(?:31))|(?:(?:0?[1,3-9]|1[0-2])\2(29|30))|(?:(?:0?[1-9])|(?:1[0-2]))\2(?:0?[1-9]|1\d|2[0-8]))$";
            string pet =
                @"^(?=\d)(?:(?!(?:1582(?:\.|-|\/)10(?:\.|-|\/)(?:0?[5-9]|1[0-4]))|(?:1752(?:\.|-|\/)0?9(?:\.|-|\/)(?:0?[3-9]|1[0-3])))(?=(?:(?!000[04]|(?:(?:1[^0-6]|[2468][^048]|[3579][^26])00))(?:(?:\d\d)(?:[02468][048]|[13579][26]))\D0?2\D29)|(?:\d{4}\D(?!(?:0?[2469]|11)\D31)(?!0?2(?:\.|-|\/)(?:29|30))))(\d{4})([-\/.])(0?\d|1[012])\2((?!00)[012]?\d|3[01])(?:$|(?=\x20\d)\x20))?((?:(?:0?[1-9]|1[012])(?::[0-5]\d){0,2}(?:\x20[aApP][mM]))|(?:[01]?\d|2[0-3])(?::[0-5]\d){1,2})?$";
            return input.IsMatch(pet);
        }

        /// <summary>
        /// 是Email否
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>是/否</returns>
        public static bool IsEmail(this string input)
        {
            string pet = @"^\w+((-\w+)|(\.\w+))*\@\w+((\.|-)\w+)*\.\w+$";
            return input.IsMatch(pet);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool IsInArray(this string strSearch, string[] stringArray, bool caseInsensetive)
        {
            return GetInArrayID(strSearch, stringArray, caseInsensetive) >= 0;
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">字符串数组</param>
        /// <returns>判断结果</returns>
        public static bool IsInArray(this string str, string[] stringarray)
        {
            return IsInArray(str, stringarray, false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsInArray(this string str, string stringarray)
        {
            return IsInArray(str, stringarray.Split(','), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <param name="strsplit">分割字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsInArray(this string str, string stringarray, char strsplit)
        {
            return IsInArray(str, stringarray.Split(strsplit), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <param name="strsplit">分割字符串</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool IsInArray(this string str, string stringarray, char strsplit, bool caseInsensetive)
        {
            return IsInArray(str, stringarray.Split(strsplit), caseInsensetive);
        }

        /// <summary>
        /// 返回指定IP是否在指定的IP数组所限定的范围内, IP数组内的IP地址可以使用*表示该IP段任意, 例如192.168.1.*
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="iparray"></param>
        /// <returns></returns>
        public static bool IsInIPArray(this string ip, string[] iparray)
        {
            string[] userip = ip.Split('.');
            for (int ipIndex = 0; ipIndex < iparray.Length; ipIndex++)
            {
                string[] tmpip = iparray[ipIndex].Split('.');
                int r = 0;
                for (int i = 0; i < tmpip.Length; i++)
                {
                    if (tmpip[i] == "*") return true;
                    if (userip.Length > i)
                    {
                        if (tmpip[i] == userip[i]) r++;
                        else break;
                    }
                    else break;
                }

                if (r == 4) return true;
            }

            return false;
        }

        /// <summary>
        /// 是整数否 非0的整数
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>是/否</returns>
        public static bool IsInt(this string input)
        {
            string pet = @"^[0-9]*[1-9][0-9]*$"; //@"^\d{1,}$"//整数校验常量//@"^-?(0|\d+)(\.\d+)?$"//数值校验常量 
            return input.IsMatch(pet);
        }

        /// <summary>
        /// 是IP类型否
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>是/否</returns>
        public static bool IsIp(this string input)
        {
            string pet = @"^(([01]?[\d]{1,2})|(2[0-4][\d])|(25[0-5]))(\.(([01]?[\d]{1,2})|(2[0-4][\d])|(25[0-5]))){3}$";
            return input.IsMatch(pet);
        }

        /// <summary>
        /// 校验登录名：只能输入4-20个以字母开头、可带数字、“_”、“.”的字串
        /// 使用方式：“用户名字符串”.IsLoginName();
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>是/否</returns>
        public static bool IsLoginName(this string input)
        {
            string pet = @"^[\@A-Za-z0-9\!\#\$\%\^\&\*\.\~]{4,22}$";
            return input.IsMatch(pet);
        }

        /// <summary>
        /// 是否匹配正则表达式
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pattern">正则表达式</param>
        /// <returns>完全匹配返回真</returns>
        public static bool IsMatch(this string str, string pattern)
        {
            if (string.IsNullOrEmpty(str)) return false;
            Regex re = new Regex(pattern, RegexOptions.IgnoreCase);
            return re.IsMatch(str);
        }

        /// <summary>
        /// 手机号 + - 
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>是/否</returns>
        public static bool IsMobile(this string input)
        {
            string pet = @"^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$";
            return input.IsMatch(pet);
        }

        /// <summary>
        /// 判断字符是否为null或者string.Empty
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 判断给定的字符串(strNumber)是否是数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumber(this string strNumber)
        {
            string pet = @"^(\-|\+)?\d+(\.\d+)?$";
            return strNumber.IsMatch(pet);
        }

        /// <summary>
        /// 校验密码：只能输入8-20个字母+数字，字母+特殊字符，数字+特殊字符,三者都有
        /// 使用方式：“用户名字符串”.IsPassword();
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>是/否</returns>
        public static bool IsPassword(this string input)
        {
            if (input.Length < 8 || input.Length > 20) return false;
            string pattern1 = @"^(?![a-zA-z]+$)(?!\d+$)(?![!@#$%^&*]+$)[a-zA-Z\d!@#$%^&*().]+$";
            return input.IsMatch(pattern1);
        }

        /// <summary>
        /// 是身份证否
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>是/否</returns>
        public static bool IsSSN(this string input)
        {
            string pet = @"\d{18}|\d{15}";
            return input.IsMatch(pet);
        }

        /// <summary>
        /// 电话号码 + -
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>是/否</returns>
        public static bool IsTelepone(this string input)
        {
            string pet = @"^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$"; //："^(\(\d{3,4}-)|\d{3.4}-)?\d{7,8}$
            return input.IsMatch(pet);
        }

        /// <summary>
        /// 是Url否
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>是/否</returns>
        public static bool IsUrl(this string input)
        {
            string pet = @"^http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
            return input.IsMatch(pet);
        }

        /// <summary>
        /// 是否为正常的Guid，非Guid.Empty
        /// </summary>
        /// <returns></returns>
        public static bool IsUsableGuid(this string str)
        {
            Guid guid;
            if (Guid.TryParse(str, out guid))
            {
                return guid != Guid.Empty;
            }

            return false;
        }
        /// <summary>
        /// 是否为年份
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidYear(this string input)
        {
            return Regex.IsMatch(input, @"^(19\d\d)|(200\d)$");
        }

        /// <summary>
        /// 替换HTML源代码
        /// </summary>
        /// <param name="HtmlCode">html源代码</param>
        /// <returns></returns>
        public static string RemoveHTML(this string HtmlCode)
        {
            string MatchVale = HtmlCode;
            MatchVale = new Regex("<br>", RegexOptions.IgnoreCase).Replace(MatchVale, "\n");
            foreach (Match s in Regex.Matches(HtmlCode, "<[^{><}]*>"))
            {
                MatchVale = MatchVale.Replace(s.Value, "");
            } //"(<[^{><}]*>)"//@"<[\s\S-! ]*?>"//"<.+?>"//<(.*)>.*<\/\1>|<(.*) \/>//<[^>]+>//<(.|\n)+?>

            MatchVale = new Regex("\n", RegexOptions.IgnoreCase).Replace(MatchVale, "<br>");
            return MatchVale;
        }

        /// <summary>
        /// 替换网页中的换行和引号
        /// </summary>
        /// <param name="HtmlCode">HTML源代码</param>
        /// <returns></returns>
        public static string ReplaceEnter(this string HtmlCode)
        {
            string s = "";
            if (HtmlCode == null || HtmlCode == "") s = "";
            else s = HtmlCode.Replace("\"", "");
            s = s.Replace("\r\n", "");
            return s;
        }

        /// <summary>
        /// 自定义的替换字符串函数
        /// </summary>
        public static string ReplaceString(
            this string SourceString,
            string SearchString,
            string ReplaceString,
            bool IsCaseInsensetive)
        {
            return Regex.Replace(
                SourceString,
                Regex.Escape(SearchString),
                ReplaceString,
                IsCaseInsensetive ? RegexOptions.IgnoreCase : RegexOptions.None);
        }

        /// <summary>
        /// 颠倒字符串次序
        /// </summary>
        /// <param name="_str">字符串</param>
        /// <returns>颠倒字符串次序</returns>
        public static string Reverse(this string _str)
        {
            if (_str.Length <= 1) return _str;
            char[] c = _str.ToCharArray();
            StringBuilder sb = new StringBuilder(c.Length);
            for (int i = c.Length - 1; i > -1; i--) sb.Append(c[i]);
            return sb.ToString();
        }

        /// <summary>
        /// 删除字符串尾部的回车/换行/空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RTrim(this string str)
        {
            return str.TrimEnd(new char[] { ' ', '\r', '\n' });
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ToFloat(this object strValue, float defValue = 0)
        {
            if ((strValue == null) || (strValue.ToString().Length > 10))
                return defValue;

            string val = strValue.ToString();
            float rst = defValue;
            if (float.TryParse(val, out rst))
                return rst;
            else
                return defValue;
        }

        /// <summary>
        /// string型转换为int型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ToInt(this object strValue, int defValue = 0)
        {
            if ((strValue == null) || (strValue.ToString() == string.Empty))
                return defValue;
            string val = strValue.ToString();
            int rst = defValue;
            if (int.TryParse(val, out rst))
                return rst;
            else
                return defValue;
        }
        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool ToBool(this object strValue, bool defValue = false)
        {
            if ((strValue == null) || (strValue.ToString() == string.Empty))
                return defValue;
            string val = strValue.ToString();
            bool rst = defValue;
            if (bool.TryParse(val, out rst))
                return rst;
            else
                return defValue;
        }
        /// <summary>
        /// Int 转 char  ，小写的 a~z 
        /// </summary>
        /// <param name="acciiInt"></param>
        /// <returns></returns>
        public static string ToLowerChar(this int acciiInt)
        {
            var tmp = acciiInt % 26;
            string c = char.ConvertFromUtf32(97 + tmp);
            return c;
        }

        /// <summary>
        ///  如果对象为空，则返回string.Empty
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static String ToStrNoNull(this object val)
        {
            return val == null ? string.Empty : val.ToString().Trim();
        }

        /// <summary>
        ///  移除空格，如果字符为null则返回string.empty
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static String TrimNull(this string val)
        {
            return val == null ? string.Empty : val.Trim();
        }

        //0000 0000-0000 007F - 0xxxxxxx  (ascii converts to 1 octet!)
        //0000 0080-0000 07FF - 110xxxxx 10xxxxxx    ( 2 octet format)
        //0000 0800-0000 FFFF - 1110xxxx 10xxxxxx 10xxxxxx (3 octet format)
        /// <summary>
        /// 判断文件流是否为UTF8字符集
        /// </summary>
        /// <param name="sbInputStream">文件流</param>
        /// <returns>判断结果</returns>
        private static bool IsUTF8(this FileStream sbInputStream)
        {
            int i;
            byte cOctets; // octets to go in this UTF-8 encoded character 
            byte chr;
            bool bAllAscii = true;
            long iLen = sbInputStream.Length;
            cOctets = 0;
            for (i = 0; i < iLen; i++)
            {
                chr = (byte)sbInputStream.ReadByte();
                if ((chr & 0x80) != 0) bAllAscii = false;
                if (cOctets == 0)
                {
                    if (chr >= 0x80)
                    {
                        do
                        {
                            chr <<= 1;
                            cOctets++;
                        }
                        while ((chr & 0x80) != 0);

                        cOctets--;
                        if (cOctets == 0) return false;
                    }
                }
                else
                {
                    if ((chr & 0xC0) != 0x80) return false;
                    cOctets--;
                }
            }

            if (cOctets > 0) return false;
            if (bAllAscii) return false;
            return true;
        }

        //      #region ToSChinese/ToTChinese
        //      /// <summary>
        ///// 转换为简体中文
        ///// </summary>
        //public static string ToSChinese(this string str)
        //{
        //	return Strings.StrConv(str, VbStrConv.SimplifiedChinese, 0) ;
        //}

        ///// <summary>
        ///// 转换为繁体中文
        ///// </summary>
        //public static string ToTChinese(this string str)
        //{
        //	return Strings.StrConv(str, VbStrConv.TraditionalChinese, 0);
        //      }
        //      #endregion
    }
}
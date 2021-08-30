using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Web;
using System.Text.RegularExpressions;

namespace NPlatform.Infrastructure
{
    /// <summary>
    /// 表达式解析 ，用于ZF02生成报表
    /// </summary>
    public enum EnumFormula
    {
        Add,//加号
        Dec,//减号
        Mul,//乘号
        Div,//除号
        Sin,//正玄
        Cos,//余玄
        Tan,//正切
        ATan,//余切
        Sqrt,//平方根
        Pow,//求幂
        None,//无
        Round,//返回某个数字按指定位数舍入后的数字。 
        Int,//返回实数舍入后的整数值。
        Exp,//返回 e 的 n 次幂。 2.71828182常数 e 等于845904，是自然对数的底数。 
        Abs,//返回参数的绝对值，参数绝对值是参数去掉正负号后的数值。
        Log10,//返回以 10 为底的对数。 
        Ln,//返回一个数的自然对数。自然对数以常数项 e（2.71828182845904）为底。 
    }

    /// <summary>
    /// FormulaDeal
    /// </summary>
    public class FormulaDeal
    {
        /// <summary>
        /// 计算表达式
        /// </summary>
        /// <param name="strExpression"></param>
        /// <returns></returns>
        private static decimal CalculateExpress(string strExpression)
        {
            string strTemp = "";
            string strTempB = "";
            string strOne = "";
            string strTwo = "";
            decimal ReplaceValue = 0;
            strExpression = strExpression.Replace(" ", string.Empty).Trim().Replace("+-", "-").Replace("--", "+").Replace("-+", "+").Replace("++", "+");
            if (strExpression.StartsWith("-"))
            {
                strExpression = "0" + strExpression;
            }

            while (strExpression.IndexOf("+") != -1 || strExpression.IndexOf("-") != -1
            || strExpression.IndexOf("*") != -1 || strExpression.IndexOf("/") != -1)
            {
                //判断负号出现的次数，如果只有一个且负号出现在第一个，例如-1，直接返回-1
                var temp = strExpression.Split('-');
                if (temp.Length == 2 && strExpression.IndexOf("-") == 0)
                {
                    if (temp[1].IndexOf("-") == -1 && temp[1].IndexOf("-") == -1 && temp[1].IndexOf("-") == -1 && temp[1].IndexOf("-") == -1)
                    {
                        return Convert.ToDecimal(strExpression);
                    }
                }
                if (strExpression.IndexOf("*") != -1)
                {
                    strTemp = strExpression.Substring(strExpression.IndexOf("*") + 1, strExpression.Length - strExpression.IndexOf("*") - 1);
                    strTempB = strExpression.Substring(0, strExpression.IndexOf("*"));
                    strOne = strTempB.Substring(GetPrivorPos(strTempB) + 1, strTempB.Length - GetPrivorPos(strTempB) - 1);

                    strTwo = strTemp.Substring(0, GetNextPos(strTemp));
                    ReplaceValue = Convert.ToDecimal(GetExpType(strOne)) * Convert.ToDecimal(GetExpType(strTwo));
                    strExpression = strExpression.Replace(strOne + "*" + strTwo, ReplaceValue.ToString());
                }
                else if (strExpression.IndexOf("/") != -1)
                {
                    strTemp = strExpression.Substring(strExpression.IndexOf("/") + 1, strExpression.Length - strExpression.IndexOf("/") - 1);
                    strTempB = strExpression.Substring(0, strExpression.IndexOf("/"));
                    strOne = strTempB.Substring(GetPrivorPos(strTempB) + 1, strTempB.Length - GetPrivorPos(strTempB) - 1);


                    strTwo = strTemp.Substring(0, GetNextPos(strTemp));
                    ReplaceValue = Convert.ToDecimal(GetExpType(strOne)) / Convert.ToDecimal(GetExpType(strTwo));
                    strExpression = strExpression.Replace(strOne + "/" + strTwo, ReplaceValue.ToString());
                }
                else if (strExpression.IndexOf("+") != -1)
                {
                    strTemp = strExpression.Substring(strExpression.IndexOf("+") + 1, strExpression.Length - strExpression.IndexOf("+") - 1);
                    strTempB = strExpression.Substring(0, strExpression.IndexOf("+"));
                    strOne = strTempB.Substring(GetPrivorPos(strTempB) + 1, strTempB.Length - GetPrivorPos(strTempB) - 1);

                    strTwo = strTemp.Substring(0, GetNextPos(strTemp));
                    ReplaceValue = Convert.ToDecimal(GetExpType(strOne)) + Convert.ToDecimal(GetExpType(strTwo));
                    strExpression = strExpression.Replace(strOne + "+" + strTwo, ReplaceValue.ToString());
                }
                else if (strExpression.IndexOf("-") != -1)
                {
                    if (strExpression.IndexOf("-") == 0)  // 开始位置是负号
                    {
                        // 下一位置没有负号，直接返回，因为运算到最后只会出现负号，不会有其它符号
                        if (strExpression.IndexOf("-", 1) != -1)
                        {
                            strTemp = strExpression.Substring(strExpression.IndexOf("-", 1) + 1, strExpression.Length - strExpression.IndexOf("-", 1) - 1);
                            strTempB = strExpression.Substring(0, strExpression.IndexOf("-", 1));
                            strOne = strTempB;// strTempB.Substring(GetPrivorPos(strTempB) + 1, strTempB.Length - GetPrivorPos(strTempB) - 1);

                            strTwo = strTemp.Substring(0, GetNextPos(strTemp));
                            ReplaceValue = Convert.ToDecimal(GetExpType(strOne)) - Convert.ToDecimal(GetExpType(strTwo));
                            strExpression = strExpression.Replace(strOne + "-" + strTwo, ReplaceValue.ToString());
                        }
                        else
                        {
                            return Convert.ToDecimal(strExpression);
                        }
                    }
                    else
                    {
                        strTemp = strExpression.Substring(strExpression.IndexOf("-") + 1, strExpression.Length - strExpression.IndexOf("-") - 1);
                        strTempB = strExpression.Substring(0, strExpression.IndexOf("-"));
                        strOne = strTempB.Substring(GetPrivorPos(strTempB) + 1, strTempB.Length - GetPrivorPos(strTempB) - 1);


                        strTwo = strTemp.Substring(0, GetNextPos(strTemp));
                        ReplaceValue = Convert.ToDecimal(GetExpType(strOne)) - Convert.ToDecimal(GetExpType(strTwo));
                        strExpression = strExpression.Replace(strOne + "-" + strTwo, ReplaceValue.ToString());
                    }
                }
            }
            return Convert.ToDecimal(strExpression);
        }
        /// <summary>
        /// 计算含数学函数的表达式
        /// </summary>
        /// <param name="strExpression"></param>
        /// <param name="expressType"></param>
        /// <returns></returns>
        private static double CalculateExExpress(string strExpression, EnumFormula expressType)
        {
            double retValue = 0;
            switch (expressType)
            {
                case EnumFormula.Sin:
                    retValue = Math.Sin(Convert.ToDouble(strExpression));
                    break;
                case EnumFormula.Cos:
                    retValue = Math.Cos(Convert.ToDouble(strExpression));
                    break;
                case EnumFormula.Tan:
                    retValue = Math.Tan(Convert.ToDouble(strExpression));
                    break;
                case EnumFormula.ATan:
                    retValue = Math.Atan(Convert.ToDouble(strExpression));
                    break;
                case EnumFormula.Sqrt:
                    retValue = Math.Sqrt(Convert.ToDouble(strExpression));
                    break;
                case EnumFormula.Pow:
                    retValue = Math.Pow(Convert.ToDouble(strExpression), 2);
                    break;
                case EnumFormula.Round:
                    retValue = Math.Round(Convert.ToDouble(strExpression), 2);
                    break;
                case EnumFormula.Int:
                    retValue = Convert.ToInt32(Math.Round(Convert.ToDecimal(strExpression), 2));
                    break;
                case EnumFormula.Exp:
                    retValue = Math.Exp(Convert.ToDouble(strExpression));
                    break;
                case EnumFormula.Abs:
                    retValue = Math.Abs(Convert.ToDouble(strExpression));
                    break;
                case EnumFormula.Log10:
                    retValue = Math.Log10(Convert.ToDouble(strExpression));
                    break;
                case EnumFormula.Ln:
                    retValue = Math.Log(Convert.ToDouble(strExpression), Math.E);
                    break;
            }
            if (retValue == 0) return Convert.ToDouble(strExpression);
            return retValue;
        }

        /// <summary>
        /// 获取操作符位置
        /// </summary>
        /// <param name="strExpression"></param>
        /// <returns></returns>
        private static int GetNextPos(string strExpression)
        {
            int[] ExpPos = new int[4];
            ExpPos[0] = strExpression.IndexOf("+");
            ExpPos[1] = strExpression.IndexOf("-");
            ExpPos[2] = strExpression.IndexOf("*");
            ExpPos[3] = strExpression.IndexOf("/");
            int tmpMin = strExpression.Length;
            for (int count = 1; count <= ExpPos.Length; count++)
            {
                if (tmpMin > ExpPos[count - 1] && ExpPos[count - 1] != -1)
                {
                    tmpMin = ExpPos[count - 1];
                }
            }
            return tmpMin;
        }


        private static int GetPrivorPos(string strExpression)
        {
            int[] ExpPos = new int[4];
            ExpPos[0] = strExpression.LastIndexOf("+");
            ExpPos[1] = strExpression.LastIndexOf("-");
            ExpPos[2] = strExpression.LastIndexOf("*");
            ExpPos[3] = strExpression.LastIndexOf("/");
            int tmpMax = -1;
            for (int count = 1; count <= ExpPos.Length; count++)
            {
                if (tmpMax < ExpPos[count - 1] && ExpPos[count - 1] != -1)
                {
                    tmpMax = ExpPos[count - 1];
                }
            }
            return tmpMax;

        }
        /// <summary>
        /// 分割表达式并计算
        /// </summary>
        /// <param name="strExpression"></param>
        /// <returns></returns>
        public static string SpiltExpression(string strExpression)
        {
            string pi = "3.141593";
            strExpression = strExpression.ToLower().Replace("pi()", pi);

            string strTemp = "";
            string strExp = "";
            while (strExpression.IndexOf("(") != -1)
            {
                strTemp = strExpression.Substring(strExpression.LastIndexOf("(") + 1, strExpression.Length - strExpression.LastIndexOf("(") - 1);
                strExp = strTemp.Substring(0, strTemp.IndexOf(")"));
                strExpression = strExpression.Replace("(" + strExp + ")", CalculateExpress(strExp).ToString());
            }
            if (strExpression.IndexOf("+") != -1 || strExpression.IndexOf("-") != -1
            || strExpression.IndexOf("*") != -1 || strExpression.IndexOf("/") != -1)
            {
                strExpression = CalculateExpress(strExpression).ToString();
            }
            return strExpression;
        }

        private static string GetExpType(string strExpression)
        {
            if (string.IsNullOrEmpty(strExpression))
            {
                strExpression = "0";
            }
            strExpression = strExpression.ToUpper();
            if (strExpression.IndexOf("SIN") != -1)
            {
                return CalculateExExpress(strExpression.Substring(strExpression.IndexOf("N") + 1, strExpression.Length - 1 - strExpression.IndexOf("N")), EnumFormula.Sin).ToString();
            }
            if (strExpression.IndexOf("COS") != -1)
            {
                return CalculateExExpress(strExpression.Substring(strExpression.IndexOf("S") + 1, strExpression.Length - 1 - strExpression.IndexOf("S")), EnumFormula.Cos).ToString();
            }
            if (strExpression.IndexOf("TAN") != -1)
            {
                return CalculateExExpress(strExpression.Substring(strExpression.IndexOf("N") + 1, strExpression.Length - 1 - strExpression.IndexOf("N")), EnumFormula.Tan).ToString();
            }
            if (strExpression.IndexOf("ATAN") != -1)
            {
                return CalculateExExpress(strExpression.Substring(strExpression.IndexOf("N") + 1, strExpression.Length - 1 - strExpression.IndexOf("N")), EnumFormula.ATan).ToString();
            }
            if (strExpression.IndexOf("SQRT") != -1)
            {
                return CalculateExExpress(strExpression.Substring(strExpression.IndexOf("T") + 1, strExpression.Length - 1 - strExpression.IndexOf("T")), EnumFormula.Sqrt).ToString();
            }
            if (strExpression.IndexOf("POW") != -1)
            {
                return CalculateExExpress(strExpression.Substring(strExpression.IndexOf("W") + 1, strExpression.Length - 1 - strExpression.IndexOf("W")), EnumFormula.Pow).ToString();
            }
            if (strExpression.IndexOf("ROUND") != -1)
            {
                return CalculateExExpress(strExpression.Substring(strExpression.IndexOf("D") + 1, strExpression.Length - 1 - strExpression.IndexOf("D")), EnumFormula.Round).ToString();
            }
            if (strExpression.IndexOf("INT") != -1)
            {
                return CalculateExExpress(strExpression.Substring(strExpression.IndexOf("T") + 1, strExpression.Length - 1 - strExpression.IndexOf("T")), EnumFormula.Int).ToString();
            }
            if (strExpression.IndexOf("EXP") != -1)
            {
                return CalculateExExpress(strExpression.Substring(strExpression.IndexOf("P") + 1, strExpression.Length - 1 - strExpression.IndexOf("P")), EnumFormula.Exp).ToString();
            }
            if (strExpression.IndexOf("ABS") != -1)
            {
                return CalculateExExpress(strExpression.Substring(strExpression.IndexOf("S") + 1, strExpression.Length - 1 - strExpression.IndexOf("S")), EnumFormula.Abs).ToString();
            }
            if (strExpression.IndexOf("LOG10") != -1)
            {
                return CalculateExExpress(strExpression.Substring(strExpression.IndexOf("0") + 1, strExpression.Length - 1 - strExpression.IndexOf("0")), EnumFormula.Log10).ToString();
            }
            if (strExpression.IndexOf("LN") != -1)
            {
                return CalculateExExpress(strExpression.Substring(strExpression.IndexOf("N") + 1, strExpression.Length - 1 - strExpression.IndexOf("N")), EnumFormula.Ln).ToString();
            }
            return strExpression;
        }
    }

    /// <summary>
    /// 用于把对象型数据转成特定数据类型的方法
    /// </summary>
    public class ConvertHelper
    {
        #region 将对象变量转成字符串变量的方法
        /// <summary>
        /// 将对象变量转成字符串变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>字符串变量</returns>
        public static string GetString(object obj)
        {
            return (obj == DBNull.Value || obj == null) ? "" : obj.ToString();
        }
        #endregion

        #region 将对象变量转成32位整数型变量的方法
        /// <summary>
        /// 将对象变量转成32位整数型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>32位整数型变量</returns>
        public static int GetInteger(object obj)
        {
            return ConvertStringToInteger(GetString(obj));
        }
        #endregion

        #region 将对象变量转成64位整数型变量的方法
        /// <summary>
        /// 将对象变量转成64位整数型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>64位整数型变量</returns>
        public static long GetLong(object obj)
        {
            return ConvertStringToLong(GetString(obj));
        }
        #endregion

        #region 将对象变量转成双精度浮点型变量的方法
        /// <summary>
        /// 将对象变量转成双精度浮点型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>双精度浮点型变量</returns>
        public static double GetDouble(object obj)
        {
            return ConvertStringToDouble(GetString(obj));
        }
        #endregion

        #region 将对象变量转成十进制数字变量的方法
        /// <summary>
        /// 将对象变量转成十进制数字变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>十进制数字变量</returns>
        public static decimal GetDecimal(object obj)
        {
            return ConvertStringToDecimal(GetString(obj));
        }
        #endregion

        #region 将对象变量转成布尔型变量的方法
        /// <summary>
        /// 将对象变量转成布尔型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>布尔型变量</returns>
        public static bool GetBoolean(object obj)
        {
            return (obj == DBNull.Value || obj == null) ? false :
                GetString(obj).Length == 0 ? false : Convert.ToBoolean(obj);
        }
        #endregion

        #region 将对象变量转成日期时间型字符串变量的方法
        /// <summary>
        /// 将对象变量转成日期时间型字符串变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <param name="sFormat">格式字符</param>
        /// <returns>时间型字符串变量</returns>
        public static string GetDateTimeString(object obj, string sFormat)
        {
            return (obj == DBNull.Value || obj == null) ? "" : Convert.ToDateTime(obj).ToString(sFormat);
        }
        #endregion

        #region 将对象变量转成日期字符串变量的方法
        /// <summary>
        /// 将对象变量转成日期字符串变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>日期字符串变量</returns>
        public static string GetShortDateString(object obj)
        {
            return GetDateTimeString(obj, "yyyy-MM-dd");
        }
        #endregion

        #region 将对象变量转成日期型变量的方法
        /// <summary>
        /// 将对象变量转成日期型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>日期型变量</returns>
        public static DateTime GetDateTime(object obj)
        {
            return obj == null || obj == DBNull.Value ? new DateTime() : Convert.ToDateTime(obj);
        }
        #endregion

        #region 将字符串转成32位整数型变量的方法
        /// <summary>
        /// 将字符串转成32位整数型变量的方法
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>32位整数型变量</returns>
        private static int ConvertStringToInteger(string s)
        {
            int result = 0;
            int.TryParse(s, out result);
            return result;
        }
        #endregion

        #region 将字符串转成64位整数型变量的方法
        /// <summary>
        /// 将字符串转成64位整数型变量的方法
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>64位整数型变量</returns>
        private static long ConvertStringToLong(string s)
        {
            long result = 0;
            long.TryParse(s, out result);
            return result;
        }
        #endregion

        #region 将字符串转成双精度浮点型变量的方法
        /// <summary>
        /// 将字符串转成双精度浮点型变量的方法
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>双精度浮点型变量</returns>
        private static double ConvertStringToDouble(string s)
        {
            double result = 0;
            double.TryParse(s, out result);
            return result;
        }
        #endregion

        #region 将字符串转成十进制数字变量的方法
        /// <summary>
        /// 将字符串转成十进制数字变量的方法
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>十进制数字变量</returns>
        private static decimal ConvertStringToDecimal(string s)
        {
            decimal result = 0;
            decimal.TryParse(s, out result);
            return result;
        }
        #endregion


        #region 将字符串转换成计算表达式并返回计算结果,支持大部分数学函数
        /// <summary>
        /// 将字符串转换成表达式并返回结果
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static dynamic ExpressEval(string expression)
        {
                string strVal = FormulaDeal.SpiltExpression(expression);
                return strVal;
        }
        #endregion

        /// <summary>
        /// 将字符串转换成表达式并返回结果
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        //public static dynamic Eval(string expression)
        //{
        //    try
        //    {
        //        MSScriptControl.ScriptControl mspt = new MSScriptControl.ScriptControl();
        //        mspt.Language = "vbscript";
        //        return mspt.Eval(expression).ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

    }
}
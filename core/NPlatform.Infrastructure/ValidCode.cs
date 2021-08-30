/********************************************************************************

** auth： DongliangYi

** date： 2016/9/5 14:05:37

** desc： 尚未编写描述

** Ver.:  V1.0.0

*********************************************************************************/

namespace NPlatform.Infrastructure
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Text;

    /// <summary>
    ///     产生中文验证码
    /// </summary>
    public class ValidCodeCn
    {
        private const double PI = 3.1415926535897932384626433832795;

        private const double PI2 = 6.283185307179586476925286766559;

        /// <summary>
        /// 生成gif格式的验证码图片
        /// </summary>
        /// <returns>返回验证码图片的 byte[]数据</returns>
        public static Bitmap CreateGifValidCode(out string checkCode, int height = 43)
        {
            checkCode = GenerateCheckCode();
            if (string.IsNullOrEmpty(checkCode))
                return null;

            var image = new Bitmap((int)Math.Ceiling(checkCode.Length * 27.0), height);
            var g = Graphics.FromImage(image);

            try
            {
                //生成随机生成器 
                var random = new Random(Guid.NewGuid().GetHashCode());

                //清空图片背景色 
                g.Clear(Color.White);

                //画图片的背景噪音线 
                for (var i = 0; i < 12; i++)
                {
                    var x1 = random.Next(image.Width);
                    var x2 = random.Next(image.Width);
                    var y1 = random.Next(image.Height);
                    var y2 = random.Next(image.Height);

                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                //Font font = new System.Drawing.Font("Arial", 16, (System.Drawing.FontStyle.Bold 　 System.Drawing.FontStyle.Italic)); 
                var font = new Font("微软雅黑", 16, FontStyle.Bold);
                var brush = new LinearGradientBrush(
                    new Rectangle(0, 0, image.Width, image.Height),
                    Color.Blue,
                    Color.DarkRed,
                    1.2f,
                    true);
                g.DrawString(checkCode, font, brush, 2, 2);

                //画图片的前景噪音点 
                for (var i = 0; i < 100; i++)
                {
                    var x = random.Next(image.Width);
                    var y = random.Next(image.Height);

                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                //image = TwistImage(image, true, 3, 1); 
                //画图片的波形滤镜效果 
                //画图片的边框线 
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                return image;
            }
            catch
            {
                image.Dispose();
                return null;
            }
            finally
            {
                g.Dispose();
            }
        }

        /// <summary>
        /// 生成随机颜色
        /// </summary>
        /// <returns></returns>
        public static string GetRandomColor()
        {
            Random randomNumFirst = new Random((int)DateTime.Now.Ticks);

            //  对于C#的随机数，没什么好说的

            System.Threading.Thread.Sleep(randomNumFirst.Next(50));

            Random randomNumSencond = new Random((int)DateTime.Now.Ticks);

            //  为了在白色背景上显示，尽量生成深色

            int intRed = randomNumFirst.Next(256);

            int intGreen = randomNumSencond.Next(256);

            int intBlue = (intRed + intGreen > 400) ? 0 : 400 - intRed - intGreen;

            intBlue = (intBlue > 255) ? 255 : intBlue;
            Color color = Color.FromArgb(intRed, intGreen, intBlue);
            string strColor = "#" + Convert.ToString(color.ToArgb(), 16).PadLeft(8, '0').Substring(2, 6);
            return strColor;
        }

        private static object[] CreateRegionCode(int strlength)
        {
            //定义一个字符串数组储存汉字编码的组成元素 
            var rBase = new string[16]
                            {
                                "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f"
                            };

            var rnd = new Random(Guid.NewGuid().GetHashCode());

            //定义一个object数组用来 
            var bytes = new object[strlength];

            /*每循环一次产生一个含两个元素的十六进制字节数组，并将其放入bject数组中 
            每个汉字有四个区位码组成 
            区位码第1位和区位码第2位作为字节数组第一个元素 
            区位码第3位和区位码第4位作为字节数组第二个元素 
            */
            for (var i = 0; i < strlength; i++)
            {
                //区位码第1位 
                var r1 = rnd.Next(11, 14);
                var str_r1 = rBase[r1].Trim();

                //区位码第2位 
                rnd = new Random(r1 * unchecked((int)DateTime.Now.Ticks) + i); //更换随机数发生器的 

                //种子避免产生重复值 
                int r2;
                if (r1 == 13)
                {
                    r2 = rnd.Next(0, 7);
                }
                else
                {
                    r2 = rnd.Next(0, 16);
                }

                var str_r2 = rBase[r2].Trim();

                //区位码第3位 
                rnd = new Random(r2 * unchecked((int)DateTime.Now.Ticks) + i);
                var r3 = rnd.Next(10, 16);
                var str_r3 = rBase[r3].Trim();

                //区位码第4位 
                rnd = new Random(r3 * unchecked((int)DateTime.Now.Ticks) + i);
                int r4;
                if (r3 == 10)
                {
                    r4 = rnd.Next(1, 16);
                }
                else if (r3 == 15)
                {
                    r4 = rnd.Next(0, 15);
                }
                else
                {
                    r4 = rnd.Next(0, 16);
                }

                var str_r4 = rBase[r4].Trim();

                //定义两个字节变量存储产生的随机汉字区位码 
                var byte1 = Convert.ToByte(str_r1 + str_r2, 16);
                var byte2 = Convert.ToByte(str_r3 + str_r4, 16);
                //将两个字节变量存储在字节数组中 
                byte[] str_r = { byte1, byte2 };

                //将产生的一个汉字的字节数组放入object数组中 
                bytes.SetValue(str_r, i);
            }

            return bytes;
        }

        /// <summary>
        /// 产生中文验证码
        /// </summary>
        /// <returns></returns>
        private static string GenerateCheckCode()
        {
            //char code; 
            var checkCode = string.Empty;

            var random = new Random(Guid.NewGuid().GetHashCode());

            var gb = Encoding.GetEncoding("gb2312");
            //调用函数产生4个随机中文汉字编码 
            var bytes = CreateRegionCode(6);
            //根据汉字编码的字节数组解码出中文汉字 
            var str1 = gb.GetString((byte[])Convert.ChangeType(bytes[0], typeof(byte[])));
            var str2 = gb.GetString((byte[])Convert.ChangeType(bytes[1], typeof(byte[])));
            var str3 = gb.GetString((byte[])Convert.ChangeType(bytes[2], typeof(byte[])));
            //输出的控制台 
            checkCode = str1 + str2 + str3;
            return checkCode;
        }

        private Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            var destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);

            // 将位图背景填充为白色 
            var graph = Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();

            double dBaseAxisLen = bXDir ? destBmp.Height : destBmp.Width;

            for (var i = 0; i < destBmp.Width; i++)
            {
                for (var j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? PI2 * j / dBaseAxisLen : PI2 * i / dBaseAxisLen;
                    dx += dPhase;
                    var dy = Math.Sin(dx);

                    // 取得当前点的颜色 
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    var color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }

            return destBmp;
        }
    }

    /// <summary>
    /// 验证码
    /// </summary>
    public class ValidCodeEn
    {
        /// <summary>
        /// 验证码绘图
        /// </summary>
        /// <example>
        /// <code>
        ///     string strIdentifyCode = Pub.Class.Identify.IdentifyCode(4);
        ///     Pub.Class.Identify.DrawIdentifyCode(strIdentifyCode, 50, 100);
        ///     Response.End();
        ///     在登录页面引用此文件:&lt;img src="Identify.aspx" border="0" style="cursor: pointer;cursor:hand;" onclick="javascript:this.src='Identify.aspx?iTime=' + Math.random();" title="单击可更换新的验证码" />
        ///     Session["IdentifyCode"]
        /// </code>
        /// </example>
        /// <param name="strIdentifyCode">验证码</param>
        /// <param name="intFgNoise">文字噪音程度</param>
        /// <param name="intBgNoise">背景噪音程度</param>
        /// <param name="height">图片高</param>
        /// <param name="width">图片宽</param>
        public static byte[] CreateGifValidCodeBytes(
            out string strIdentifyCode,
            int intFgNoise,
            int intBgNoise,
            int height = 43,
            int width = 30)
        {
            var img = CreateGifValidCode(out strIdentifyCode, intFgNoise, intBgNoise, height, width);
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                //将图片 保存到内存流中
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                //将内存流 里的 数据  转成 byte 数组 返回
                return ms.ToArray();
            }
        }
        /// <summary>
        /// 验证码绘图
        /// </summary>
        /// <example>
        /// </example>
        /// <param name="strIdentifyCode">验证码</param>
        /// <param name="intFgNoise">文字噪音程度</param>
        /// <param name="intBgNoise">背景噪音程度</param>
        /// <param name="height">图片高</param>
        /// <param name="width">图片宽</param>
        public static Bitmap CreateGifValidCode(
            out string strIdentifyCode,
            int intFgNoise,
            int intBgNoise,
            int height = 43,
            int width = 30)
        {
            strIdentifyCode = IdentifyCode(5);
            if (strIdentifyCode == null || strIdentifyCode.Trim() == String.Empty)
            {
                return null;
            }
            else
            {
                var length = (int)Math.Ceiling((strIdentifyCode.Length * 12.5));
                length = length > width ? length : width;

                Bitmap bmpImage = new Bitmap(length, height); //建立一个位图文件 确立长宽
                Graphics grpGraphics = Graphics.FromImage(bmpImage);

                try
                {
                    grpGraphics.Clear(Color.White); //清空图片背景色

                    for (int i = 0; i < intBgNoise; i++)
                    {
                        //画图片的背景噪音线
                        Random rndRandom = new Random(Guid.NewGuid().GetHashCode()); //生成随机生成器
                        int int_x1 = rndRandom.Next(bmpImage.Width);
                        int int_x2 = rndRandom.Next(bmpImage.Width);
                        int int_y1 = rndRandom.Next(bmpImage.Height);
                        int int_y2 = rndRandom.Next(bmpImage.Height);
                        int int_x3 = rndRandom.Next(bmpImage.Width);
                        int int_y3 = rndRandom.Next(bmpImage.Height);
                        grpGraphics.DrawLines(
                            new Pen(GetRandomColor(), 2),
                            new Point[]
                                {
                                    new Point { X = int_x1, Y = int_y1 }, new Point { X = int_x2, Y = int_y2 },
                                    new Point { X = int_x3, Y = int_y3 }
                                });
                    }

                    Font font = new Font("Arial", 14, (FontStyle.Bold | FontStyle.Italic)); //把产生的随机数以字体的形式写入画面
                    LinearGradientBrush brhBrush = new LinearGradientBrush(
                        new Rectangle(0, 0, bmpImage.Width, bmpImage.Height),
                        Color.Brown,
                        Color.Black,
                        1.2f,
                        true);
                    grpGraphics.DrawString(strIdentifyCode, font, brhBrush, 2, 2);

                    for (int i = 0; i < intFgNoise; i++)
                    {
                        //画图片的前景噪音点
                        Random rndRandom = new Random(Guid.NewGuid().GetHashCode()); //生成随机生成器
                        int int_x = rndRandom.Next(bmpImage.Width);
                        int int_y = rndRandom.Next(bmpImage.Height);

                        bmpImage.SetPixel(int_x, int_y, GetRandomColor());
                    }

                    grpGraphics.DrawRectangle(
                        new Pen(Color.Silver),
                        0,
                        0,
                        bmpImage.Width - 1,
                        bmpImage.Height - 1); //画图片的边框线
                    return bmpImage;
                }
                catch
                {
                    bmpImage.Dispose();
                    return null;
                }
                finally
                {
                    grpGraphics.Dispose();
                }
            }
        }

        /// <summary>
        /// 生成随机颜色
        /// </summary>
        /// <returns></returns>
        public static Color GetRandomColor()
        {
            Random randomNumFirst = new Random(Guid.NewGuid().GetHashCode());

            Random randomNumSencond = new Random(Guid.NewGuid().GetHashCode());

            //  为了在白色背景上显示，尽量生成深色

            int intRed = randomNumFirst.Next(256);

            int intGreen = randomNumSencond.Next(256);

            int intBlue = (intRed + intGreen > 400) ? 0 : 400 - intRed - intGreen;

            intBlue = (intBlue > 255) ? 255 : intBlue;
            Color color = Color.FromArgb(intRed, intGreen, intBlue);
            return color;
        }

        /// <summary>
        /// 取得随机字符串，并设置Session值
        /// </summary>
        /// <example>
        /// <code>
        ///     string strIdentifyCode = Pub.Class.Identify.IdentifyCode(4);
        ///     Pub.Class.Identify.DrawIdentifyCode(strIdentifyCode, 50, 100);
        ///     Response.End();
        ///     在登录页面引用此文件:&lt;img src="Valid.aspx"  border="0" />
        ///     Session["IdentifyCode"]
        /// </code>
        /// </example>
        /// <param name="intLength"></param>
        /// <returns></returns>
        public static string IdentifyCode(int intLength)
        {
            int intNumber;
            char chrCode;
            string strIdentifyCode = String.Empty;
            Random rndRandom = new Random();
            for (int i = 0; i < intLength; i++)
            {
                intNumber = rndRandom.Next();
                if (intNumber % 2 == 0)
                {
                    chrCode = (char)('0' + (char)(intNumber % 10)); //如果随机数是偶数 取余
                }
                else
                {
                    chrCode = (char)('A' + (char)(intNumber % 26)); //如果随机数是奇数 选择从[A-Z]
                }

                strIdentifyCode += chrCode.ToString();
            }

            return strIdentifyCode;
        }
    }
}
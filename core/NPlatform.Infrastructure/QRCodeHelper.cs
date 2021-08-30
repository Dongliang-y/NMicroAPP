/********************************************************************************

** auth： DongliangYi

** date： 2016/9/5 15:08:00

** desc： 尚未编写描述

** Ver.:  V1.0.0

*********************************************************************************/

namespace NPlatform.Infrastructure
{
    using System.Drawing;

    using QRCoder;

    /// <summary>
    /// 二维码工具类
    /// </summary>
    public class QRCodeHelper
    {
        /// <summary>  
        /// 生成二维码图片  
        /// </summary>  
        /// <param name="codeStr">要生成二维码的字符串</param>      
        /// <returns>二维码图片</returns>  
        public Bitmap CreateQRCode(string codeStr)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(codeStr, QRCodeGenerator.ECCLevel.H);
            QRCode qrCode = new QRCode(qrCodeData);

            Bitmap qrCodeImage = qrCode.GetGraphic(20,Color.Black,Color.White, true /*如果是的话，在整个二维码周围画一个白色的边框*/);
            return qrCodeImage;
        }
    }
}
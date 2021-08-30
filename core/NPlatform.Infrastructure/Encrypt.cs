namespace NPlatform.Infrastructure
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    #region DES

    /// <summary>
    /// DES加密，高速可逆
    /// </summary>
    public class DES
    {
        // Key为64位（8个字符）
        private static string ms_IV = "asdfghjk";

        private static string ms_Key = "asdfghjk";

        /// <summary>
        /// 私密
        /// </summary>
        protected static string Ms_IV
        {
            set
            {
                ms_IV = value;
            }
        }

        /// <summary>
        /// 公密
        /// </summary>
        protected static string Ms_Key
        {
            set
            {
                ms_Key = value;
            }
        }

        private static byte[] IV_64
        {
            get
            {
                return new byte[] { 0x37, 0x67, 0xf6, 0x4f, 0x24, 0x63, 0xa7, 3 };
            }
        }

        private static byte[] KEY_64
        {
            get
            {
                return new byte[] { 0x2a, 0x10, 0x5d, 0x9c, 0x4e, 4, 0xda, 0x20 };
            }
        }

        /// <summary>
        /// 内置固定Key解密
        /// </summary>
        public static string DESDecrypt(string strOutput)
        {
            if (strOutput != string.Empty)
            {
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream stream = new MemoryStream(Convert.FromBase64String(strOutput));
                CryptoStream stream2 = new CryptoStream(
                    stream,
                    provider.CreateDecryptor(KEY_64, IV_64),
                    CryptoStreamMode.Read);
                StreamReader reader = new StreamReader(stream2);
                return reader.ReadToEnd();
            }

            return string.Empty;
        }

        /// <summary>
        /// 使用8位长度的字符Key解密
        /// </summary>
        public static string DESDecryptByKey(string decryptString, string key)
        {
            if (decryptString != string.Empty)
            {
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                provider.Key = Encoding.ASCII.GetBytes(key);
                provider.IV = Encoding.ASCII.GetBytes(key);
                MemoryStream stream = new MemoryStream(Convert.FromBase64String(decryptString));
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Read);
                StreamReader reader = new StreamReader(stream2);
                return reader.ReadToEnd();
            }

            return string.Empty;
        }

        /// <summary>
        /// 内置固定Key加密
        /// </summary>
        public static string DESEncrypt(string strInput)
        {
            if (strInput != string.Empty)
            {
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(
                    stream,
                    provider.CreateEncryptor(KEY_64, IV_64),
                    CryptoStreamMode.Write);
                StreamWriter writer = new StreamWriter(stream2);
                writer.Write(strInput);
                writer.Flush();
                stream2.FlushFinalBlock();
                stream.Flush();
                return Convert.ToBase64String(stream.GetBuffer(), 0, int.Parse(stream.Length.ToString()));
            }

            return string.Empty;
        }

        /// <summary>
        /// 使用8位长度的字符Key加密
        /// </summary>
        public static string DESEncryptByKey(string ecryptString, string key)
        {
            if (ecryptString != string.Empty)
            {
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                provider.Key = Encoding.ASCII.GetBytes(key);
                provider.IV = Encoding.ASCII.GetBytes(key);
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
                StreamWriter writer = new StreamWriter(stream2);
                writer.Write(ecryptString);
                writer.Flush();
                stream2.FlushFinalBlock();
                stream.Flush();
                return Convert.ToBase64String(stream.GetBuffer(), 0, int.Parse(stream.Length.ToString()));
            }

            return string.Empty;
        }
    }

    #endregion

    #region MD5

    /// <summary>
    /// 不可逆
    /// </summary>
    public class MD5S
    {
        /// <summary>
        /// MD5 32位
        /// </summary>
        public static string EncodeMd5(string s)
        {
            using (MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider())
            {
                byte[] bytes = Encoding.Default.GetBytes(s);
                return BitConverter.ToString(provider.ComputeHash(bytes)).Replace("-", string.Empty);
            }
        }

        /// <summary>
        /// MD5 16位
        /// </summary>
        /// <param name="convertString"></param>
        /// <returns></returns>
        public static string GetMd5Str(string convertString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(convertString)), 4, 8);
            t2 = t2.Replace("-", string.Empty);
            return t2.ToLower();
        }

        /// <summary>
        /// SHA256函数
        /// </summary>
        /// /// <param name="str">原始字符串</param>
        /// <returns>SHA256结果</returns>
        public static string GetSHA256(string str)
        {
            byte[] SHA256Data = Encoding.UTF8.GetBytes(str);
            SHA256Managed Sha256 = new SHA256Managed();

            byte[] Result = Sha256.ComputeHash(SHA256Data);
            return Convert.ToBase64String(Result); // 返回长度为44字节的字符串
        }
    }

    #endregion

    #region AES

    /// <summary> 
    /// AES加密
    /// </summary> 
    public class AES
    {
        /// <summary>
        /// 默认密钥向量
        /// </summary>
        private static byte[] keys =
            {
                0x41, 0x72, 0x65, 0x79, 0x6F, 0x75, 0x6D, 0x79, 0x53, 0x6E, 0x6F, 0x77, 0x6D, 0x61, 0x6E, 0x3F
            };

        /// <summary>
        /// AES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串,失败返源串</returns>
        public static string Decode(string decryptString, string decryptKey)
        {
            try
            {
                decryptKey = GetSubString(decryptKey, 32, string.Empty);
                decryptKey = decryptKey.PadRight(32, ' ');

                RijndaelManaged rijndaelProvider = new RijndaelManaged();
                rijndaelProvider.Key = Encoding.UTF8.GetBytes(decryptKey);
                rijndaelProvider.IV = keys;
                ICryptoTransform rijndaelDecrypt = rijndaelProvider.CreateDecryptor();

                byte[] inputData = Convert.FromBase64String(decryptString);
                byte[] decryptedData = rijndaelDecrypt.TransformFinalBlock(inputData, 0, inputData.Length);

                return Encoding.UTF8.GetString(decryptedData);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// AES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串,失败返回源串</returns>
        public static string Encode(string encryptString, string encryptKey="cszjjw.2020-5-19")
        {
            encryptKey = GetSubString(encryptKey, 32, string.Empty);
            encryptKey = encryptKey.PadRight(32, ' ');

            RijndaelManaged rijndaelProvider = new RijndaelManaged();
            rijndaelProvider.Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 32));
            rijndaelProvider.IV = keys;
            ICryptoTransform rijndaelEncrypt = rijndaelProvider.CreateEncryptor();

            byte[] inputData = Encoding.UTF8.GetBytes(encryptString);
            byte[] encryptedData = rijndaelEncrypt.TransformFinalBlock(inputData, 0, inputData.Length);

            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="pLength">指定长度</param>
        /// <param name="pTailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int pLength, string pTailString)
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
    }

    #endregion

    #region RSA

    /// <summary>
    /// RSA 非对称加密算法
    /// </summary>
    public class RSA
    {
        private RSACryptoServiceProvider _rsa = new RSACryptoServiceProvider();

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="s"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] s, string privateKey)
        {
            this._rsa.FromXmlString(privateKey);
            return this._rsa.Decrypt(s, false);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="s"></param>
        /// <param name="privateKey"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public string Decrypt(string s, string privateKey, string encode)
        {
            byte[] bytes = this.Decrypt(Convert.FromBase64String(s), privateKey);
            return Encoding.GetEncoding(encode).GetString(bytes);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="s"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] s, string publicKey)
        {
            this._rsa.FromXmlString(publicKey);
            return this._rsa.Encrypt(s, false);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="s"></param>
        /// <param name="publicKey"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public string Encrypt(string s, string publicKey, string encode)
        {
            return Convert.ToBase64String(this.Encrypt(Encoding.GetEncoding(encode).GetBytes(s), publicKey));
        }

        /// <summary>
        /// 同一个对象产生的私密用来解密
        /// </summary>
        /// <returns></returns>
        public string GetPrivateKey()
        {
            return this._rsa.ToXmlString(true);
        }

        /// <summary>
        /// 同一个对象产生的公密用来加密
        /// </summary>
        public string GetPublicKey()
        {
            return this._rsa.ToXmlString(false);
        }
    }

    #endregion
}
using Grit.Net.Common.Convert;
using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Grit.Net.Common.Encryption
{
    public class DES
    {
        //默认密钥向量  
        private static readonly byte[] IV_Keys = { 0xAB, 0xCD, 0x02, 0x34, 0x56, 0xEF, 0x87, 0x91 };
        // 创建Key
        public static string GenerateKey()
        {
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
            return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
        }
        ///MD5加密
        public static string MD5Encrypt(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();

        }

        ///MD5解密
        public static string MD5Decrypt(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (DataConvert.ToInt(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();

            return Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary>  
        /// DES加密字符串  
        /// </summary>  
        /// <param name="encryptString">待加密的字符串</param>  
        /// <param name="encryptKey">加密密钥,要求为8位</param>  
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>  
        public static string EncryptDES(string encryptString, byte[] encryptKey)
        {
            try
            {
                byte[] rgbKey = encryptKey;
                byte[] rgbIV = IV_Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return System.Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary>  
        /// DES解密字符串  
        /// </summary>  
        /// <param name="decryptString">待解密的字符串</param>  
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>  
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>  
        public static string DecryptDES(string decryptString, byte[] decryptKey)
        {
            try
            {
                byte[] rgbKey = decryptKey;
                byte[] rgbIV = IV_Keys;
                byte[] inputByteArray = System.Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Net.Common.Convert
{
    public class ByteConvert
    {/// <summary>
     /// 将指定进制的字符串转换到指定进制的字符串
     /// </summary>
     /// <param name="value">要转换的字符串</param>
     /// <param name="fromBase">value 中数字的基数，它必须是 2、8、10 或 16。</param>
     /// <param name="toBase">value 中数字的基数，它必须是 2、8、10 或 16。</param>
     /// <returns></returns>
        public static string ToString(string value, int fromBase, int toBase)
        {
            return System.Convert.ToString(System.Convert.ToInt32(value, fromBase), toBase);
        }

        /// <summary>
        /// 将字节数组节转换成指定进制的字符串
        /// </summary>
        /// <param name="byteArray">要转换的字符串</param>
        /// <param name="toBase">它必须是 2 或 16</param>
        /// <returns></returns>
        public static string ToString(byte[] byteArray, int toBase = 16)
        {
            if (byteArray == null || byteArray.Length == 0)
                return string.Empty;

            switch (toBase)
            {
                case 2:
                    return ByteArrayToBinaryString(byteArray);
                case 16:
                    return ByteArrayToHexStr(byteArray);
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// 将指定进制的字符串转换成字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fromBase"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(string value, int fromBase = 16)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            switch (fromBase)
            {
                case 2:
                    return BinaryStringToByteArray(value);
                case 16:
                    return HexStrToByteArray(value);
                default:
                    return null;
            }
        }

        /// <summary>
        /// 字节数组节转换成二进制字符串
        /// </summary>
        /// <param name="b">要转换的字节数组</param>
        /// <returns></returns>
        public static string ByteArrayToBinaryString(byte[] byteArray)
        {
            int capacity = byteArray.Length * 8;
            StringBuilder sb = new StringBuilder(capacity);
            for (int i = 0; i < byteArray.Length; i++)
            {
                sb.Append(byte2BinString(byteArray[i]));
            }
            return sb.ToString();
        }

        private static string byte2BinString(byte b)
        {
            return System.Convert.ToString(b, 2).PadLeft(8, '0');
        }

        /// <summary>
        /// 二进制字符串转换成字节数组
        /// </summary>
        /// <param name="binaryString">要转换的字符，如"00000000 11111111"</param>
        /// <returns></returns>
        private static byte[] BinaryStringToByteArray(string binaryString)
        {
            binaryString = binaryString.Replace(" ", "");

            if ((binaryString.Length % 8) != 0)
                throw new ArgumentException("二进制字符串长度不对");

            byte[] buffer = new byte[binaryString.Length / 8];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = System.Convert.ToByte(binaryString.Substring(i * 8, 8).Trim(), 2);
            }
            return buffer;

        }

        /// <summary>
        /// 字节数组转换成十六进制字符串
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>
        /// <returns></returns>
        public static string ByteArrayToHexStr(byte[] byteArray)
        {
            int capacity = byteArray.Length * 2;
            StringBuilder sb = new StringBuilder(capacity);

            if (byteArray != null)
            {
                for (int i = 0; i < byteArray.Length; i++)
                {
                    sb.Append(byteArray[i].ToString("X2"));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 十六进制字符串转换成字节数组 
        /// </summary>
        /// <param name="hexString">要转换的字符串</param>
        /// <returns></returns>
        private static byte[] HexStrToByteArray(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                throw new ArgumentException("十六进制字符串长度不对");
            byte[] buffer = new byte[hexString.Length / 2];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = System.Convert.ToByte(hexString.Substring(i * 2, 2).Trim(), 0x10);
            }
            return buffer;
        }
    }
}

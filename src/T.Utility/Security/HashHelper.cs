using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace T.Utility.Security
{
    /// <summary>
    /// 哈希算法封装类
    /// </summary>
    public static class HashHelper
    {
        #region md5

        /// <summary>
        /// 计算字节数组的md5值
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public static string Md5(byte[] bytes)
        {
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                byte[] byteArray = md5.ComputeHash(bytes);

                var stringBuilder = new StringBuilder();
                foreach (byte b in byteArray)
                {
                    stringBuilder.AppendFormat("{0:x2}", b);
                }
                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// 计算字符串的md5值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="encoding">字符编码(默认值:UTF-8)</param>
        /// <returns></returns>
        public static string Md5(string str, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            byte[] bytes = encoding.GetBytes(str);
            return Md5(bytes);
        }

        #endregion

        #region sha1

        /// <summary>
        /// 计算文件流的sha1值
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <returns></returns>
        public static string Sha1(Stream stream)
        {
            using (SHA1 sha1 = new SHA1Managed())
            {
                byte[] byteArray = sha1.ComputeHash(stream);

                var stringBuilder = new StringBuilder();
                foreach (byte b in byteArray)
                {
                    stringBuilder.AppendFormat("{0:x2}", b);
                }
                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// 计算字节数组的sha1值
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public static string Sha1(byte[] bytes)
        {
            using (SHA1 sha1 = new SHA1Managed())
            {
                byte[] byteArray = sha1.ComputeHash(bytes);

                var stringBuilder = new StringBuilder();
                foreach (byte b in byteArray)
                {
                    stringBuilder.AppendFormat("{0:x2}", b);
                }
                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// 计算字符串的sha1值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="encoding">字符编码(默认值:UTF-8)</param>
        /// <returns></returns>
        public static string Sha1(string str, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            byte[] bytes = encoding.GetBytes(str);
            return Sha1(bytes);
        }

        #endregion

        #region sha256

        /// <summary>
        /// 计算字节数组的sha256值
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public static string Sha256(byte[] bytes)
        {
            using (SHA256 sha256 = new SHA256Managed())
            {
                byte[] byteArray = sha256.ComputeHash(bytes);

                var stringBuilder = new StringBuilder();
                foreach (byte b in byteArray)
                {
                    stringBuilder.AppendFormat("{0:x2}", b);
                }
                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// 计算字符串的sha256值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="encoding">字符编码(默认值:UTF-8)</param>
        /// <returns></returns>
        public static string Sha256(string str, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            byte[] bytes = encoding.GetBytes(str);
            return Sha256(bytes);
        }

        #endregion

        #region sha512

        /// <summary>
        /// 计算字节数组的sha512值
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public static string Sha512(byte[] bytes)
        {
            using (SHA512 sha512 = new SHA512Managed())
            {
                byte[] byteArray = sha512.ComputeHash(bytes);

                var stringBuilder = new StringBuilder();
                foreach (byte b in byteArray)
                {
                    stringBuilder.AppendFormat("{0:x2}", b);
                }
                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// 计算字符串的sha512值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="encoding">字符编码(默认值:UTF-8)</param>
        /// <returns></returns>
        public static string Sha512(string str, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            byte[] bytes = encoding.GetBytes(str);
            return Sha512(bytes);
        }

        #endregion

    }
}

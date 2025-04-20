using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Demo.Sign.Utils
{
    /// <summary>
    /// 加解密服务
    /// </summary>
    public class AesService
    {
        /// <summary>
        /// 密钥
        /// </summary>
        static readonly byte[] Key = System.Text.ASCIIEncoding.ASCII.GetBytes("0123456789abcdef");
        /// <summary>
        /// 向量
        /// </summary>
        static readonly byte[] IV = System.Text.ASCIIEncoding.ASCII.GetBytes("abcdef0123456789");

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] EncryptStringToBytes_Aes(string plainText)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string DecryptStringFromBytes_Aes(byte[] cipherText)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }


        /// <summary>
        /// 加密
        /// 返回大写的密码
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Encode(string plainText)
        {
            return ConverterService.GetStringFormByteArray(EncryptStringToBytes_Aes(plainText)).ToUpper();
        }

        /// <summary>
        /// 解密
        /// 返回源码
        /// </summary>
        /// <param name="cipherText">带解密字符串</param>
        /// <returns></returns>
        public static string Decode(string cipherText)
        {
            return DecryptStringFromBytes_Aes(ConverterService.GetByteArrayFormString(cipherText));
        }

    }
    /// <summary>
    /// 字节与字符串转换服务
    /// </summary>
    public class ConverterService
    {

        /// <summary>
        /// 字节数组转转换成字符串，按照 <see cref="spiltor"/> 进行分割
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="spiltor">分隔符，默认无分割符号</param>
        /// <param name="hasLastSpiltor">是否保留最后一个分隔符，默认False</param>
        /// <returns></returns>
        public static string GetStringFormByteArray(byte[] data, string spiltor = "", bool hasLastSpiltor = false)
        {
            if (data == null || data.Length == 0) return "数据为空";

            // 定义字符串的内存空间
            StringBuilder sb = new StringBuilder(data.Length * 3);

            if (string.IsNullOrEmpty(spiltor))
            {
                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("X2"));
                }
            }
            else
            {
                // 然后将数组里面的内容挨个赋值
                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("X2"));

                    if (i == data.Length - 1)
                    {
                        if (hasLastSpiltor)
                        {
                            sb.Append(spiltor);
                        }
                    }
                    else
                    {
                        sb.Append(spiltor);
                    }
                }
            }

            // 将数据转换成字符串输出
            return sb.ToString().Trim();
        }

        /// <summary>
        /// 获取以 <see cref="splitor"/> 分开的16进制字符的值
        /// </summary>
        /// <param name="shex">16进制数据字符串</param>
        /// <param name="spiltor">分隔符，默认无分割符号</param>
        /// <returns></returns>
        public static byte[] GetByteArrayFormString(string shex, string splitor = "")
        {
            List<byte> bytList = new List<byte>();

            if (string.IsNullOrEmpty(splitor))
            {
                for (int i = 0; i < shex.Length; i += 2)
                {
                    bytList.Add(Convert.ToByte(shex.Substring(i, 2), 16));
                }
            }
            else
            {
                string[] ssArray = shex.Trim().Split(splitor.ToCharArray()[0]);
                foreach (var s in ssArray)
                {                //将十六进制的字符串转换成数值  
                    bytList.Add(Convert.ToByte(s, 16));
                }
            }
            //返回字节数组          
            return bytList.ToArray();
        }


        public static string TransferBytesToString(Double transferSpeed)
        {
            var value = transferSpeed > 0 ? transferSpeed / 1024 : 0; //get KB/s

            if (value == 0)
            {
                return "0 KB";
            }

            if (value < 1024)
            {
                return Math.Round(value, 2).ToString() + " KB";
            }
            else
            {
                value = value / 1024;
                return Math.Round(value, 2).ToString() + " MB";
            }
        }
    }

}

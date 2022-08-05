using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace edit20210325.Function
{
    public class DESFunction
    {
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="pToEncrypt">要加密的字串</param>
        /// <param name="sKey">長度只有8的字串</param>
        /// <returns></returns>
        public static string DESEncrypt(string pToEncrypt, string sKey)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
                des.Key = new ASCIIEncoding().GetBytes(sKey);
                des.IV = new ASCIIEncoding().GetBytes(sKey);
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
            catch(Exception)
            {
                return "輸入字串有問題!";
            }
            
            
        }
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="pToEncrypt">字串</param>
        /// <param name="myKey">內建的Key</param>
        /// <param name="sKey">產生的Key</param>
        /// <returns></returns>
        public static string DESEncrypt(string pToEncrypt, string myKey, string sKey)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
                des.Key = new ASCIIEncoding().GetBytes(myKey);
                des.IV = new ASCIIEncoding().GetBytes(sKey);
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
            catch (Exception)
            {
                return "輸入字串有問題!";
            }


        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="pToDecrypt">解加密的字串</param>
        /// <param name="sKey">長度只有8的字串</param>
        /// <returns></returns>
        public static string DESDecrypt(string pToDecrypt, string sKey)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }

                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                StringBuilder ret = new StringBuilder();

                return System.Text.Encoding.Default.GetString(ms.ToArray());
            }
            catch (Exception)
            {
                return "輸出字串有問題!";
            }        
        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="pToDecrypt">加密後的碼</param>
        /// <param name="myKey">內建Key，長度8</param>
        /// <param name="sKey">產生給我的Key，長度8</param>
        /// <returns></returns>
        public static string DESDecrypt(string pToDecrypt, string myKey,string sKey)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }

                des.Key = ASCIIEncoding.ASCII.GetBytes(myKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                StringBuilder ret = new StringBuilder();

                return System.Text.Encoding.Default.GetString(ms.ToArray());
            }
            catch (Exception)
            {
                return "輸出字串有問題!";
            }
        }


        public static string GetRandomStringByHashKey(int length)
        {
            var str = Path.GetRandomFileName().Replace(".", "");
            return str.Substring(0, length);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace _8DManagementSystem.Common
{
    public class DESEncrypt
    {
        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(text);
            var hashPasswordForStoringInConfigFile = FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5");
            if (hashPasswordForStoringInConfigFile != null)
                des.Key = Encoding.ASCII.GetBytes(hashPasswordForStoringInConfigFile.Substring(0, 8));
            var passwordForStoringInConfigFile = FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5");
            if (passwordForStoringInConfigFile != null)
                des.IV = Encoding.ASCII.GetBytes(passwordForStoringInConfigFile.Substring(0, 8));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            var hashPasswordForStoringInConfigFile = FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5");
            if (hashPasswordForStoringInConfigFile != null)
                des.Key = Encoding.ASCII.GetBytes(hashPasswordForStoringInConfigFile.Substring(0, 8));
            var passwordForStoringInConfigFile = FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5");
            if (passwordForStoringInConfigFile != null)
                des.IV = Encoding.ASCII.GetBytes(passwordForStoringInConfigFile.Substring(0, 8));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }
    }
}

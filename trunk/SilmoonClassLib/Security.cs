using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Security
{
    public class SmHash
    {
        public SmHash()
        {

        }

        public static string Get16MD5(string strSource)
        {
            //new 
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            //获取密文字节数组 
            byte[] bytResult = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(strSource));

            //转换成字符串，并取9到25位 
            string strResult = BitConverter.ToString(bytResult, 4, 8);
            //转换成字符串，32位 
            //string strResult = BitConverter.ToString(bytResult); 

            //BitConverter转换出来的字符串会在每个字符中间产生一个分隔符，需要去除掉 
            strResult = strResult.Replace("-", "");
            return strResult;
        }
        //// <summary> 
        /// 进行MD5的32位加密
        /// </summary> 
        /// <param name="strSource">需要加密的明文</param> 
        /// <returns>返回32位加密结果</returns> 
        public static string Get32MD5(string strSource)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strSource, "MD5");
        }
    }
}

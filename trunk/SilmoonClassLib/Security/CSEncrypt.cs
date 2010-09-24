using System;
using System.IO;
using System.Security.Cryptography;

using System.Text;

namespace Silmoon.Security
{
    /// <summary>   
    /// 对称加密算法类   
    /// </summary>   
    public class CSEncrypt:IDisposable
    {
        private SymmetricAlgorithm mobjCryptoService;
        private string Key;
        /// <summary>   
        /// 对称加密类的构造函数   
        /// </summary>   
        public CSEncrypt()
        {
            mobjCryptoService = new RijndaelManaged();
            Key = "8UFCy76G7jH7yuBI0456lhj!y6&(*jkP8FtmaTuz(%&hj9H$ilJ$75&fvHx*h%(H";
        }
        /// <summary>   
        /// 获得密钥   
        /// </summary>   
        /// <returns>密钥</returns>   
        private byte[] GetLegalKey()
        {
            string sTemp = Key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
                sTemp = sTemp.Substring(0, KeyLength);
            else if (sTemp.Length < KeyLength)
                sTemp = sTemp.PadRight(KeyLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }
        /// <summary>   
        /// 获得初始向量IV   
        /// </summary>   
        /// <returns>初试向量IV</returns>   
        private byte[] GetLegalIV()
        {
            string sTemp = "6Gfghj*Ghg7!UNI57i%$hjkE4Wk7%g6HJ($jhHBh(ughUb#er&!hg4ufb&95GUY8";
            mobjCryptoService.GenerateIV();
            byte[] bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
                sTemp = sTemp.Substring(0, IVLength);
            else if (sTemp.Length < IVLength)
                sTemp = sTemp.PadRight(IVLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }
        /// <summary>   
        /// 加密方法   
        /// </summary>   
        /// <param name="Source">待加密的串</param>   
        /// <returns>经过加密的串</returns>   
        public string Encrypto(string Source)
        {
            if (Source == "") return "";
            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
            MemoryStream ms = new MemoryStream();
            mobjCryptoService.Key = GetLegalKey();
            mobjCryptoService.IV = GetLegalIV();
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            byte[] bytOut = ms.ToArray();
            return EncryptString.EncryptSilmoonBinry(Convert.ToBase64String(bytOut));
        }
        /// <summary>   
        /// 解密方法   
        /// </summary>   
        /// <param name="Source">待解密的串</param>   
        /// <returns>经过解密的串</returns>   
        public string Decrypto(string Source)
        {
            try
            {
                if (EncryptString.DiscryptSilmoonBinry(Source) == "") return "";
                byte[] bytIn = Convert.FromBase64String(EncryptString.DiscryptSilmoonBinry(Source));
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                mobjCryptoService.Key = GetLegalKey();
                mobjCryptoService.IV = GetLegalIV();
                ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            catch { return ""; }
        }


        #region IDisposable 成员

        public void Dispose()
        {
            mobjCryptoService.Clear();
        }

        #endregion
    }
}

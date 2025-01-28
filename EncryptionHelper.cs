using System;

namespace EAACtrl
{
    internal static class EncryptionHelper
    {
        public static string Encrypt(string plainText)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(plainText);
            byte[] result;
            using (System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes("$6%ItQ9Eteh1zEPJ"));
                using (System.Security.Cryptography.TripleDESCryptoServiceProvider tripDes = new System.Security.Cryptography.TripleDESCryptoServiceProvider()
                {
                    Key = keys,
                    Mode = System.Security.Cryptography.CipherMode.ECB,
                    Padding = System.Security.Cryptography.PaddingMode.PKCS7
                })
                {
                    using (System.Security.Cryptography.ICryptoTransform transform = tripDes.CreateEncryptor())
                    {
                        result = transform.TransformFinalBlock(data, 0, data.Length);
                    }
                }
            }
            return Convert.ToBase64String(result);
        }
        public static string Decrypt(string encryptedText)
        {
            try
            {
                byte[] data = Convert.FromBase64String(encryptedText);
                byte[] result;
                using (System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
                {
                    byte[] keys = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes("$6%ItQ9Eteh1zEPJ"));
                    using (System.Security.Cryptography.TripleDESCryptoServiceProvider tripDes = new System.Security.Cryptography.TripleDESCryptoServiceProvider()
                    {
                        Key = keys,
                        Mode = System.Security.Cryptography.CipherMode.ECB,
                        Padding = System.Security.Cryptography.PaddingMode.PKCS7
                    })
                    {
                        using (System.Security.Cryptography.ICryptoTransform transform = tripDes.CreateDecryptor())
                        {
                            result = transform.TransformFinalBlock(data, 0, data.Length);
                        }
                    }
                }
                return System.Text.Encoding.UTF8.GetString(result);
            }
            catch (Exception)
            {
                return encryptedText; // pass back what was passed in
            }
        }
    }
}

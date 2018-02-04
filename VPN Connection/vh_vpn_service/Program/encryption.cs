using System;
using System.Security.Cryptography;
using System.Text;

namespace vh_vpn {
    class Encryption {

        private static string HASH_KEY = "V|-|C0m";
        private Logging logging = new Logging();
        public string Encrypt(string text) {
            byte[] data = UTF8Encoding.UTF8.GetBytes(text);
            using(MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider()) {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(HASH_KEY));
                using (TripleDESCryptoServiceProvider triDes = new TripleDESCryptoServiceProvider() {
                    Key = keys,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7 }
                ) {
                    ICryptoTransform transform = triDes.CreateEncryptor();
                    byte[] result = transform.TransformFinalBlock(data, 0, data.Length);
                    return Convert.ToBase64String(result, 0, result.Length);
                }
            }
        }

        public string Decrypt(string text) {
            text = text.Replace("@", "");
            text = text.Replace("^", "");
            byte[] data = Convert.FromBase64String(text);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider()) {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(HASH_KEY));
                using (TripleDESCryptoServiceProvider triDes = new TripleDESCryptoServiceProvider() {
                    Key = keys,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                }){
                    ICryptoTransform transform = triDes.CreateDecryptor();
                    byte[] result = transform.TransformFinalBlock(data, 0, data.Length);
                    return UTF8Encoding.UTF8.GetString(result);
                }
            }
        }
    }
}

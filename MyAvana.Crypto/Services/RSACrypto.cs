using MyAvanaApi.IServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyAvanaApi.Services
{
    public class RSACrypto : ICryptoService
    {
        private readonly string _key;
        public RSACrypto(string key)
        {
            _key = key;
        }
        public (string result, bool success, string error) Decrypt(string encryptedString)
        {
            try
            {
                string encryptionkey = _key;
                byte[] keybytes = Encoding.ASCII.GetBytes(encryptionkey.Length.ToString());
                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                byte[] encryptedData = Convert.FromBase64String(encryptedString.Replace(" ", "+"));
                PasswordDeriveBytes pwdbytes = new PasswordDeriveBytes(encryptionkey, keybytes);
                using (ICryptoTransform decryptrans = rijndaelCipher.CreateDecryptor(pwdbytes.GetBytes(32), pwdbytes.GetBytes(16)))
                {
                    using (MemoryStream mstrm = new MemoryStream(encryptedData))
                    {
                        using (CryptoStream cryptstm = new CryptoStream(mstrm, decryptrans, CryptoStreamMode.Read))
                        {
                            byte[] plainText = new byte[encryptedData.Length];
                            int decryptedCount = cryptstm.Read(plainText, 0, plainText.Length);
                            return (Encoding.Unicode.GetString(plainText, 0, decryptedCount), true, "");
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                return ("", false, Ex.Message);
            }
        }

        public (string result, bool success, string error) Encrypt(string decryptedString)
        {
            try
            {
                string encryptionkey = _key;
                byte[] keybytes = Encoding.ASCII.GetBytes(encryptionkey.Length.ToString());
                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                byte[] plainText = Encoding.Unicode.GetBytes(decryptedString);
                PasswordDeriveBytes pwdbytes = new PasswordDeriveBytes(encryptionkey, keybytes);
                using (ICryptoTransform encryptrans = rijndaelCipher.CreateEncryptor(pwdbytes.GetBytes(32), pwdbytes.GetBytes(16)))
                {
                    using (MemoryStream mstrm = new MemoryStream())
                    {
                        using (CryptoStream cryptstm = new CryptoStream(mstrm, encryptrans, CryptoStreamMode.Write))
                        {
                            cryptstm.Write(plainText, 0, plainText.Length);
                            cryptstm.Close();
                            return (Convert.ToBase64String(mstrm.ToArray()), true, "");
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                return ("", false, Ex.Message);
            }
        }
        public string GetRandomKey(int len)
        {
            int maxSize = len;
            char[] chars = new char[30];
            string a;
            a = "1234567890";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data) { result.Append(chars[b % (chars.Length)]); }
            return result.ToString();
        }
    }
}

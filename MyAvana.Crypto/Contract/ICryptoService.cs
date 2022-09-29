using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaApi.IServices
{
    public interface ICryptoService
    {
        (string result, bool success, string error) Encrypt(string decryptedString);
        (string result, bool success, string error) Decrypt(string encryptedString);
        string GetRandomKey(int len);
    }
}

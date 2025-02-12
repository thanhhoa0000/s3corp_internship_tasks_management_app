using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TaskManagementApp.SharedLibraries.BaseSharedLibraries.Helper
{
    public class SecretHandler
    {
        private readonly byte[] _aesKey;
        private readonly byte[] _aesIV;

        public SecretHandler(string key, string iv)
        {
            _aesKey = Convert.FromBase64String(key);
            _aesIV = Convert.FromBase64String(iv);
        }

        public string ReadSecret(string path)
        {
            if (!File.Exists(path))
                throw new Exception($"Secret not found: {path}");

            return File.ReadAllText(path).Trim();
        }

        public string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = _aesKey;
                aes.IV = _aesIV;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var writer = new StreamWriter(cs))
                    {
                        writer.Write(plainText);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public string Decrypt(string encryptedText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = _aesKey;
                aes.IV = _aesIV;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cs))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}

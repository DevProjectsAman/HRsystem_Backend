using System.Security.Cryptography;
using System.Text;

namespace HRsystem.Api.Shared.EncryptText
{
 
    public static class SimpleCrypto
    {



     //   private static readonly string Key = "MyUltraSecretKey123!"; // put in config
                                                                     // here i want to to read the key from appsettings.json or environment variable named  Encryption  EncryptionKey


        private static string? _key;
        private static readonly object _lock = new();

        public static void Initialize(IConfiguration configuration)
        {
            lock (_lock)
            {
                // Prefer environment variable, fallback to appsettings.json
                _key = Environment.GetEnvironmentVariable("EncryptionKey") ??
                       configuration["Encryption:EncryptionKey"];

                if (string.IsNullOrWhiteSpace(_key))
                    throw new InvalidOperationException("Encryption key not found in environment variables or configuration.");
            }
        }

        private static string Key
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_key))
                    throw new InvalidOperationException("SimpleCrypto not initialized. Call Initialize() with configuration.");
                return _key;
            }
        }




        public static string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = SHA256.HashData(Encoding.UTF8.GetBytes(Key));
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            ms.Write(aes.IV, 0, aes.IV.Length);

            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(string cipherText)
        {
            var fullCipher = Convert.FromBase64String(cipherText);
            using var aes = Aes.Create();
            aes.Key = SHA256.HashData(Encoding.UTF8.GetBytes(Key));

            var iv = new byte[aes.BlockSize / 8];
            Array.Copy(fullCipher, 0, iv, 0, iv.Length);
            aes.IV = iv;

            var cipher = new byte[fullCipher.Length - iv.Length];
            Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(cipher);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
    }

}

using System.Security.Cryptography;
using System.Text;

namespace Pangea.Shared.Security.Services
{
    public class CryptingService
    {
        #region Public Methods

        public string Encrypt(string password, string input)
        {
            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            var bytesEncrypted = AESEncrypt(bytesToBeEncrypted, password);

            return Convert.ToBase64String(bytesEncrypted);
        }

        public string Decrypt(string password, string input)
        {
            var bytesToBeDecrypted = Convert.FromBase64String(input);
            var bytesDecrypted = AESDecrypt(bytesToBeDecrypted, password);

            return Encoding.UTF8.GetString(bytesDecrypted);
        }

        #endregion

        #region Internal Methods

        private byte[] AESEncrypt(byte[] bytesToBeEncrypted, string password)
        {
            byte[]? encryptedBytes = null;
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using MemoryStream ms = new();
            {
                using var AES = Aes.Create();
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(password, saltBytes);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Padding = PaddingMode.Zeros;

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        private byte[] AESDecrypt(byte[] bytesToBeDecrypted, string password)
        {
            byte[]? decryptedBytes = null;
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using MemoryStream ms = new();
            {
                using var AES = Aes.Create();
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(password, saltBytes);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Padding = PaddingMode.Zeros;

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        #endregion
    }
}

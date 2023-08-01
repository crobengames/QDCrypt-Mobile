using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QDCrypt.Models
{
    public static class QDCryptography
    {
        public static string GetSHA256Hash(string dataToHash)
        {
            // Setup SHA
            string hash = string.Empty;

            // Compute hash
            byte[] crypto = SHA256.HashData(Encoding.UTF8.GetBytes(dataToHash).AsSpan(0, Encoding.UTF8.GetByteCount(dataToHash)));

            // Convert to hex
            foreach (byte bit in crypto)
                hash += bit.ToString("x2");

            return hash;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="key"></param> 32 character long string = 256 bits
        /// <param name="iv"></param> 16 character long string = 128 bits
        /// <returns></returns>
        public static string AesEcnrypt(string plainText, string key, string iv)
        {
            byte[] result;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new())
                {
                    using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        result = msEncrypt.ToArray();
                    }
                }

                encryptor.Dispose();
            }

            return Convert.ToBase64String(result);
        }

        public static string AesDecrypt(string cipher, string key, string iv)
        {

            byte[] cipherBytes = Convert.FromBase64String(cipher);
            string result;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.KeySize = 256;
                aesAlg.BlockSize = 128;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            result = srDecrypt.ReadToEnd();
                        }
                    }
                }

                decryptor.Dispose();
            }

            return result;
        }
    }
}

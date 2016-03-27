using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Esb.Management.Shell
{
    public class CryptographyService : ICryptographyService
    {
	    public string TripleDESEncrypt(string plain, string key)
        {
            Guard.AgainstNullOrEmptyString(plain, "plain");
            Guard.AgainstNullOrEmptyString(key, "key");

            return Convert.ToBase64String(GetEncryptedBytes(key, plain.Length, Encoding.UTF8.GetBytes(plain)));
        }

        private static byte[] GetEncryptedBytes(string key, int plainLength, byte[] plainBytes)
        {
            byte[] encryptedBytes;

            using (var ms = new MemoryStream((plainLength * 2) - 1))
            using (
                var cs = new CryptoStream(ms, TripleDESCryptoServiceProvider(key).CreateEncryptor(),
                                          CryptoStreamMode.Write))
            {
                cs.Write(plainBytes, 0, plainBytes.Length);

                cs.FlushFinalBlock();

                encryptedBytes = new byte[(int)ms.Length];

                ms.Position = 0;

                ms.Read(encryptedBytes, 0, (int)ms.Length);
            }
            return encryptedBytes;
        }

        private static TripleDESCryptoServiceProvider TripleDESCryptoServiceProvider(string key)
        {
            return new TripleDESCryptoServiceProvider
                       {
                           IV = new byte[8],
                           Key =
                               new PasswordDeriveBytes(key, new byte[0]).CryptDeriveKey("RC2", "MD5", 128, new byte[8])
                       };
        }

        public string TripleDESDecrypt(string encrypted, string key)
        {
            Guard.AgainstNullOrEmptyString(encrypted, "secure");
            Guard.AgainstNullOrEmptyString(key, "key");

            return Encoding.UTF8.GetString(GetPlainBytes(key, encrypted.Length, Convert.FromBase64String(encrypted)));
        }

	    private static byte[] GetPlainBytes(string key, int secureLength, byte[] encryptedBytes)
        {
            byte[] plainBytes;

            using (var ms = new MemoryStream(secureLength))
            using (
                var cs = new CryptoStream(ms, TripleDESCryptoServiceProvider(key).CreateDecryptor(),
                                          CryptoStreamMode.Write))
            {
                cs.Write(encryptedBytes, 0, encryptedBytes.Length);

                cs.FlushFinalBlock();

                plainBytes = new byte[(int)ms.Length];

                ms.Position = 0;

                ms.Read(plainBytes, 0, (int)ms.Length);
            }

            return plainBytes;
        }
    }
}
using System;
using System.IO;
using System.Security.Cryptography;


namespace LicenseManager.Utils
{
    public class Encryption
    {
        public static string EncryptPwd(string clearText)
        {
            byte[] clearBytes;
            byte[] encryptedHash;

            if (clearText != String.Empty)
            {
                SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider();
                clearBytes = System.Text.Encoding.UTF8.GetBytes(clearText);
                encryptedHash = SHA1.ComputeHash(clearBytes);
                SHA1.Clear();
                return Convert.ToBase64String(encryptedHash);
            }
            else
            {
                return String.Empty;
            }
        }

        public static string EncryptData(string Password, string clearText)
        {
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));

            return Convert.ToBase64String(encryptedData);
        }

        public static string DecryptData(string Password, string cipherText)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));

            return System.Text.Encoding.Unicode.GetString(decryptedData);
        }

        public static void EncryptFile(string Password, string fileIn, string fileOut)
        {
            FileStream fsIn = new FileStream(fileIn, FileMode.Open, FileAccess.Read);
            FileStream fsOut = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.Write);

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

            Rijndael alg = Rijndael.Create();
            alg.Key = pdb.GetBytes(32);
            alg.IV = pdb.GetBytes(16);

            CryptoStream cs = new CryptoStream(fsOut, alg.CreateEncryptor(), CryptoStreamMode.Write);
            int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int bytesRead;
            do
            {
                bytesRead = fsIn.Read(buffer, 0, bufferLen);
                cs.Write(buffer, 0, bytesRead);
            } while (bytesRead != 0);
            cs.Close();
            fsIn.Close();
        }

        public static void DecryptFile(string Password, string fileIn, string fileOut)
        {
            FileStream fsIn = new FileStream(fileIn, FileMode.Open, FileAccess.Read);
            FileStream fsOut = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.Write);

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

            Rijndael alg = Rijndael.Create();
            alg.Key = pdb.GetBytes(32);
            alg.IV = pdb.GetBytes(16);

            CryptoStream cs = new CryptoStream(fsOut, alg.CreateDecryptor(), CryptoStreamMode.Write);
            int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int bytesRead;
            do
            {
                bytesRead = fsIn.Read(buffer, 0, bufferLen);
                cs.Write(buffer, 0, bytesRead);
            } while (bytesRead != 0);
            cs.Close();
            fsIn.Close();
        }

        private static byte[] Encrypt(byte[] clearData, string Password)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

            return Encrypt(clearData, pdb.GetBytes(32), pdb.GetBytes(16));
        }

        private static byte[] Decrypt(byte[] cipherData, string Password)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

            return Decrypt(cipherData, pdb.GetBytes(32), pdb.GetBytes(16));
        }

        private static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;

            CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(clearData, 0, clearData.Length);
            cs.Close();

            byte[] encryptedData = ms.ToArray();
            return encryptedData;
        }

        private static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();

            Rijndael alg = Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;

            CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(cipherData, 0, cipherData.Length);
            cs.Close();

            byte[] decryptedData = ms.ToArray();
            return decryptedData;
        }
    }
}
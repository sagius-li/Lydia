using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace OCG.Security.Operation
{
    public class GenericAESCryption
    {
        public static string EncryptString(string stringToEncrypt)
        {
            return EncryptString(stringToEncrypt, "OCGDESecurityAES");
        }

        public static string EncryptString(string stringToEncrypt, string key)
        {
            //Set up the encryption objects
            using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(key)))
            {
                byte[] sourceBytes = Encoding.ASCII.GetBytes(stringToEncrypt);
                ICryptoTransform ictE = acsp.CreateEncryptor();

                //Set up stream to contain the encryption
                MemoryStream msS = new MemoryStream();

                //Perform the encrpytion, storing output into the stream
                CryptoStream csS = new CryptoStream(msS, ictE, CryptoStreamMode.Write);
                csS.Write(sourceBytes, 0, sourceBytes.Length);
                csS.FlushFinalBlock();

                //sourceBytes are now encrypted as an array of secure bytes
                byte[] encryptedBytes = msS.ToArray(); //.ToArray() is important, don't mess with the buffer

                //return the encrypted bytes as a BASE64 encoded string
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        public static string DecryptString(string stringToEncrypt)
        {
            return DecryptString(stringToEncrypt, "OCGDESecurityAES");
        }

        public static string DecryptString(string stringToDecrypt, string key)
        {
            //Set up the encryption objects
            using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(key)))
            {
                byte[] RawBytes = Convert.FromBase64String(stringToDecrypt);
                ICryptoTransform ictD = acsp.CreateDecryptor();

                //RawBytes now contains original byte array, still in Encrypted state

                //Decrypt into stream
                MemoryStream msD = new MemoryStream(RawBytes, 0, RawBytes.Length);
                CryptoStream csD = new CryptoStream(msD, ictD, CryptoStreamMode.Read);
                //csD now contains original byte array, fully decrypted

                //return the content of msD as a regular string
                return (new StreamReader(csD)).ReadToEnd();
            }
        }

        private static AesCryptoServiceProvider GetProvider(byte[] key)
        {
            AesCryptoServiceProvider result = new AesCryptoServiceProvider();
            result.BlockSize = 128;
            result.KeySize = 128;
            result.Mode = CipherMode.CBC;
            result.Padding = PaddingMode.PKCS7;

            // use OCGMobileService as initial vector
            result.GenerateIV();
            result.IV = new byte[] { 79, 67, 71, 77, 111, 98, 105, 108, 101, 83, 101, 114, 118, 105, 99, 101 };

            byte[] RealKey = GetKey(key, result);
            result.Key = RealKey;
            // result.IV = RealKey;
            return result;
        }

        private static byte[] GetKey(byte[] suggestedKey, SymmetricAlgorithm p)
        {
            byte[] kRaw = suggestedKey;
            List<byte> kList = new List<byte>();

            for (int i = 0; i < p.LegalKeySizes[0].MinSize; i += 8)
            {
                kList.Add(kRaw[(i / 8) % kRaw.Length]);
            }
            byte[] k = kList.ToArray();
            return k;
        }
    }
}

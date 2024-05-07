using System.Security.Cryptography;

namespace Gateway.Model.Password
{
    public static class EncryptionHelper
    {
       
        //private static readonly string EncryptionKey = GenerateRandomKey(256);

        //public static string Encrypt(string plainText)
        //{
        //    using (Aes aesAlg = Aes.Create())
        //    {
        //        aesAlg.Key = Convert.FromBase64String(EncryptionKey);
        //        aesAlg.IV = GenerateRandomIV(); // Generate a random IV for each encryption

        //        aesAlg.Padding = PaddingMode.PKCS7; // Set the padding mode to PKCS7

        //        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        //        using (MemoryStream msEncrypt = new MemoryStream())
        //        {
        //            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        //            {
        //                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
        //                {
        //                    swEncrypt.Write(plainText);
        //                }
        //            }
        //            return Convert.ToBase64String(aesAlg.IV.Concat(msEncrypt.ToArray()).ToArray());
        //        }
        //    }
        //}

        //public static string Decrypt(string cipherText)
        //{
        //    byte[] cipherBytes = Convert.FromBase64String(cipherText);

        //    using (Aes aesAlg = Aes.Create())
        //    {
        //        aesAlg.Key = Convert.FromBase64String(EncryptionKey);
        //        aesAlg.IV = cipherBytes.Take(16).ToArray();

        //        aesAlg.Padding = PaddingMode.PKCS7; // Set the padding mode to PKCS7

        //        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        //        using (MemoryStream msDecrypt = new MemoryStream(cipherBytes, 16, cipherBytes.Length - 16))
        //        {
        //            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
        //            {
        //                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
        //                {
        //                    return srDecrypt.ReadToEnd();
        //                }
        //            }
        //        }
        //    }
        //}

        //private static byte[] GenerateRandomIV()
        //{
        //    using (Aes aesAlg = Aes.Create())
        //    {
        //        aesAlg.GenerateIV();
        //        return aesAlg.IV;
        //    }
        //}

        //private static string GenerateRandomKey(int keySizeInBits)
        //{
        //    // Convert the key size to bytes
        //    int keySizeInBytes = keySizeInBits / 8;

        //    // Create a byte array to hold the random key
        //    byte[] keyBytes = new byte[keySizeInBytes];

        //    // Use a cryptographic random number generator to fill the byte array
        //    using (var rng = new RNGCryptoServiceProvider())
        //    {
        //        rng.GetBytes(keyBytes);
        //    }

        //    // Convert the byte array to a base64-encoded string for storage
        //    return Convert.ToBase64String(keyBytes);
        //}
        public static string GenerateKey()
        {
            string keybase64 = ""; 
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.KeySize = 256;
                aesAlg.GenerateKey();
                keybase64 = Convert.ToBase64String(aesAlg.Key);
            }
            return keybase64;
        }

        public static string Encrypt(string plainText,string Key, string IVKey)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Padding = PaddingMode.Zeros;
                aesAlg.Key = Convert.FromBase64String(Key);
                //aesAlg.GenerateIV();
                aesAlg.IV = Convert.FromBase64String(IVKey);
                ICryptoTransform cryptoTransform = aesAlg.CreateEncryptor();
                byte[] encryptedData;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        encryptedData = ms.ToArray();
                        //remove white space
                        encryptedData = encryptedData.Where(b => b != 0).ToArray();
                    }
                   
                }
                return Convert.ToBase64String(encryptedData);
            }
        }

        //public static string Decrypt(string CipherText, string Key, string IVKey)
        //{
        //    using (Aes aesAlg = Aes.Create())
        //    {
        //        aesAlg.Padding = PaddingMode.Zeros;
        //        aesAlg.Key = Convert.FromBase64String(Key);
        //        aesAlg.IV = Convert.FromBase64String(IVKey);

        //        ICryptoTransform decryptoTransform = aesAlg.CreateDecryptor();
        //        string plainText="";
        //        byte[] ciper = Convert.FromBase64String(CipherText);

        //        using (MemoryStream ms = new MemoryStream(ciper))
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, decryptoTransform, CryptoStreamMode.Read))
        //            {
        //                using (StreamReader sr = new StreamReader(cs))
        //                {
        //                    plainText = sr.ReadToEnd();
        //                    //remove white space
        //                    plainText = plainText.Replace("\0","");
        //                }
                        
        //            }

        //        }
        //        return plainText;
        //    }
        //}
        
        public static string Decrypt(string CipherText, string Key, string IVKey)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Padding = PaddingMode.Zeros;
                aesAlg.Key = Convert.FromBase64String(Key);
                aesAlg.IV = Convert.FromBase64String(IVKey);

                ICryptoTransform decryptoTransform = aesAlg.CreateDecryptor();
                string plainText="";
                byte[] ciper = Convert.FromBase64String(CipherText);

                using (MemoryStream ms = new MemoryStream(ciper))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptoTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            plainText = sr.ReadToEnd();
                            //remove white space
                            plainText = plainText.Replace("\0","");
                            plainText = plainText.Replace("\a","");
                        }
                        
                    }

                }
                return plainText;
            }
        }

    }
}

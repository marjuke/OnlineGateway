using System.Security.Cryptography;
using System.Text;

namespace Gateway.Model.Password
{
    public static class PasswordDecryptor
    {
        public static string GetPassword(string password)
        {
            string key = "apTN9ruR3hef1d1cQ4zylWRSeXmXwwKZg98NiKdrEmI=";
            var KeyByte = Convert.FromBase64String(key);
            var passHexa = Convert.FromBase64String(password);
            var d = passHexa.Take(16).ToArray();
            var s = passHexa.Skip(16).ToArray();
            var result = Decrypt(s, KeyByte, d);
            string modifiedresult = RemoveFirstAndLast(result, 6, 12);
            return modifiedresult;
        }
        static string RemoveFirstAndLast(string input, int charactersToRemoveFromStart, int charactersToRemoveFromEnd)
        {
            // Check if the string is long enough
            if (input.Length >= charactersToRemoveFromStart + charactersToRemoveFromEnd)
            {
                // Remove characters from the start and end
                return input.Substring(charactersToRemoveFromStart, input.Length - charactersToRemoveFromStart - charactersToRemoveFromEnd);
            }
            else
            {
                // Handle the case where the string is too short
                return "Error: String is too short to perform the specified removal.";
            }
        }
        static string Decrypt(byte[] ciphertext, byte[] key, byte[] iv)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = key;
            aesAlg.IV = iv;
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            byte[] decryptedBytes;
            using (var msDecrypt = new System.IO.MemoryStream(ciphertext))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var msPlain = new System.IO.MemoryStream())
                    {
                        csDecrypt.CopyTo(msPlain);
                        decryptedBytes = msPlain.ToArray();
                    }
                }
            }
            return Encoding.UTF8.GetString(decryptedBytes);
        }
        // encrypt password
        //public static string Encrypt(string plainText, byte[] key, byte[] iv)
        //{
        //    using Aes aesAlg = Aes.Create();
        //    aesAlg.Key = key;
        //    aesAlg.IV = iv;
        //    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
        //    byte[] encrypted;
        //    using (var msEncrypt = new System.IO.MemoryStream())
        //    {
        //        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        //        {
        //            using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
        //            {
        //                swEncrypt.Write(plainText);
        //            }
        //        }
        //        encrypted = msEncrypt.ToArray();
        //    }
        //    return Convert.ToBase64String(encrypted);
        //}

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MAPICore.Utils
{
    public class Encryptor
    {
        private readonly string _key = "E546C8DF278CD5931069B522E695D4F2";

        public string Encrypt(string text)
        {
            var key = Encoding.UTF8.GetBytes(_key);
            using (var aseAlg = Aes.Create())
            {
                using (var encryptor = aseAlg.CreateEncryptor(key, aseAlg.IV))
                {
                    using (var Msencrypt = new MemoryStream())
                    {
                        using(var csencrypt = new CryptoStream(Msencrypt,encryptor,CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csencrypt))
                        {
                            swEncrypt.Write(text);
                        }
                        var iv = aseAlg.IV;

                        var decryptedContent = Msencrypt.ToArray();
                        var result = new byte[iv.Length + decryptedContent.Length];
                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);
                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        public string Decrypt(string encryptedText)
        {
            try
            {
                var fullCipher = Convert.FromBase64String(encryptedText);
                var iv = new byte[16];
                var cipher = new byte[fullCipher.Length - iv.Length];

                Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);

                Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - iv.Length);

                var key = Encoding.UTF8.GetBytes(_key);
                string result = "";
                using (var aseAlg = Aes.Create())
                {
                    using (var decryptor = aseAlg.CreateDecryptor(key, iv))
                    {
                        using (var Msencrypt = new MemoryStream(cipher))
                        {
                            using (var csdecrypt = new CryptoStream(Msencrypt, decryptor, CryptoStreamMode.Read))
                            {
                                using (var srDecryptor = new StreamReader(csdecrypt))
                                {
                                    result = srDecryptor.ReadToEnd();
                                }
                            }
                        }
                    }
                }

                return result;
            }catch(Exception ex)
            {
                return null;
            }
        }

    }

}

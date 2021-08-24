using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace spk_dot
{
    class RsaHelper
    {
        public static RSA ReadPubKeyFromFile(string filename)
        {
            string pemContents = System.IO.File.ReadAllText(filename);
            var rsaPublicKey = RSA.Create();
            rsaPublicKey.ImportFromPem(pemContents);
            return rsaPublicKey;

        }

        public static RSA ReadPrivateKeyFromFile(string filename)
        {
            string pemContents = System.IO.File.ReadAllText(filename);
            const string RsaPrivateKeyHeader = "-----BEGIN RSA PRIVATE KEY-----";
            const string RsaPrivateKeyFooter = "-----END RSA PRIVATE KEY-----";

            if (pemContents.StartsWith(RsaPrivateKeyHeader))
            {
                int endIdx = pemContents.IndexOf(
                    RsaPrivateKeyFooter,
                    RsaPrivateKeyHeader.Length,
                    StringComparison.Ordinal);

                string base64 = pemContents.Substring(
                    RsaPrivateKeyHeader.Length,
                    endIdx - RsaPrivateKeyHeader.Length);

                byte[] der = Convert.FromBase64String(base64);
                RSA rsa = RSA.Create();
                rsa.ImportRSAPrivateKey(der, out _);
                return rsa;
            }

            // "BEGIN PRIVATE KEY" (ImportPkcs8PrivateKey),
            // "BEGIN ENCRYPTED PRIVATE KEY" (ImportEncryptedPkcs8PrivateKey),
            // "BEGIN PUBLIC KEY" (ImportSubjectPublicKeyInfo),
            // "BEGIN RSA PUBLIC KEY" (ImportRSAPublicKey)
            // could any/all be handled here.
            throw new InvalidOperationException();
        }


        public static byte[] Encrypt(RSA pubKey, string text)
        {
            byte[] encoded = Encoding.ASCII.GetBytes(text);
            byte[] encrypted = pubKey.Encrypt(encoded, RSAEncryptionPadding.Pkcs1);
            return encrypted;
        }

        public static string Decrypt(RSA privKey, byte[] encrypted)
        {
            byte[] decrypted = privKey.Decrypt(encrypted, RSAEncryptionPadding.Pkcs1);
            String decoded = Encoding.ASCII.GetString(decrypted);
            return decoded;
        }
    }
}

using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Text;

namespace KeyVaultDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            FetchSecretUsingAppId();
            Uri _url = new Uri("https://puneetkeyvault.vault.azure.net/");
            GetSecretUsingManageIdentity(_url);
            UsingEncryptionKey(_url);

            Console.ReadKey();
        }

        private static void UsingEncryptionKey(Uri _url)
        {
            TokenCredential cred = new DefaultAzureCredential();
            KeyClient client = new KeyClient(_url, cred);
            string textToEncrypt = "Sample Text";

            var _key = client.GetKey("EncryptKey");

            // The CryptographyClient class is part of the Azure Key vault package
            // This is used to perform cryptographic operations with Azure Key Vault keys
            var crypto_client = new CryptographyClient(_key.Value.Id, cred);

            // We first need to take the bytes of the string that needs to be converted

            byte[] text_to_bytes = Encoding.UTF8.GetBytes(textToEncrypt);

            EncryptResult _result = crypto_client.Encrypt(EncryptionAlgorithm.RsaOaep, text_to_bytes);

            Console.WriteLine("The encrypted text");
            Console.WriteLine(Convert.ToBase64String(_result.Ciphertext));

            // Now lets decrypt the text
            // We first need to convert our Base 64 string of the Cipertext to bytes

            byte[] ciper_to_bytes = _result.Ciphertext;

            DecryptResult _text_decrypted = crypto_client.Decrypt(EncryptionAlgorithm.RsaOaep, ciper_to_bytes);

            Console.WriteLine(Encoding.UTF8.GetString(_text_decrypted.Plaintext));
        }

        private static void GetSecretUsingManageIdentity(Uri _url)
        {
            TokenCredential cred = new DefaultAzureCredential();

            SecretClient client = new SecretClient(_url, cred);

            var secret = client.GetSecret("dbpassword");

            Console.Write(secret.Value.Value);

            client.SetSecret("dbpassword", "ChangedData");

            client.SetSecret("Test", "Test");

            client.StartDeleteSecret("Test");
        }

        private static void FetchSecretUsingAppId()
        {
            string appId = "a36d46c9-9335-4137-9e2f-d0079fc15ff5";
            string tenantId = "9a0c6406-e26c-4288-a20c-4df2a8eb78a7";
            string secretId = "pbg-ZlEbO_.4GxC7.023jthP-oV0U_8o6s";
            string secretName = "dbpassword";
            Uri _url = new Uri("https://puneetkeyvault.vault.azure.net/");
            ClientSecretCredential cred = new ClientSecretCredential(tenantId, appId, secretId);
            SecretClient _client = new SecretClient(_url, cred);
            var secret = _client.GetSecret(secretName);
            Console.WriteLine(secret.Value.Value);
        }
    }
}

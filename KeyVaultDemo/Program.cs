using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;

namespace KeyVaultDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            FetchSecretUsingAppId();
            Uri _url = new Uri("https://puneetkeyvault.vault.azure.net/");

            Console.Read();
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

using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using Amazon.SecretsManager.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS.SecretManagerExample
{
    class Program
    {
        public static SecretsManagerCache cache;
        public static string GetCacheData(string secretName)
        {
            return cache.GetSecretString(secretName).Result;
        }
        public static string GetSecret(string secretName)
        {
            var config = new AmazonSecretsManagerConfig { RegionEndpoint = RegionEndpoint.USEast2 };            
            IAmazonSecretsManager client = new AmazonSecretsManagerClient("<aws-access-key-id>", "<aws-secret-access-key>", config);

            GetSecretValueRequest request = new GetSecretValueRequest()
            {
                SecretId = secretName,
                VersionStage = "AWSCURRENT" // VersionStage defaults to AWSCURRENT if unspecified.
            };
            GetSecretValueResponse response = null;
            try
            {
                response = client.GetSecretValueAsync(request).Result;
                cache = new SecretsManagerCache(client);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return response?.SecretString;
        }
        static void Main(string[] args)
        {
            var secretValue = GetSecret("<secret-name>");
            var secretValueFromCache = Program.GetCacheData("<secret-name>");

            Console.WriteLine("Secret fetched from AWS Secret Manager : " + secretValue);
            Console.WriteLine("Secret fetched from AWS Cache Manager : " + secretValueFromCache);
            Console.ReadLine();
        }
    }

}

﻿using Amazon.SecretsManager.Model;
using Amazon.SecretsManager;
using Amazon;
using Customers.Microservice.Domain.SeedWork;
using Newtonsoft.Json;

namespace Customers.Microservice.Infrastructure.External
{
    public static class AWS
    {
        public static class SecretManager
        {
            public static async Task<Dictionary<string, string>> GetSecret(string? secret = null, string? region = null)
            {
                GetSecretValueResponse response;

                try
                {
                    response = await new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region ?? Constant.AWS.SecretManager.region))
                                  .GetSecretValueAsync(new GetSecretValueRequest { SecretId = secret ?? Constant.AWS.SecretManager.secretName });
                }
                catch (Exception e)
                {
                    throw;
                }

                return JsonConvert.DeserializeObject<Dictionary<string, string>>(response.SecretString) ?? [];
            }
        }
    }
}

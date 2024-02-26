using Amazon.SecretsManager.Model;
using Amazon.SecretsManager;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Microservice.Infrastructure.External
{
    public interface IAWS
    {
        Task<Dictionary<string, string>> GetSecretsFromSecretManager(string? secret = null, string? region = null);
    }
}

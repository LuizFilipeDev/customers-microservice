using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Microservice.Domain.SeedWork
{
    public static class Constant
    {
        public static class RequestLimiter
        {
            public static readonly string policy = "fixed";
            public static readonly int permitLimit = 5;
            public static readonly TimeSpan window = TimeSpan.FromSeconds(1);
        }

        public static class BearerToken
        {
            public static readonly TimeSpan expiration = TimeSpan.FromMinutes(30);
        }

        public static class AWS
        {
            public static class SecretManager
            {
                public static readonly string secretName = "customer.api";
                public static readonly string region = "sa-east-1";
            }
        }
    }
}

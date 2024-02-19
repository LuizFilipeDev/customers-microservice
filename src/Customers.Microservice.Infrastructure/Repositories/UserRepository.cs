using Customers.Microservice.Domain.Aggregates.User;
using Customers.Microservice.Infrastructure.External;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Customers.Microservice.Infrastructure.Repositories
{
    public class UserRepository: IUserRepository
    {
        public async Task<bool> SelectByNameAndPasswordAsync(string? name, string? password)
        {
            //Open your connection using Context folder or consume external services using External folder and execute your actions.
            Dictionary<string, string> dictionary = await AWS.SecretManager.GetSecret();

            //Example of rule to API login works
            foreach (KeyValuePair<string, string> keyValuePair in dictionary)
            {
                if(keyValuePair.Key == name && keyValuePair.Value == password)
                    return true;
            }

            return false;
        }
    }
}

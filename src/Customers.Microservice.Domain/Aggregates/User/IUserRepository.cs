using Customers.Microservice.Domain.Aggregates.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Microservice.Domain.Aggregates.User
{
    public interface IUserRepository
    {
        Task<bool> SelectByNameAndPasswordAsync(string name, string password);
    }
}

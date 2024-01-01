using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Microservice.Domain.Aggregates.User
{
    public interface IUser
    {
        public string? Name { get; set; }
        public string? Password { get; set; }
    }
}

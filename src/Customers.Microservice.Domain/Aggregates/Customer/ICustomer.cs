using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Customers.Microservice.Domain.Aggregates.Customer
{
    public interface ICustomer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}

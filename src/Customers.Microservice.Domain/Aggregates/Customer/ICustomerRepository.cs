using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Microservice.Domain.Aggregates.Customer
{
    public interface ICustomerRepository
    {
        int Insert(ICustomer customer);
        bool Update(int id, ICustomer customer);
        bool Delete(int id);
        ICustomer SelectById(int id);
        List<ICustomer> Select();
    }
}

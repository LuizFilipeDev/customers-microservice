using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Microservice.Domain.Aggregates.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public List<ICustomer> Select()
        {
            //Domain rules/logic

            return _customerRepository.Select();
        }

        public ICustomer SelectById(int id)
        {
            //Domain rules/logic

            return _customerRepository.SelectById(id);
        }

        public int Insert(ICustomer customer)
        {
            //Domain rules/logic

            return _customerRepository.Insert(customer);
        }

        public bool Update(int id, ICustomer customer)
        {
            //Domain rules/logic

            return _customerRepository.Update(id, customer);
        }

        public bool Delete(int id)
        {
            //Domain rules/logic

            return _customerRepository.Delete(id);
        }
    }
}

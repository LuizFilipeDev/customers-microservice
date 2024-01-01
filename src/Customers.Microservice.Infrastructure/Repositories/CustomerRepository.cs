using Customers.Microservice.Domain;
using Customers.Microservice.Domain.Aggregates.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Microservice.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerInMemory _customerInMemory;

        public CustomerRepository(CustomerInMemory customerInMemory){
            _customerInMemory = customerInMemory;
        }
        
        public List<ICustomer> Select()
        {
            //Open your connection using Context folder or consume external services using External folder and execute your actions.

            return _customerInMemory.customers;
        }

        public ICustomer SelectById(int id)
        {
            //Open your connection using Context folder or consume external services using External folder and execute your actions.
            return _customerInMemory?.customers?.Find(x => x.Id == id) ?? new Customer();
        }

        public int Insert(ICustomer customer)
        {
            //Open your connection using Context folder or consume external services using External folder and execute your actions.
            int newId = _customerInMemory.customers.Count() + 1;
            _customerInMemory.customers.Add(new Customer{ Id = newId, Name = customer.Name});
            return newId;
        }

        public bool Update(int id, ICustomer customer)
        {
            //Open your connection using Context folder or consume external services using External folder and execute your actions.
            ICustomer currentCustomer = _customerInMemory?.customers?.Find(x => x.Id == id) ?? new Customer();

            if(currentCustomer.Id == default)
                return false;
            
            currentCustomer.Name = customer.Name;
            return true;
        }

        public bool Delete(int id)
        {
            //Open your connection using Context folder or consume external services using External folder and execute your actions.
            ICustomer currentCustomer = _customerInMemory?.customers?.Find(x => x.Id == id) ?? new Customer();

            if(currentCustomer.Id == default)
                return false;

            _customerInMemory?.customers?.RemoveAt(id);
            return true;
        }
    }
}

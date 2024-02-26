using Customers.Microservice.Domain;
using Customers.Microservice.Domain.Aggregates.Customer;

namespace Customers.Microservice.Infrastructure.Repositories
{
    public class CustomerRepository(CustomerInMemory customerInMemory) : ICustomerRepository
    {
        private readonly CustomerInMemory _customerInMemory = customerInMemory;

        public List<ICustomer> Select()
        {
            //Open your connection using Context folder or consume external services using External folder and execute your actions.

            return _customerInMemory.Customers;
        }

        public ICustomer SelectById(int id)
        {
            //Open your connection using Context folder or consume external services using External folder and execute your actions.
            return _customerInMemory?.Customers?.Find(x => x.Id == id) ?? new Customer();
        }

        public int Insert(ICustomer customer)
        {
            //Open your connection using Context folder or consume external services using External folder and execute your actions.
            _customerInMemory.CountId += 1;
            _customerInMemory.Customers.Add(new Customer{ Id = _customerInMemory.CountId, Name = customer.Name});
            return _customerInMemory.CountId;
        }

        public bool Update(int id, ICustomer customer)
        {
            //Open your connection using Context folder or consume external services using External folder and execute your actions.
            ICustomer currentCustomer = _customerInMemory?.Customers?.Find(x => x.Id == id) ?? new Customer();

            if(currentCustomer.Id == default)
                return false;
            
            currentCustomer.Name = customer.Name;
            return true;
        }

        public bool Delete(int id)
        {
            //Open your connection using Context folder or consume external services using External folder and execute your actions.
            ICustomer currentCustomer = _customerInMemory?.Customers?.Find(x => x.Id == id) ?? new Customer();

            if(currentCustomer.Id == default)
                return false;

            _customerInMemory?.Customers?.RemoveAll(x => x.Id == id);
            return true;
        }
    }
}

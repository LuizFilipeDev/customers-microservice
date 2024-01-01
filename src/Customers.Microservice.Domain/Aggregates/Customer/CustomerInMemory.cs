using Customers.Microservice.Domain.Aggregates.Customer;

namespace Customers.Microservice.Domain{
    public class CustomerInMemory
    {
        public List<ICustomer> customers = [];

        public CustomerInMemory(){

            customers = new()
            {
                new Customer { Id = 1, Name = "Jorge" },
                new Customer { Id = 2, Name = "Alberto" },
                new Customer { Id = 3, Name = "Pedro" },
                new Customer { Id = 4, Name = "Paulo" },
                new Customer { Id = 5, Name = "Mateus" }
            };
        }
    }
}

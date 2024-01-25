using Customers.Microservice.Domain.Aggregates.Customer;
using Moq;
using Xunit;

namespace Customers.Microservice.Domain.Test
{
    public class DomainTest
    {
        public class CustomerTest{

            private readonly Mock<CustomerInMemory> _mockCustomerInMemory = new();

            [Fact]
            public void GivenANeedOfSelectAllCustomersWhenTryToSelectAllCustomersThenReturnAListOfCustomers()
            {   
                //Arrange
                var mockCustomerService = new Mock<ICustomerService>();
                mockCustomerService
                    .Setup(x => x.Select())
                    .Returns(() => _mockCustomerInMemory.Object.Customers);

                //Act
                List<ICustomer> result = mockCustomerService.Object.Select();
                
                //Assert
                Assert.NotNull(result);
                Assert.True(result.Count > default(int));
            }
        }
    }
}

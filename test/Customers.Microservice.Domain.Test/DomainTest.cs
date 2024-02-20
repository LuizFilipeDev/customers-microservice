using Customers.Microservice.Domain.Aggregates.Customer;
using Customers.Microservice.Domain.Aggregates.User;
using Customers.Microservice.Domain.SeedWork;
using Moq;
using Xunit;

namespace Customers.Microservice.Domain.Test
{
    public class DomainTest
    {
        public class CustomerTest
        {
            public class CustomerServiceTest
            {

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

                [Theory]
                [InlineData(5)]
                public void GivenANeedOfSelectACustomerByIdWhenTrySelectACustomerByIdThenReturnACustomer(int id)
                {
                    //Arrange
                    var mockCustomerService = new Mock<ICustomerService>();
                    mockCustomerService
                        .Setup(x => x.SelectById(id))
                        .Returns((int id) => new Customer { Id = id, Name = "Test" });

                    //Act
                    ICustomer result = mockCustomerService.Object.SelectById(id);

                    //Assert
                    Assert.NotNull(result);
                    Assert.NotNull(result.Name);
                    Assert.NotEmpty(result.Name);
                    Assert.Equal(id, result.Id);
                }

                [Fact]
                public void GivenANeedOfInsertACustomerWhenTryInsertACustomerThenReturnTheCustomerId()
                {
                    //Arrange
                    var mockCustomer = new Mock<ICustomer>();
                    mockCustomer.SetupProperty(x => x.Id);
                    mockCustomer.SetupProperty(x => x.Name);
                    mockCustomer.Object.Id = 5;
                    mockCustomer.Object.Name = "Test";

                    var mockCustomerService = new Mock<ICustomerService>();
                    mockCustomerService
                        .Setup(x => x.Insert(mockCustomer.Object))
                        .Returns((ICustomer customer) => customer.Id);

                    //Act
                    int result = mockCustomerService.Object.Insert(mockCustomer.Object);

                    //Assert
                    Assert.True(result != default);
                    Assert.Equal(mockCustomer.Object.Id, result);
                }

                [Theory]
                [InlineData(5)]
                public void GivenANeedOfUpdateACustomerWhenTryUpdateACustomerThenReturnTrue(int id)
                {
                    //Arrange
                    var mockCustomer = new Mock<ICustomer>();
                    mockCustomer.Object.Name = "Test";

                    var mockCustomerService = new Mock<ICustomerService>();
                    mockCustomerService
                        .Setup(x => x.Update(id, mockCustomer.Object))
                        .Returns((int id, ICustomer customer) => true);

                    //Act
                    bool result = mockCustomerService.Object.Update(id, mockCustomer.Object);

                    //Assert
                    Assert.True(result);
                }

                [Theory]
                [InlineData(5)]
                public void GivenANeedOfDeleteACustomerWhenTryDeleteACustomerThenReturnTrue(int id)
                {
                    //Arrange
                    var mockCustomerService = new Mock<ICustomerService>();
                    mockCustomerService
                        .Setup(x => x.Delete(id))
                        .Returns((int id) => true);

                    //Act
                    bool result = mockCustomerService.Object.Delete(id);

                    //Assert
                    Assert.True(result);
                }
            }

            public class CustomerRepositoryTest
            {
                private readonly Mock<CustomerInMemory> _mockCustomerInMemory = new();

                [Fact]
                public void GivenANeedOfSelectAllCustomersWhenTryToSelectAllCustomersThenReturnAListOfCustomers()
                {
                    //Arrange
                    var mockCustomerRepository = new Mock<ICustomerRepository>();
                    mockCustomerRepository
                        .Setup(x => x.Select())
                        .Returns(() => _mockCustomerInMemory.Object.Customers);

                    //Act
                    List<ICustomer> result = mockCustomerRepository.Object.Select();

                    //Assert
                    Assert.NotNull(result);
                    Assert.True(result.Count > default(int));
                }

                [Theory]
                [InlineData(5)]
                public void GivenANeedOfSelectACustomerByIdWhenTrySelectACustomerByIdThenReturnACustomer(int id)
                {
                    //Arrange
                    var mockCustomerRepository = new Mock<ICustomerRepository>();
                    mockCustomerRepository
                        .Setup(x => x.SelectById(id))
                        .Returns((int id) => new Customer { Id = id, Name = "Test" });

                    //Act
                    ICustomer result = mockCustomerRepository.Object.SelectById(id);

                    //Assert
                    Assert.NotNull(result);
                    Assert.NotNull(result.Name);
                    Assert.NotEmpty(result.Name);
                    Assert.Equal(id, result.Id);
                }

                [Fact]
                public void GivenANeedOfInsertACustomerWhenTryInsertACustomerThenReturnTheCustomerId()
                {
                    //Arrange
                    var mockCustomer = new Mock<ICustomer>();
                    mockCustomer.SetupAllProperties();
                    mockCustomer.Object.Id = 5;
                    mockCustomer.Object.Name = "Test";

                    var mockCustomerRepository = new Mock<ICustomerRepository>();
                    mockCustomerRepository
                        .Setup(x => x.Insert(mockCustomer.Object))
                        .Returns((ICustomer customer) => customer.Id);

                    //Act
                    int result = mockCustomerRepository.Object.Insert(mockCustomer.Object);

                    //Assert
                    Assert.True(result != default);
                    Assert.Equal(mockCustomer.Object.Id, result);
                }

                [Theory]
                [InlineData(5)]
                public void GivenANeedOfUpdateACustomerWhenTryUpdateACustomerThenReturnTrue(int id)
                {
                    //Arrange
                    var mockCustomer = new Mock<ICustomer>();
                    mockCustomer.Object.Name = "Test";

                    var mockCustomerRepository = new Mock<ICustomerRepository>();
                    mockCustomerRepository
                        .Setup(x => x.Update(id, mockCustomer.Object))
                        .Returns((int id, ICustomer customer) => true);

                    //Act
                    bool result = mockCustomerRepository.Object.Update(id, mockCustomer.Object);

                    //Assert
                    Assert.True(result);
                }

                [Theory]
                [InlineData(5)]
                public void GivenANeedOfDeleteACustomerWhenTryDeleteACustomerThenReturnTrue(int id)
                {
                    //Arrange
                    var mockCustomerRepository = new Mock<ICustomerRepository>();
                    mockCustomerRepository
                        .Setup(x => x.Delete(id))
                        .Returns((int id) => true);

                    //Act
                    bool result = mockCustomerRepository.Object.Delete(id);

                    //Assert
                    Assert.True(result);
                }
            }
        }

        public class UserTest
        {
            public class UserServiceTest
            {
                [Theory]
                [InlineData("fake.admin.authorized")]
                public async void GivenAValidUserWhenTryToVerifyIfIsValidThenReturnTrue(string fakeAdminAuthorized)
                {
                    //Arrange
                    var mockUser = new Mock<IUser>();
                    mockUser.SetupAllProperties();
                    mockUser.Object.Name = fakeAdminAuthorized;
                    mockUser.Object.Password = fakeAdminAuthorized;

                    var mockUserService = new Mock<IUserService>();
                    mockUserService
                        .Setup(x => x.IsValidUser(mockUser.Object))
                        .Returns((IUser user) => Task.FromResult(user.Name == fakeAdminAuthorized 
                                                              && user.Password == fakeAdminAuthorized));

                    //Act
                    bool result = await mockUserService.Object.IsValidUser(mockUser.Object);

                    //Assert
                    Assert.True(result);
                }

                [Theory]
                [InlineData("fake.admin.authorized", "fake.admin.unauthorized")]
                public async void GivenAInvalidUserWhenTryToVerifyIfIsValidThenReturnFalse(string fakeAdminAuthorized, string fakeAdminUnauthorized)
                {
                    //Arrange
                    var mockUser = new Mock<IUser>();
                    mockUser.SetupAllProperties();
                    mockUser.Object.Name = fakeAdminUnauthorized;
                    mockUser.Object.Password = fakeAdminUnauthorized;

                    var mockUserService = new Mock<IUserService>();
                    mockUserService
                        .Setup(x => x.IsValidUser(mockUser.Object))
                        .Returns((IUser user) => Task.FromResult(user.Name == fakeAdminAuthorized
                                                              && user.Password == fakeAdminAuthorized));

                    //Act
                    bool result = await mockUserService.Object.IsValidUser(mockUser.Object);

                    //Assert
                    Assert.False(result);
                }
            }

            public class UserRepositoryTest
            {
                [Theory]
                [InlineData("fake.admin.authorized")]
                public async void GivenAValidUserWhenTryToSelectByNameAndPasswordThenReturnTrue(string fakeAdminAuthorized)
                {
                    //Arrange
                    var mockUserRepository = new Mock<IUserRepository>();
                    mockUserRepository
                        .Setup(x => x.SelectByNameAndPasswordAsync(fakeAdminAuthorized, fakeAdminAuthorized))
                        .Returns((string name, string password) 
                        => Task.FromResult(name == fakeAdminAuthorized 
                        && password == fakeAdminAuthorized));

                    //Act
                    bool result = await mockUserRepository.Object.SelectByNameAndPasswordAsync(fakeAdminAuthorized, fakeAdminAuthorized);

                    //Assert
                    Assert.True(result);
                }

                [Theory]
                [InlineData("fake.admin.authorized", "fake.admin.unauthorized")]
                public async void GivenAInvalidUserWhenTryToSelectByNameAndPasswordThenReturnFalse(string fakeAdminAuthorized, string fakeAdminUnauthorized)
                {
                    //Arrange
                    var mockUserRepository = new Mock<IUserRepository>();
                    mockUserRepository
                        .Setup(x => x.SelectByNameAndPasswordAsync(fakeAdminUnauthorized, fakeAdminUnauthorized))
                        .Returns((string name, string password)
                        => Task.FromResult(name == fakeAdminAuthorized
                        && password == fakeAdminAuthorized));

                    //Act
                    bool result = await mockUserRepository.Object.SelectByNameAndPasswordAsync(fakeAdminUnauthorized, fakeAdminUnauthorized);

                    //Assert
                    Assert.False(result);
                }
            }
        }

        public class SeedWorkTest
        {
            public class CPFValidatorTest
            {
                [Fact]
                public void GivenAValidCPFWhenTryValidateTheCPFThenReturnTrue()
                {
                    //Arrange
                    //CPF created by 4Devs website (https://www.4devs.com.br/gerador_de_cpf)
                    string cpf = "948.138.820-48";

                    //Act
                    bool result = CPFValidator.IsCpf(cpf);

                    //Assert
                    Assert.True(result);
                }

                [Fact]
                public void GivenAInvalidCPFWhenTryValidateTheCPFThenReturnFalse()
                {
                    //Arrange
                    string cpf = "000.138.820-48";

                    //Act
                    bool result = CPFValidator.IsCpf(cpf);

                    //Assert
                    Assert.False(result);
                }
            }
        }
    }
}

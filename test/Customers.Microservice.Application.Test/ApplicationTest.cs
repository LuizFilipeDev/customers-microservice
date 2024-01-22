using System.Net;
using System.Security.Claims;
using Amazon.SSO.Model;
using Customers.Microservice.Domain.Aggregates.Customer;
using Customers.Microservice.Domain.Aggregates.User;
using Customers.Microservice.Domain.SeedWork;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Customers.Microservice.Application.Test
{
    public class ApplicationTest
    {
        private static readonly Mock<HttpContext> _mockContext = new();

        public class Login{

            [Theory]
            [InlineData("fake.admin.authorized")]
            public async Task GivenAuthorizedUserWhenTryToLoginThenReturnSuccess(string fakeAdminAuthorized)
            {   
                //Arrange
                User user = new() { Name = fakeAdminAuthorized, Password = fakeAdminAuthorized};

                var mockUserService = new Mock<IUserService>();
                mockUserService
                    .Setup(x => x.IsValidUser(user))
                    .Returns((IUser user) => Task.FromResult(user.Name == fakeAdminAuthorized && user.Password == fakeAdminAuthorized));

                var mockServiceProvider = new Mock<IServiceProvider>(); 
                mockServiceProvider    
                    .Setup(x => x.GetService(typeof(IUserService)))
                    .Returns(mockUserService.Object);

                //Act
                SignInHttpResult result = (SignInHttpResult) await Extensions.EndpointsExtensions.Login(user, mockServiceProvider.Object);
                
                //Assert
                Assert.NotNull(result);
                Assert.True(result.Principal?.Identities.Any(x => x.Name == fakeAdminAuthorized));
            }

            [Theory]
            [InlineData("fake.admin.authorized", "fake.admin.unauthorized")]
            public async Task GivenUnauthorizedUserWhenTryToLoginThenReturnUnauthorized(string fakeAdminAuthorized, string fakeAdminUnauthorized)
            {   

                //Arrange
                User mockUser = new() { Name = fakeAdminUnauthorized, Password = fakeAdminUnauthorized};

                var mockUserService = new Mock<IUserService>();
                mockUserService
                    .Setup(x => x.IsValidUser(mockUser))
                    .Returns((IUser user) => Task.FromResult(user.Name == fakeAdminAuthorized && user.Password == fakeAdminAuthorized));

                var mockServiceProvider = new Mock<IServiceProvider>(); 
                mockServiceProvider 
                    .Setup(x => x.GetService(typeof(IUserService)))
                    .Returns(mockUserService.Object);

                //Act
                UnauthorizedHttpResult result = (UnauthorizedHttpResult) await Extensions.EndpointsExtensions.Login(mockUser, mockServiceProvider.Object);
                
                //Assert
                Assert.NotNull(result);
                Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
            }

            [Fact]
            public void GivenAnUnauthorizedUserWhenTryToRequestThenReturnUnauthorized()
            {   
                //Arrange
                _mockContext.Setup(x => x.User).Returns(new ClaimsPrincipal(  
                new ClaimsIdentity(
                    new[] { new Claim(ClaimTypes.Name, string.Empty) },
                    BearerTokenDefaults.AuthenticationScheme
                ) ));

                //Act
                string name = _mockContext.Object.User.Claims
                    .Where(x => x.Type == ClaimTypes.Name)
                    .SingleOrDefault()?.Value ?? string.Empty;

                //Assert
                Assert.Equal(string.Empty, name);
            }
        }

        public class MapGetCustomerById{
            
            [Theory]
            [InlineData(5)]
            public void GivenAnAuthorizedUserAndValidCustomerIdWhenTryToGetAnCustomerThenReturnSuccessWithCustomerObject(int id)
            {   
                //Arrange
                Mock<Customer> mockCustomer = new Mock<Customer>();
                mockCustomer.Object.Id = id;
                mockCustomer.Object.Name = string.Empty;

                var mockCustomerService = new Mock<ICustomerService>();
                mockCustomerService
                    .Setup(x => x.SelectById(id))
                    .Returns((int id) => mockCustomer.Object);
                    
                var mockServiceProvider = new Mock<IServiceProvider>(); 
                mockServiceProvider 
                    .Setup(x => x.GetService(typeof(ICustomerService)))
                    .Returns(mockCustomerService.Object);

                //Act
                var result =  (Ok<ICustomer>) Extensions.EndpointsExtensions.MapGetCustomerById(id, mockServiceProvider.Object);

                //Assert
                Assert.NotNull(result);
                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
                Assert.Equal(id, result?.Value?.Id);
            }

            [Theory]
            [InlineData(10)]
            public void GivenAnAuthorizedUserAndInvalidCustomerIdWhenTryToGetAnCustomerThenReturnNotFound(int id)
            {   
                //Arrange
                var mockCustomerService = new Mock<ICustomerService>();
                mockCustomerService
                    .Setup(x => x.SelectById(id))
                    .Returns((int id) => new Customer{ Id = default, Name = string.Empty});

                var mockServiceProvider = new Mock<IServiceProvider>(); 
                mockServiceProvider 
                    .Setup(x => x.GetService(typeof(ICustomerService)))
                    .Returns(mockCustomerService.Object);

                //Act
                var result =  (NotFound) Extensions.EndpointsExtensions.MapGetCustomerById(id, mockServiceProvider.Object);

                //Assert
                Assert.NotNull(result);
                Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            }
        }
    
        public class MapGetAllCustomers{

            [Fact]
            public void GivenAnAuthorizedUserWhenTryToGetListOfCustomerThenReturnSuccessWithListOfCustomers()
            {   
                //Arrange

                Mock<List<ICustomer>> customersList = new();

                customersList.Object.Add(new Customer{ Id = 0, Name = string.Empty});
                customersList.Object.Add(new Customer{ Id = 0, Name = string.Empty});

                var mockCustomerService = new Mock<ICustomerService>();
                mockCustomerService
                    .Setup(x => x.Select())
                    .Returns(customersList.Object);

                var mockServiceProvider = new Mock<IServiceProvider>(); 
                mockServiceProvider 
                    .Setup(x => x.GetService(typeof(ICustomerService)))
                    .Returns(mockCustomerService.Object);

                //Act
                var result =  (Ok<List<ICustomer>>) Extensions.EndpointsExtensions.MapGetAllCustomers(mockServiceProvider.Object);

                Console.WriteLine(JsonConvert.SerializeObject(result));
                
                //Assert
                Assert.NotNull(result);
                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
                Assert.Equal(2, result?.Value?.Count);
            }
        }

        public class MapPostCustomer{

            [Fact]
            public void GivenAnAuthorizedUserAndValidCustomerWhenTryToPostAnCustomerThenReturnSuccessWithCustomerId()
            {   
                //Arrange
                int expectedIdentifier = 5;
                Mock<Customer> mockCustomer = new Mock<Customer>();
                mockCustomer.Object.Name = "test";

                var mockCustomerService = new Mock<ICustomerService>();
                mockCustomerService
                    .Setup(x => x.Insert(mockCustomer.Object))
                    .Returns((ICustomer customer) => expectedIdentifier);
                    
                var mockServiceProvider = new Mock<IServiceProvider>(); 
                mockServiceProvider 
                    .Setup(x => x.GetService(typeof(ICustomerService)))
                    .Returns(mockCustomerService.Object);

                //Act
                var result =  (Created<string>) Extensions.EndpointsExtensions.MapPostCustomer(mockCustomer.Object, mockServiceProvider.Object);

                //Assert
                Assert.NotNull(result);
                Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
                Assert.Equal(expectedIdentifier.ToString(), result?.Value);
            }

        }
    
        public class MapPutCustomerById{

            [Theory]
            [InlineData(5)]
            public void GivenAnAuthorizedUserAndValidCustomerWhenTryToPutAnCustomerByIdThenReturnSuccess(int id)
            {   
                //Arrange
                Mock<Customer> mockCustomer = new Mock<Customer>();
                mockCustomer.Object.Name = "test";

                var mockCustomerService = new Mock<ICustomerService>();
                mockCustomerService
                    .Setup(x => x.Update(id, mockCustomer.Object))
                    .Returns((int id, ICustomer customer) => true);
                    
                var mockServiceProvider = new Mock<IServiceProvider>(); 
                mockServiceProvider 
                    .Setup(x => x.GetService(typeof(ICustomerService)))
                    .Returns(mockCustomerService.Object);

                //Act
                var result =  (Ok) Extensions.EndpointsExtensions.MapPutCustomerById(id, mockCustomer.Object, mockServiceProvider.Object);

                //Assert
                Assert.NotNull(result);
                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            }

            [Theory]
            [InlineData(10)]
            public void GivenAnAuthorizedUserAndValidCustomerWhenTryToPutAnCustomerByIdThenReturnNotFound(int id)
            {   
                //Arrange
                Mock<Customer> mockCustomer = new Mock<Customer>();
                mockCustomer.Object.Name = "test";

                var mockCustomerService = new Mock<ICustomerService>();
                mockCustomerService
                    .Setup(x => x.Update(id, mockCustomer.Object))
                    .Returns((int id, ICustomer customer) => false);
                    
                var mockServiceProvider = new Mock<IServiceProvider>(); 
                mockServiceProvider 
                    .Setup(x => x.GetService(typeof(ICustomerService)))
                    .Returns(mockCustomerService.Object);

                //Act
                var result =  (NotFound) Extensions.EndpointsExtensions.MapPutCustomerById(id, mockCustomer.Object, mockServiceProvider.Object);

                //Assert
                Assert.NotNull(result);
                Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            }

        }

        public class MapPatchCustomerById{

            [Theory]
            [InlineData(5)]
            public void GivenAnAuthorizedUserAndValidCustomerWhenTryToPatchAnCustomerByIdThenReturnSuccess(int id)
            {   
                //Arrange
                Mock<Customer> mockCustomer = new Mock<Customer>();
                mockCustomer.Object.Name = "test";

                var mockCustomerService = new Mock<ICustomerService>();
                mockCustomerService
                    .Setup(x => x.Update(id, mockCustomer.Object))
                    .Returns((int id, ICustomer customer) => true);
                    
                var mockServiceProvider = new Mock<IServiceProvider>(); 
                mockServiceProvider 
                    .Setup(x => x.GetService(typeof(ICustomerService)))
                    .Returns(mockCustomerService.Object);

                //Act
                var result =  (Ok) Extensions.EndpointsExtensions.MapPatchCustomerById(id, mockCustomer.Object, mockServiceProvider.Object);

                //Assert
                Assert.NotNull(result);
                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            }

            [Theory]
            [InlineData(10)]
            public void GivenAnAuthorizedUserAndValidCustomerWhenTryToPatchAnCustomerByIdThenReturnNotFound(int id)
            {   
                //Arrange
                Mock<Customer> mockCustomer = new Mock<Customer>();
                mockCustomer.Object.Name = "test";

                var mockCustomerService = new Mock<ICustomerService>();
                mockCustomerService
                    .Setup(x => x.Update(id, mockCustomer.Object))
                    .Returns((int id, ICustomer customer) => false);
                    
                var mockServiceProvider = new Mock<IServiceProvider>(); 
                mockServiceProvider 
                    .Setup(x => x.GetService(typeof(ICustomerService)))
                    .Returns(mockCustomerService.Object);

                //Act
                var result =  (NotFound) Extensions.EndpointsExtensions.MapPatchCustomerById(id, mockCustomer.Object, mockServiceProvider.Object);

                //Assert
                Assert.NotNull(result);
                Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            }
        }

        public class MapDeleteCustomerById{

            [Theory]
            [InlineData(5)]
            public void GivenAnAuthorizedUserAndValidCustomerWhenTryToDeleteAnCustomerByIdThenReturnSuccess(int id)
            {   
                //Arrange
                var mockCustomerService = new Mock<ICustomerService>();
                mockCustomerService
                    .Setup(x => x.Delete(id))
                    .Returns((int id) => true);
                    
                var mockServiceProvider = new Mock<IServiceProvider>(); 
                mockServiceProvider 
                    .Setup(x => x.GetService(typeof(ICustomerService)))
                    .Returns(mockCustomerService.Object);

                //Act
                var result =  (Ok) Extensions.EndpointsExtensions.MapDeleteCustomerById(id, mockServiceProvider.Object);

                //Assert
                Assert.NotNull(result);
                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            }

            [Theory]
            [InlineData(10)]
            public void GivenAnAuthorizedUserAndValidCustomerWhenTryToDeleteAnCustomerByIdThenReturnNotFound(int id)
            {   
                //Arrange
                var mockCustomerService = new Mock<ICustomerService>();
                mockCustomerService
                    .Setup(x => x.Delete(id))
                    .Returns((int id) => false);
                    
                var mockServiceProvider = new Mock<IServiceProvider>(); 
                mockServiceProvider 
                    .Setup(x => x.GetService(typeof(ICustomerService)))
                    .Returns(mockCustomerService.Object);

                //Act
                var result =  (NotFound) Extensions.EndpointsExtensions.MapDeleteCustomerById(id, mockServiceProvider.Object);

                //Assert
                Assert.NotNull(result);
                Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            }
        }
    }
}

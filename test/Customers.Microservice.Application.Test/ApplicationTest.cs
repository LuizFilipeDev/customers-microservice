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
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Customers.Microservice.Application.Test
{
    public class ApplicationTest
    {
        private static readonly Mock<IServiceProvider> _mockServiceProvider = new();
        private static readonly Mock<HttpContext> _mockContext = new();

        public class Login{
            
            private readonly Mock<IServiceProvider> _mockServiceProvider = new();

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

                _mockServiceProvider
                    .Setup(x => x.GetService(typeof(IUserService)))
                    .Returns(mockUserService.Object);

                //Act
                SignInHttpResult result = (SignInHttpResult) await Extensions.EndpointsExtensions.Login(user, _mockServiceProvider.Object);
                
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

                _mockServiceProvider
                    .Setup(x => x.GetService(typeof(IUserService)))
                    .Returns(mockUserService.Object);

                //Act
                UnauthorizedHttpResult result = (UnauthorizedHttpResult) await Extensions.EndpointsExtensions.Login(mockUser, _mockServiceProvider.Object);
                
                //Assert
                Assert.NotNull(result);
                Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
            }
        }

        public class MapGetCustomerById{
            
            [Theory]
            [InlineData(5)]
            public void GivenAnAuthorizedUserAndValidCustomerIdWhenTryToGetAnCustomerThenReturnSuccessWithCustomerObject(int id)
            {   
                //Arrange
                var mockCustomerService = new Mock<ICustomerService>();
                mockCustomerService
                    .Setup(x => x.SelectById(id))
                    .Returns((int id) => new Customer{ Id = id, Name = "test"});

                _mockServiceProvider
                    .Setup(x => x.GetService(typeof(ICustomerService)))
                    .Returns(mockCustomerService.Object);

                //Act
                var result =  (Ok<ICustomer>) Extensions.EndpointsExtensions.MapGetCustomerById(id, _mockServiceProvider.Object);

                //Assert
                Assert.NotNull(result);
                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
                Assert.Equal(result?.Value?.Id, id);
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

                _mockServiceProvider
                    .Setup(x => x.GetService(typeof(ICustomerService)))
                    .Returns(mockCustomerService.Object);

                //Act
                var result =  (NotFound) Extensions.EndpointsExtensions.MapGetCustomerById(id, _mockServiceProvider.Object);

                //Assert
                Assert.NotNull(result);
                Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            }

            [Fact]
            public void GivenAnUnauthorizedUserWhenTryToGetAnCustomerByIdThenReturnUnauthorized()
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
    
        public class MapGetAllCustomers{
            
        }
    }
}

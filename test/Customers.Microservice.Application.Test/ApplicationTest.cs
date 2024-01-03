using System.Net;
using Customers.Microservice.Domain.Aggregates.Customer;
using Customers.Microservice.Domain.Aggregates.User;
using Customers.Microservice.Domain.SeedWork;
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
                User mockUser = new() { Name = fakeAdminUnauthorized, Password = fakeAdminUnauthorized};

                //Arrange
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
        }

        public class MapGetCustomerById{
            
            [Theory]
            [InlineData(5)]
            public void GivenAnUserIdWhenTryToGetAnCustomerThenReturnSuccessWithCustomerObject(int id)
            {   
                //Arrange
                var mockCustomerService = new Mock<ICustomerService>();
                mockCustomerService
                    .Setup(x => x.SelectById(id))
                    .Returns((int id) => new Customer{ Id = id, Name = "test"});

                var mockServiceProvider = new Mock<IServiceProvider>();
                mockServiceProvider
                    .Setup(x => x.GetService(typeof(ICustomerService)))
                    .Returns(mockCustomerService.Object);

                //Act
                var result =  (Ok<ICustomer>) Extensions.EndpointsExtensions.MapGetCustomerById(5, mockServiceProvider.Object);

                //Assert
                Assert.NotNull(result);
                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
                Assert.Equal(result?.Value?.Id, id);
            }
        }
    }
}

using Customers.Microservice.Domain.SeedWork;
using Customers.Microservice.Infrastructure.External;
using Moq;
using Xunit;

namespace Customers.Microservice.Infrastructure.Test
{
    public class InfrastructureTest
    {
        public class ExternalTest
        {
            public class AWSTest
            {
                [Theory]
                [InlineData("user.admin.test", "password.admin.test")]
                public async void GivenANeedOfSelectAllSecretsFromSecretManagerWhenTryToSelectAllSecretsThenReturnSecrets(string key, string value)
                {
                    //Arrange
                    var dictionary = new Dictionary<string, string>(1)
                    {
                        { key, value }
                    };
                    var mockAWS = new Mock<IAWS>();
                    mockAWS
                        .Setup(x => x.GetSecretsFromSecretManager("customer.api.test", "sa-east-1-test"))
                        .Returns((string secretName, string region)
                        => Task.FromResult(dictionary));

                    //Act
                    Dictionary<string, string> result = await mockAWS.Object.
                        GetSecretsFromSecretManager("customer.api.test", "sa-east-1-test");

                    //Assert
                    Assert.NotNull(result);
                    Assert.True(result.Count > default(int));
                    Assert.True(result.ContainsKey(key));
                }
            }
        }
    }
}

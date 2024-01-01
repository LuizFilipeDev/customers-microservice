using Customers.Microservice.Domain;
using Customers.Microservice.Domain.Aggregates.Customer;
using Customers.Microservice.Domain.Aggregates.User;
using Customers.Microservice.Domain.SeedWork;
using Customers.Microservice.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using System.Threading.RateLimiting;

namespace Customers.Microservice.Application.Extensions
{
    public static class ServicesExtensions
    {
        private static void AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ICustomerService, CustomerService>();
            serviceCollection.AddScoped<ICustomerRepository, CustomerRepository>();
            serviceCollection.AddScoped<IUserService, UserService>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddSingleton<CustomerInMemory>();
        }

        private static void AddRequestLimiter(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddRateLimiter(o =>
            {
                o.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                o.AddPolicy(Constant.RequestLimiter.policy, context => RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection?.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = Constant.RequestLimiter.permitLimit,
                        Window = Constant.RequestLimiter.window
                    }
                ));
            });
        }

        private static void AddBearerToken(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAuthentication().AddBearerToken(b =>
            {
                b.BearerTokenExpiration = Constant.BearerToken.expiration;
            });
        }

        private static void AddCustomSwagger(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Customers.Microservice", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "Token",
                    In = ParameterLocation.Header,
                    Description = "Token Access"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                c.EnableAnnotations();
            });
        }

        public static void AddCustomServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddRequestLimiter();
            serviceCollection.AddBearerToken();
            serviceCollection.AddServices();
            serviceCollection.AddCustomSwagger();
        }
    }
}

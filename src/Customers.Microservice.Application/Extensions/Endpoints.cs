using Customers.Microservice.Domain.Aggregates.Customer;
using Customers.Microservice.Domain.Aggregates.User;
using Customers.Microservice.Domain.SeedWork;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Customers.Microservice.Application.Extensions
{
    public static class EndpointsExtensions
    {
        public static void AddEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapPost("/login",
                [SwaggerOperation(
                    Summary = "Generate token",
                    Description = "Generate your token to use on all endpoints",
                    Tags = new[] { "Authentication" }
                )]
            [SwaggerResponse(200, "Success!")]
            [SwaggerResponse(401, "Unauthorized!")]
            [SwaggerResponse(429, "Too many requests!")]
            async ([FromForm] User user, [FromServices] IServiceProvider serviceProvider) =>
            {
                if(string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Password))
                    return Results.Unauthorized();
                
                IUserService? userService = serviceProvider?.GetService<IUserService>();

                if (await userService?.IsValidUser(user))
                {
                    var claimsPrincipal = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new[] { new Claim(ClaimTypes.Name, user.Name) },
                            BearerTokenDefaults.AuthenticationScheme
                        )
                    );

                    return Results.SignIn(claimsPrincipal);
                }
                else
                {
                    return Results.Unauthorized();
                }
            })
            .DisableAntiforgery()
            .RequireRateLimiting(Constant.RequestLimiter.policy)
            .WithOpenApi();

            endpointRouteBuilder.MapGet("/customer/{id}",
                [SwaggerOperation(
                    Summary = "Get a customer by identifier",
                    Description = "Get a customer by identifier",
                    Tags = new[] { "Customer" }
                )]
            [SwaggerResponse(200, "Success!")]
            [SwaggerResponse(401, "Unauthorized!", typeof(object))]
            [SwaggerResponse(404, "The customer was not found, the id is incorrect!", typeof(object))]
            [SwaggerResponse(429, "Too many requests!", typeof(object))]
            ([FromRoute] int id, [FromServices] IServiceProvider serviceProvider) =>
            {
                ICustomerService? customerService = serviceProvider.GetService<ICustomerService>();
                ICustomer customer = customerService?.SelectById(id) ?? new Customer();
                return customer.Id != default ? Results.Ok(customer) : Results.NotFound();

            }).RequireAuthorization()
            .RequireRateLimiting(Constant.RequestLimiter.policy)
            .WithOpenApi();

            endpointRouteBuilder.MapGet("/customer",
                [SwaggerOperation(
                    Summary = "Get a list of customers",
                    Description = "Get a list of customers",
                    Tags = new[] { "Customer" }
                )]
            [SwaggerResponse(200, "Success!")]
            [SwaggerResponse(401, "Unauthorized!", typeof(object))]
            [SwaggerResponse(429, "Too many requests!", typeof(object))]
            ([FromServices] IServiceProvider serviceProvider) =>
            {
                ICustomerService? customerService = serviceProvider.GetService<ICustomerService>();
                return customerService?.Select();

            }).RequireAuthorization()
            .RequireRateLimiting(Constant.RequestLimiter.policy)
            .WithOpenApi();

            endpointRouteBuilder.MapPost("/customer",
                [SwaggerOperation(
                    Summary = "Creates a new customer",
                    Description = "Creates a new customer",
                    Tags = new[] { "Customer" }
                )]
            [SwaggerResponse(201, "Created!", typeof(int))]
            [SwaggerResponse(401, "Unauthorized!", typeof(object))]
            [SwaggerResponse(429, "Too many requests!", typeof(object))]
            ([FromBody] Customer customer, [FromServices] IServiceProvider serviceProvider) =>
            {
                ICustomerService? customerService = serviceProvider.GetService<ICustomerService>();
                return customerService?.Insert(customer);

            }).RequireAuthorization()
            .RequireRateLimiting(Constant.RequestLimiter.policy)
            .WithOpenApi();

            endpointRouteBuilder.MapPut("/customer/{id}",
                [SwaggerOperation(
                    Summary = "Update customer properties",
                    Description = "Update customer properties",
                    Tags = new[] { "Customer" }
                )]
            [SwaggerResponse(200, "Success!")]
            [SwaggerResponse(401, "Unauthorized!", typeof(object))]
            [SwaggerResponse(404, "The customer was not found, the id is incorrect!", typeof(object))]
            [SwaggerResponse(429, "Too many requests!", typeof(object))]
            ([FromRoute] int id, [FromBody] Customer customer, [FromServices] IServiceProvider serviceProvider) =>
            {
                ICustomerService? customerService = serviceProvider.GetService<ICustomerService>();
                return customerService?.Update(id, customer) ?? false ? Results.Ok() : Results.NotFound();

            }).RequireAuthorization()
            .RequireRateLimiting(Constant.RequestLimiter.policy)
            .WithOpenApi();

            endpointRouteBuilder.MapPatch("/customer/{id}",
                [SwaggerOperation(
                    Summary = "Update part of customer properties",
                    Description = "Update part of customer properties",
                    Tags = new[] { "Customer" }
                )]
            [SwaggerResponse(200, "Success!")]
            [SwaggerResponse(401, "Unauthorized!", typeof(object))]
            [SwaggerResponse(404, "The customer was not found, the id is incorrect!", typeof(object))]
            [SwaggerResponse(429, "Too many requests!", typeof(object))]
            ([FromRoute] int id, [FromBody] Customer customer, [FromServices] IServiceProvider serviceProvider) =>
            {
                ICustomerService? customerService = serviceProvider.GetService<ICustomerService>();
                return customerService?.Update(id, customer) ?? false ? Results.Ok() : Results.NotFound();

            }).RequireAuthorization()
            .RequireRateLimiting(Constant.RequestLimiter.policy)
            .WithOpenApi();

            endpointRouteBuilder.MapDelete("/customer/{id}",
                [SwaggerOperation(
                    Summary = "Delete an customer by identifier",
                    Description = "Delete an customer by identifier",
                    Tags = new[] { "Customer" }
                )]
            [SwaggerResponse(200, "Success!")]
            [SwaggerResponse(401, "Unauthorized!", typeof(object))]
            [SwaggerResponse(404, "The customer was not found, the id is incorrect!", typeof(object))]
            [SwaggerResponse(429, "Too many requests!", typeof(object))]
             ([FromRoute] int id, [FromServices] IServiceProvider serviceProvider) =>
            {
                ICustomerService? customerService = serviceProvider.GetService<ICustomerService>();
                return customerService?.Delete(id) ?? false ? Results.Ok() : Results.NotFound();

            }).RequireAuthorization()
            .RequireRateLimiting(Constant.RequestLimiter.policy)
            .WithOpenApi();
        }
    }
}
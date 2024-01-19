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
            endpointRouteBuilder.MapPost("/login", Login)
                .DisableAntiforgery()
                .RequireRateLimiting(Constant.RequestLimiter.policy)
                .WithOpenApi();

            endpointRouteBuilder.MapGet("/customer/{id}", MapGetCustomerById)
                .RequireAuthorization()
                .RequireRateLimiting(Constant.RequestLimiter.policy)
                .WithOpenApi();

            endpointRouteBuilder.MapGet("/customer", MapGetAllCustomers)
                .RequireAuthorization()
                .RequireRateLimiting(Constant.RequestLimiter.policy)
                .WithOpenApi();

            endpointRouteBuilder.MapPost("/customer", MapPostCustomer)
                .RequireAuthorization()
                .RequireRateLimiting(Constant.RequestLimiter.policy)
                .WithOpenApi();

            endpointRouteBuilder.MapPut("/customer/{id}", MapPutCustomerById)
                .RequireAuthorization()
                .RequireRateLimiting(Constant.RequestLimiter.policy)
                .WithOpenApi();

            endpointRouteBuilder.MapPatch("/customer/{id}", MapPatchCustomerById)
                .RequireAuthorization()
                .RequireRateLimiting(Constant.RequestLimiter.policy)
                .WithOpenApi();

            endpointRouteBuilder.MapDelete("/customer/{id}", MapDeleteCustomerById).RequireAuthorization()
            .RequireRateLimiting(Constant.RequestLimiter.policy)
            .WithOpenApi();
        }

        [SwaggerOperation(
            Summary = "Generate token",
            Description = "Generate your token to use on all endpoints",
            Tags = new[] { "Authentication" }
        )]
        [SwaggerResponse(200, "Success!")]
        [SwaggerResponse(401, "Unauthorized!")]
        [SwaggerResponse(429, "Too many requests!")]
        public static async Task<IResult> Login([FromForm] User user, [FromServices] IServiceProvider serviceProvider){
            
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
        }

        [SwaggerOperation(
            Summary = "Get a customer by identifier",
            Description = "Get a customer by identifier",
            Tags = new[] { "Customer" }
        )]
        [SwaggerResponse(200, "Success!")]
        [SwaggerResponse(401, "Unauthorized!", typeof(object))]
        [SwaggerResponse(404, "The customer was not found, the id is incorrect!", typeof(object))]
        [SwaggerResponse(429, "Too many requests!", typeof(object))]
        public static IResult MapGetCustomerById([FromRoute] int id, [FromServices] IServiceProvider serviceProvider){

            ICustomerService? customerService = serviceProvider.GetService<ICustomerService>();
            ICustomer customer = customerService?.SelectById(id) ?? new Customer();
            return customer.Id != default ? Results.Ok(customer) : Results.NotFound();
        }

        [SwaggerOperation(
            Summary = "Get a list of customers",
            Description = "Get a list of customers",
            Tags = new[] { "Customer" }
        )]
        [SwaggerResponse(200, "Success!")]
        [SwaggerResponse(401, "Unauthorized!", typeof(object))]
        [SwaggerResponse(429, "Too many requests!", typeof(object))]
        public static IResult MapGetAllCustomers([FromServices] IServiceProvider serviceProvider){

            ICustomerService? customerService = serviceProvider.GetService<ICustomerService>();
            return Results.Ok<List<ICustomer>>(customerService?.Select());
        }

        [SwaggerOperation(
            Summary = "Creates a new customer",
            Description = "Creates a new customer",
            Tags = new[] { "Customer" }
        )]
        [SwaggerResponse(201, "Created!", typeof(int))]
        [SwaggerResponse(401, "Unauthorized!", typeof(object))]
        [SwaggerResponse(429, "Too many requests!", typeof(object))]
        public static IResult MapPostCustomer([FromBody] Customer customer, [FromServices] IServiceProvider serviceProvider){

            ICustomerService? customerService = serviceProvider.GetService<ICustomerService>();
            return Results.Created("/customer", customerService?.Insert(customer).ToString());
        }

        [SwaggerOperation(
            Summary = "Update customer properties",
            Description = "Update customer properties",
            Tags = new[] { "Customer" }
        )]
        [SwaggerResponse(200, "Success!")]
        [SwaggerResponse(401, "Unauthorized!", typeof(object))]
        [SwaggerResponse(404, "The customer was not found, the id is incorrect!", typeof(object))]
        [SwaggerResponse(429, "Too many requests!", typeof(object))]
        public static IResult MapPutCustomerById([FromRoute] int id, [FromBody] Customer customer, [FromServices] IServiceProvider serviceProvider){

                ICustomerService? customerService = serviceProvider.GetService<ICustomerService>();
                return customerService?.Update(id, customer) ?? false ? Results.Ok() : Results.NotFound();
        }

        [SwaggerOperation(
            Summary = "Update part of customer properties",
            Description = "Update part of customer properties",
            Tags = new[] { "Customer" }
        )]
        [SwaggerResponse(200, "Success!")]
        [SwaggerResponse(401, "Unauthorized!", typeof(object))]
        [SwaggerResponse(404, "The customer was not found, the id is incorrect!", typeof(object))]
        [SwaggerResponse(429, "Too many requests!", typeof(object))]
        public static IResult MapPatchCustomerById([FromRoute] int id, [FromBody] Customer customer, [FromServices] IServiceProvider serviceProvider){
            ICustomerService? customerService = serviceProvider.GetService<ICustomerService>();
            return customerService?.Update(id, customer) ?? false ? Results.Ok() : Results.NotFound();
        }

        [SwaggerOperation(
            Summary = "Delete an customer by identifier",
            Description = "Delete an customer by identifier",
            Tags = new[] { "Customer" }
        )]
        [SwaggerResponse(200, "Success!")]
        [SwaggerResponse(401, "Unauthorized!", typeof(object))]
        [SwaggerResponse(404, "The customer was not found, the id is incorrect!", typeof(object))]
        [SwaggerResponse(429, "Too many requests!", typeof(object))]
        public static IResult MapDeleteCustomerById([FromRoute] int id, [FromServices] IServiceProvider serviceProvider){

            ICustomerService? customerService = serviceProvider.GetService<ICustomerService>();
            return customerService?.Delete(id) ?? false ? Results.Ok() : Results.NotFound();
        }
    }
}
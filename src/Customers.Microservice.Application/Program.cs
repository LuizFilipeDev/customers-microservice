using Customers.Microservice.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();
builder.Services.AddCustomServices();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => { 
    c.DefaultModelsExpandDepth(-1);
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseRateLimiter();

app.AddEndpoints();

app.Run();
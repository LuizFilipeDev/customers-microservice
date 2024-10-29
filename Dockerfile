FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY src/Customers.Microservice.Application/Customers.Microservice.Application.csproj src/Customers.Microservice.Application/
COPY src/Customers.Microservice.Domain/Customers.Microservice.Domain.csproj src/Customers.Microservice.Domain/
COPY src/Customers.Microservice.Infrastructure/Customers.Microservice.Infrastructure.csproj src/Customers.Microservice.Infrastructure/
RUN dotnet restore src/Customers.Microservice.Application/Customers.Microservice.Application.csproj

COPY . .
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /out ./

EXPOSE 8080
ENTRYPOINT ["dotnet", "Customers.Microservice.Application.dll"]

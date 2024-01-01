namespace Customers.Microservice.Domain.Aggregates.User
{
    public interface IUserService
    {
        Task<bool> IsValidUser(IUser user);
    }
}

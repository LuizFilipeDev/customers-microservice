using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Microservice.Domain.Aggregates.User
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> IsValidUser(IUser user) {

            //Domain rules/logic

            //Commented the line below to don't use the AWS Secret Manager for now. But it works!
            //return await _userRepository.SelectByNameAndPasswordAsync(user.Name ?? string.Empty, user.Password ?? string.Empty);
            return await Task.FromResult(true);
        }
    }
}

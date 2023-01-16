using Sat.Recruitment.Api.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<User>> GetUsers();
        Task<bool> CreateUser(User newUser);
    }
}

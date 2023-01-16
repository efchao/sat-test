using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Domain;
using Sat.Recruitment.Api.Interfaces;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UsersControllerTest
    {
        [Fact]
        public async Task Test_User_Created_Ok()
        {
            var user = new User {
                Name = "Mike",
                Email = "mike@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                Money = 124,
                UserType = User.Type.Normal
            };
            var mockUsersRepository = new MockUsersRepository();
            var usersController = new UsersController(mockUsersRepository);
            var result = await usersController.CreateUser(user);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Errors);
            Assert.Single(result.Errors);
            Assert.Equal(User.GetErrorString(User.Error.Created), result.Errors.First());
        }

        [Fact]
        public async Task Test_User_Duplicated_Error()
        {
            var user = new User
            {
                Name = "Agustina",
                Email = "Agustina@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                Money = 124,
                UserType = User.Type.Normal
            };
            var mockUsersRepository = new MockUsersRepository();
            var usersController = new UsersController(mockUsersRepository);
            var result = await usersController.CreateUser(user);

            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Errors);
            Assert.Single(result.Errors);
            Assert.Equal(User.GetErrorString(User.Error.Duplicated), result.Errors.First());
        }

        //Mock Classes
        private class MockUsersRepository : IUsersRepository
        {
            private List<User> _users = new List<User>();
            public Task<List<User>> GetUsers()
            {
                if(_users.Count == 0)
                {    
                    _users = new List<User>
                    {
                        new User
                        {
                            Name = "Juan", Email = "Juan@marmol.com", Phone = "+5491154762312", Address = "Peru 2464",
                            UserType = User.Type.Normal, Money = 1234
                        },
                        new User
                        {
                            Name = "Franco", Email = "Franco.Perez@gmail.com", Phone = "+534645213542", Address = "Alvear y Colombres",
                            UserType = User.Type.Premium, Money = 112234
                        },
                        new User
                        {
                            Name = "Agustina", Email = "Agustina@gmail.com", Phone = "+534645213542", Address = "Garay y Otra Calle",
                            UserType = User.Type.SuperUser, Money = 112234
                        }
                    };
                }
                return Task.FromResult(_users);
            }
            public Task<bool> CreateUser(User newUser)
            {
                _users.Add(newUser);
                return Task.FromResult(true);
            }
        }
    }
}

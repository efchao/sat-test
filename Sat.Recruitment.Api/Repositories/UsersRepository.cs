using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.IO;

using Sat.Recruitment.Api.Domain;
using Sat.Recruitment.Api.Interfaces;

namespace Sat.Recruitment.Api.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly List<User> _users = new List<User>();
        public Task<bool> CreateUser(User newUser)
        {
            _users.Add(newUser);
            return Task.FromResult(true);
        }
        public async Task<List<User>> GetUsers()
        {
            await LoadUsers();
            return _users;
        }

        //private load users
        private async Task LoadUsers()
        {
            if(_users.Count == 0)
            {
                var reader = ReadUsersFromFile();
                while (reader.Peek() >= 0)
                {
                    var line = await reader.ReadLineAsync();
                    var fields = line.Split(',');
                    if (fields.Length < 6) continue; //defensive programming in case user row is corrupted, we skip it

                    var userType = CalculateUserType(fields[4]);
                    var money = CalculateMoney(fields[5]);
                    var user = new User
                    {
                        Name = fields[0],
                        Email = fields[1],
                        Phone = fields[2],
                        Address = fields[3],
                        UserType = userType,
                        Money = money
                    };
                    _users.Add(user);
                }
                reader.Close();
            }
        }
        private StreamReader ReadUsersFromFile()
        {
            var path = Directory.GetCurrentDirectory() + "/Files/Users.txt";
            var fileStream = new FileStream(path, FileMode.Open);
            var reader = new StreamReader(fileStream);
            return reader;
        }
        private User.Type CalculateUserType(string field, User.Type defaultUserType = User.Type.Normal)
        {
            if (!Enum.TryParse(field, out User.Type usertype))
            {
                usertype = defaultUserType;
            }
            return usertype;
        }
        private decimal CalculateMoney(string field, decimal defaultMoney = 0)
        {
            if (!decimal.TryParse(field, out decimal money))
            {
                money = defaultMoney;
            }
            return money;
        }
    }
}

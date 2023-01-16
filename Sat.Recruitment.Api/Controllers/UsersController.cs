using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Threading.Tasks;

using Sat.Recruitment.Api.Domain;
using Sat.Recruitment.Api.Interfaces;
using System.Collections.Generic;

using SatUser = Sat.Recruitment.Api.Domain.User; //avoid clashing with ControllerBase's ClaimsPrincipal User

namespace Sat.Recruitment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUsersRepository usersRepository,
            ILogger<UsersController> logger = null
        )
        {
            _usersRepository = usersRepository ?? throw new ArgumentException(nameof(usersRepository));
            _logger = logger;
        }

        [HttpPost]
        [Route("/create-user")]
        public async Task<Result> CreateUser(SatUser requestUser)
        {
            var errors = requestUser.ValidateErrors();
            if (errors.Any())
                return Result.Error(errors);

            var newUser = new SatUser(requestUser);
            newUser.ApplyPercentage();
            newUser.NormalizeEmail();

            var users = await _usersRepository.GetUsers();
            bool isDuplicated = users.Any(user => user.IsDuplicated(newUser));
            if (!isDuplicated)
                await _usersRepository.CreateUser(newUser); //do create the user as this controller method dictates

            string returnMessage =  isDuplicated ?
                SatUser.GetErrorString(SatUser.Error.Duplicated)
                :
                SatUser.GetErrorString(SatUser.Error.Created)
            ;

            _logger?.LogInformation(returnMessage);

            return new Result
            {
                IsSuccess = !isDuplicated,
                Errors = new List<string> { returnMessage }
            };
        }
    }
}

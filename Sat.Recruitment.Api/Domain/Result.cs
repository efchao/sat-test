using System.Collections.Generic;
using System.Linq;

namespace Sat.Recruitment.Api.Domain
{
    public class Result
    {
        public static Result Success(string error)
        {
            var result = new Result();
            result.IsSuccess = true;
            result.Errors.Add(error);
            return result;
        }
        public static Result Error(string error)
        {
            return Error(new[] { error });
        }
        public static Result Error(IEnumerable<string> errors)
        {
            var result = new Result();
            result.IsSuccess = false;
            result.Errors = errors.ToList();
            return result;
        }
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}

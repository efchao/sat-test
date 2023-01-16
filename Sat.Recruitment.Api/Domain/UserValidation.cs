using System.Collections.Generic;

namespace Sat.Recruitment.Api.Domain
{
    //User Validation
    public partial class User
    {
        public List<string> ValidateErrors()
        {
            var errors = new List<string>();
            ValidationResult validationResult;

            validationResult = ValidateRequired(nameof(Name), Name);
            if (!validationResult.IsSuccess)
                errors.Add(validationResult.Error);

            validationResult = ValidateRequired(nameof(Email), Email);
            if (!validationResult.IsSuccess)
                errors.Add(validationResult.Error);

            validationResult = ValidateRequired(nameof(Address), Address);
            if (!validationResult.IsSuccess)
                errors.Add(validationResult.Error);

            validationResult = ValidateRequired(nameof(Phone), Phone);
            if (!validationResult.IsSuccess)
                errors.Add(validationResult.Error);

            return errors;
        }
        private static ValidationResult ValidateRequired(string property, string value)
        {
            bool isValid = !string.IsNullOrWhiteSpace(value);
            return new ValidationResult
            {
                IsSuccess = isValid,
                Error = isValid? string.Empty : $"The property '{property}' is required"
            };
        }
        private class ValidationResult
        {
            public bool IsSuccess { get; set; }
            public string Error { get; set; }
        }
    }
}

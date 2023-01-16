using System;
using System.Collections.Generic;

namespace Sat.Recruitment.Api.Domain
{
    public partial class User
    {
        //Constructors
        public User() { }
        public User(User other)
        {
            Name = other.Name;
            Email = other.Email;
            Address = other.Address;
            Phone = other.Phone;
            UserType = other.UserType;
            Money = other.Money;
        }

        //Private
        public enum Error
        {
            Duplicated, Created
        }
        //error strings are all managed here, so they can be easily changed in one place or even loaded and internationalized.
        private static readonly Dictionary<Error, string> _errors = new Dictionary<Error, string>
        {
            [Error.Duplicated] = "The user is duplicated",
            [Error.Created] = "User Created"
        };
        public static string GetErrorString(Error error)
        {
            _errors.TryGetValue(error, out string result);
            return result; //null if key not found
        }

        //Properties
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public Type UserType { get; set; }
        public decimal Money { get; set; }

        public bool IsNormal => UserType == Type.Normal;
        public bool IsSuperUser => UserType == Type.SuperUser;
        public bool IsPremium => UserType == Type.Premium;

        //Subtypes
        public enum Type
        {
            Normal,
            SuperUser,
            Premium
        }

        //Methods
        public void NormalizeEmail()
        {
            var aux = Email.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            if (aux.Length <= 0) return; //defensive programming

            var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);
            aux[0] = aux[0].Replace(".", "");
            if(atIndex >= 0)
                aux[0] = aux[0].Remove(atIndex);

            if (aux.Length >= 2)
            {
                Email = string.Join("@", new string[] { aux[0], aux[1] });
            }
            else
            {
                Email = aux[0];
            }
        }
        public bool IsDuplicated(User other)
        {
            return
                Email == other.Email ||
                Phone == other.Phone ||
                (Name == other.Name && Address == other.Address);
        }
        public decimal ApplyPercentage()
        {
            var percentage = 0m;

            if(Money > 100)
            {
                switch(UserType)
                {
                    case Type.Normal:
                        percentage = 0.12m;
                        break;
                    case Type.SuperUser:
                        percentage = 0.20m;
                        break;
                    case Type.Premium:
                        percentage = 2;
                        break;
                }
            }
            else if(Money > 10 && UserType == Type.Normal)
            {
                percentage = 0.8m;
            }
            var gif = Money * percentage;
            Money += gif;
            //NOTE: another more succint way to say the same
            //Money *= (1 + percentage);

            return Money;
        }
    }
}

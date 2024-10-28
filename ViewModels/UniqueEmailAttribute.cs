
using Moghtareb.Models;
using Moghtareb.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Moghtareb.ViewModels
{
    internal class UniqueEmailAttribute : ValidationAttribute
    {
        private readonly IUserRepository _userRepository;
        public UniqueEmailAttribute(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string email = value.ToString();
            User userFromReq = validationContext.ObjectInstance as User;
            User user = _userRepository.Get(email);
            if(user == null)
            {
                return ValidationResult.Success;
            }
            else if(user.id == userFromReq.id)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Email already exist");
        }
    }
}
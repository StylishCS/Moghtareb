using System.ComponentModel.DataAnnotations;

namespace Moghtareb.ViewModels
{
    public class UserRegisterationViewModel
    {
        [MaxLength(25, ErrorMessage = "Name must be 25 characters max")]
        public string name { get; set; }
        [DataType(DataType.EmailAddress)]
        //[UniqueEmail]
        public string email { get; set; }
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and be at least 8 characters long.")]
        public string password { get; set; }
        [Compare("password")]
        [DataType(DataType.Password)]
        public string confirmPassword { get; set; }

    }
}

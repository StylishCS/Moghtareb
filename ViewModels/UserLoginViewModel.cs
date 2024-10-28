using System.ComponentModel.DataAnnotations;

namespace Moghtareb.ViewModels
{
    public class UserLoginViewModel
    {
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}

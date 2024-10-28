namespace Moghtareb.Services
{
    using BCrypt.Net;
    public class UserService : IUserServices
    {
        public string HashPassword(string password)
        {
            return BCrypt.HashPassword(password);
        }
        public bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            return BCrypt.Verify(plainPassword, hashedPassword);
        }
    }
}

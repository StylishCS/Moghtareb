namespace Moghtareb.Services
{
    public interface IUserServices
    {
        string HashPassword(string password);
        bool VerifyPassword(string plainPassword, string hashedPassword);
    }
}

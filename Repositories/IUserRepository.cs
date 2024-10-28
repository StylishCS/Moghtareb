using Moghtareb.Models;

namespace Moghtareb.Repositories
{
    public interface IUserRepository
    {
        void Create(User user);
        bool Exists(string email);
        User Get(string email);
        User GetById(int id);
        void Update(User user);
        List<Ad> GetAds(int id);
        void Delete(int id);
        void Save();
        Task SaveAsync();
        Task<User> GetByIdAsync(int id);
    }
}
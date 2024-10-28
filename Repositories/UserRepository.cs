using Moghtareb.Models;

namespace Moghtareb.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MoghtarebDbContext _context;
        public UserRepository(MoghtarebDbContext context)
        {
            _context = context;
        }
        public void Create(User user)
        {
            _context.Users.Add(user);
        }

        public void Delete(int id)
        {
            User user = _context.Users.FirstOrDefault(u => u.id == id);
            user.isDeleted = true;
            user.deletionDate = DateTime.UtcNow;
        }

        public bool Exists(string email)
        {
            return _context.Users.FirstOrDefault(u => u.email == email) != null ? true : false;
        }

        public User Get(string email)
        {
            return _context.Users.FirstOrDefault(u => u.email == email);
        }

        public List<Ad> GetAds(int id)
        {
            User user = _context.Users.FirstOrDefault(u => u.id == id);
            return user.Ads.ToList();
        }

        public User GetById(int id)
        {
            User user = _context.Users.FirstOrDefault(_u => _u.id == id);
            return user;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            User user = await _context.Users.FindAsync(id);
            return user;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }
    }
}

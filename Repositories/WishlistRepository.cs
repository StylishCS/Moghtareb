using Microsoft.EntityFrameworkCore;
using Moghtareb.Models;

namespace Moghtareb.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly MoghtarebDbContext _context;

        public WishlistRepository(MoghtarebDbContext context)
        {
            _context = context;
        }

        public void Add(AdUserWishlist ad)
        {
            _context.AdUserWishlists.Add(ad);
        }

        public List<AdUserWishlist> GetWishlist()
        {
            return _context.AdUserWishlists.Include(w => w.ad).Include(w => w.user).ToList();
        }

        public bool isInWishList(int userId, int adId)
        {
            User user = _context.Users.Include(u => u.Wishlist).FirstOrDefault(u => u.id == userId);
            return user.Wishlist.FirstOrDefault(w => w.adId == adId) != null;
        }

        public void Remove(int userId, int adId)
        {
            AdUserWishlist item = _context.AdUserWishlists.FirstOrDefault(u => u.adId == adId && u.userId == userId);
            _context.AdUserWishlists.Remove(item);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

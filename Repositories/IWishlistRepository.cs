using Moghtareb.Models;

namespace Moghtareb.Repositories
{
    public interface IWishlistRepository
    {
        List<AdUserWishlist> GetWishlist();
        void Add(AdUserWishlist ad);
        void Remove(int userId, int adId);
        void Save();
        bool isInWishList(int userId, int adId);
    }
}
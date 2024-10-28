using Moghtareb.Models;

namespace Moghtareb.Repositories
{
    public interface IAdRepository
    {
        List<Ad> GetAll();
        Ad GetById(int id);
        List<Ad> GetByLocation(string location);
        void Create(Ad ad);
        void Update(Ad ad);
        void Delete(int id);
        void MarkAsRented(int id);
        void Save();
        Task SaveAsync();
        List<Ad> RelatedAds(string location);
    }
}
using Microsoft.EntityFrameworkCore;
using Moghtareb.Models;

namespace Moghtareb.Repositories
{
    public class AdRepository : IAdRepository
    {
        private readonly MoghtarebDbContext _context;
        public AdRepository(MoghtarebDbContext context)
        {
            _context = context;
        }
        public void Create(Ad ad)
        {
            _context.Ads.Add(ad);
        }

        public void Delete(int id)
        {
            Ad ad = _context.Ads.FirstOrDefault(a => a.id == id);
            ad.isDeleted = true;
        }

        public List<Ad> GetAll()
        {
            return _context.Ads
            .Where(a => a.isDeleted == false)
            .Include(a => a.Images)
            .Include(a => a.user)
            .OrderByDescending(a => a.user.isFeatured)
            .ToList();

        }

        public Ad GetById(int id)
        {
            return _context.Ads
                .Where(a => a.isDeleted == false)
                .Include(a => a.Images)
                .Include(a => a.user)
                .FirstOrDefault(a => a.id == id);
        }

        public List<Ad> GetByLocation(string location)
        {
            return _context.Ads
                .Where(a => a.isDeleted == false)
                .Where(a => a.city == location)
                .Include(a => a.user)
                .OrderByDescending(a => a.user.isFeatured)
                .ToList();
        }

        public void MarkAsRented(int id)
        {
            Ad ad = _context.Ads.FirstOrDefault(a => a.id == id);
            ad.isRented = true;
            ad.updatedAt = DateTime.Now;
        }

        public List<Ad> RelatedAds(string location)
        {
            List<Ad> ads = _context.Ads
                .Where(a => a.isDeleted == false)
                .Where(a => a.city == location)
                .Include(a => a.user)
                .OrderByDescending(a => a.user.isFeatured)
                .ToList();
            return ads;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Ad ad)
        {
            _context.Ads.Update(ad);
        }
    }
}
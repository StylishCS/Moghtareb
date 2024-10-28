using Microsoft.EntityFrameworkCore;
using Moghtareb.Models;

namespace Moghtareb.Repositories
{
    public class AdViewsDetailsRepository : IAdViewsDetailsRepository
    {
        private readonly MoghtarebDbContext _context;
        public AdViewsDetailsRepository(MoghtarebDbContext context)
        {
            _context = context;
        }

        public List<AdUserPhoneView> GetPhoneViews()
        {
            return _context
                .AdUserPhoneViews
                .Include(v => v.user)
                .Include(v => v.ad)
                .ToList();
        }

        public List<AdUserView> GetViews()
        {
            return _context
                .AdUserViews
                .Include(v => v.user)
                .Include(v => v.ad)
                .ToList();
        }

        public List<AdUserWhatsappView> GetWhatsappViews()
        {
            return _context
                .AdUserWhatsappViews
                .Include(v => v.user)
                .Include(v => v.ad)
                .ToList();
        }

        public void LogPhoneView(AdUserPhoneView adUserPhoneView)
        {
            _context.AdUserPhoneViews.Add(adUserPhoneView);
        }

        public void LogView(AdUserView adUserView)
        {
            _context.AdUserViews.Add(adUserView);
        }

        public void LogWhatsappView(AdUserWhatsappView adUserWhatsappView)
        {
            _context.AdUserWhatsappViews.Add(adUserWhatsappView);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

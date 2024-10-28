using Moghtareb.Models;

namespace Moghtareb.Repositories
{
    public class AdReportRepository : IAdReportRepository
    {
        private MoghtarebDbContext _context;
        public AdReportRepository(MoghtarebDbContext context)
        {
            _context = context;
        }
        public void Create(AdReport report)
        {
            _context.AdReports.Add(report);
        }

        public List<AdReport> GetAll()
        {
            return _context.AdReports.ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

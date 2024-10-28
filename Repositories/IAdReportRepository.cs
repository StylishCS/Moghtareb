using Moghtareb.Models;

namespace Moghtareb.Repositories
{
    public interface IAdReportRepository
    {
        void Create(AdReport report);
        List<AdReport> GetAll();
        void Save();
    }
}
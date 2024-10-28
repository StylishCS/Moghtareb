using Moghtareb.Models;

namespace Moghtareb.Repositories
{
    public interface ITransactionRepository
    {
        void Create(Transaction transaction);
        List<Transaction> GetAll();
        Transaction GetById(int id);
        void Save();
    }
}
using Microsoft.EntityFrameworkCore;
using Moghtareb.Models;

namespace Moghtareb.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly MoghtarebDbContext _context;
        public TransactionRepository(MoghtarebDbContext context)
        {
            _context = context;
        }
        public void Create(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
        }

        public List<Transaction> GetAll()
        {
            return _context.Transactions.Include(t => t.userId).ToList();
        }

        public Transaction GetById(int id)
        {
            return _context.Transactions.Include(t => t.userId).FirstOrDefault(t => t.id == id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

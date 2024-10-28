using Microsoft.EntityFrameworkCore;

namespace Moghtareb.Models
{
    public class MoghtarebDbContext : DbContext
    {
        public MoghtarebDbContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Ad> Ads { get; set; }
        public DbSet<AdUserWishlist> AdUserWishlists { get; set; }
        public DbSet<AdImage> AdImages{ get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<AdReport> AdReports { get; set; }
        public DbSet<AdUserView> AdUserViews{ get; set; }
        public DbSet<AdUserPhoneView> AdUserPhoneViews { get; set; }
        public DbSet<AdUserWhatsappView> AdUserWhatsappViews { get; set; }
    }
}

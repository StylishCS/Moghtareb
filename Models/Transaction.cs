using System.ComponentModel.DataAnnotations.Schema;

namespace Moghtareb.Models
{
    public class Transaction
    {
        public int id { get; set; }
        public string stripeTransactionId { get; set; }
        public string type { get; set; }
        public double amount { get; set; }
        [ForeignKey("user")]
        public int userId { get; set; }
        public User user { get; set; }
        public DateTime issuedAt { get; set; } = DateTime.UtcNow;

    }
}

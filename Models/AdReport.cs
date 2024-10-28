using System.ComponentModel.DataAnnotations;

namespace Moghtareb.Models
{
    public class AdReport
    {
        public int id { get; set; }
        public int adId { get; set; }
        public Ad? ad { get; set; }
        public string subject { get; set; }
        [MaxLength(255)]
        public string? message { get; set; }
        public DateTime? createdAt { get; set; } = DateTime.UtcNow;
    }
}

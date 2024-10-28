using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moghtareb.Models
{
    [PrimaryKey(nameof(userId), nameof(adId))]
    public class AdUserWishlist
    {
        [ForeignKey("user")]
        public int userId { get; set; }
        public User user { get; set; }

        [ForeignKey("ad")]
        public int adId { get; set; }
        public Ad ad { get; set; }
    }
}

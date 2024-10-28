using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moghtareb.Models
{
    public class AdImage
    {
        [Key]
        public string imageUrl { get; set; }
        [ForeignKey("ad")]
        public int adId { get; set; }
        public virtual Ad ad { get; set; }
    }
}

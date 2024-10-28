using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moghtareb.Models
{
    public class Ad
    {
        public int id { get; set; }
        [MaxLength(100, ErrorMessage = "Title must be 100 characters max")]
        public string title { get; set; }
        [ForeignKey("user")]
        public int userId { get; set; }
        public User? user { get; set; }
        [Range(1,100000, ErrorMessage = "Price number is invalid")]
        public double price { get; set; }
        //[MaxLength(500, ErrorMessage = "Description must be 255 characters max")]
        public string description { get; set; }
        [MaxLength(50, ErrorMessage = "Governorate must be 50 characters max")]
        public string governorate { get; set; }
        [MaxLength(50, ErrorMessage = "City must be 50 characters max")]
        public string city { get; set; }
        [MaxLength(255, ErrorMessage = "Address must be 255 characters max")]
        public string address { get; set; }
        [RegularExpression("^(weekly|monthly|yearly)$", ErrorMessage = "Please select proper option")]
        public string rentalFrequency { get; set; }
        [Range(1,1000, ErrorMessage = "Area number is invalid")]
        public double area { get; set; }
        [Range(1,10, ErrorMessage = "Bedrooms number is invalid")]
        public int bedrooms { get; set; }
        [Range(1, 10, ErrorMessage = "Bathrooms number is invalid")]
        public int bathrooms { get; set; }
        [Range(1, 20, ErrorMessage = "Level number is invalid")]
        public int level { get; set; }
        [Range(0, 200000, ErrorMessage = "Deposite number is invalid")]
        public int deposit { get; set; }
        [Range(0, 200000, ErrorMessage = "Insurance number is invalid")]
        public int insurance { get; set; }
        //[RegularExpression(@"^(\s*[a-zA-Z0-9]+\s*)(,\s*[a-zA-Z0-9]+\s*)*$", ErrorMessage = "Something Went Wrong")] // seperated by commas
        public string amenities { get; set; }
        public bool isRented { get; set; } = false;
        public bool isDeleted { get; set; } = false;
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public DateTime updatedAt { get; set; } = DateTime.UtcNow;
        public virtual ICollection<AdUserView> views { get; set; } = new List<AdUserView>();
        public virtual ICollection<AdUserPhoneView> phoneViews { get; set; } = new List<AdUserPhoneView>();
        public virtual ICollection<AdUserWhatsappView> whatsViews { get; set; } = new List<AdUserWhatsappView>();
        public virtual ICollection<AdUserWishlist> Wishlist { get; set; } = new List<AdUserWishlist>();
        public virtual ICollection<AdImage> Images { get; set; } = new List<AdImage>();
    }
}

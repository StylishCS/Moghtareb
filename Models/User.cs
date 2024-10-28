using System.ComponentModel.DataAnnotations;

namespace Moghtareb.Models
{
    public class User
    {
        public int id{ get; set; }
        [MaxLength(25, ErrorMessage = "Name must be 25 characters max")]
        public string name { get; set; }
        public string? image { get; set; } = "/images/avatar.png";
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and be at least 8 characters long.")]
        public string password { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? phone { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? whatsapp{ get; set; }
        [MaxLength(50, ErrorMessage = "University must be 50 characters max")]
        public string? university { get; set; }
        [MaxLength(50, ErrorMessage = "Governorate must be 50 characters max")]
        public string? governorate { get; set; }
        [MaxLength(50, ErrorMessage = "City must be 50 characters max")]
        public string? city { get; set; }
        [Range(18,80, ErrorMessage = "You must be older than 18 years old")]
        public int? age { get; set; }
        [MaxLength(50, ErrorMessage = "Degree must be 50 characters max")]
        public string? degree { get; set; }
        public bool isDeleted { get; set; } = false;
        public bool isFeatured { get; set; } = false;
        public bool isSecured { get; set; } = false;
        public string? otp {  get; set; }
        public DateTime? deletionDate { get; set; }
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public virtual ICollection<Ad> Ads { get; set; } = new List<Ad>();

        public virtual ICollection<AdUserWishlist> Wishlist { get; set; } = new List<AdUserWishlist>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public virtual ICollection<AdUserView> views { get; set; } = new List<AdUserView>();
        public virtual ICollection<AdUserPhoneView> phoneViews { get; set; } = new List<AdUserPhoneView>();
        public virtual ICollection<AdUserWhatsappView> whatsViews { get; set; } = new List<AdUserWhatsappView>();
    }
}

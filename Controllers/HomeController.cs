using Microsoft.AspNetCore.Mvc;
using Moghtareb.Models;
using Moghtareb.Repositories;
using X.PagedList.Extensions;

namespace Moghtareb.Controllers
{
    public class HomeController : Controller
    {
        private IAdRepository _adRepository;
        private IWishlistRepository _wishlistRepository;
        private IAdViewsDetailsRepository _adViewsDetailsRepository;
        private IUserRepository _userRepository;
        public HomeController(IAdRepository adRepository, IWishlistRepository wishlistRepository, IAdViewsDetailsRepository adViewsDetailsRepository, IUserRepository userRepository)
        {
            _adRepository = adRepository;
            _wishlistRepository = wishlistRepository;
            _adViewsDetailsRepository = adViewsDetailsRepository;
            _userRepository = userRepository;
        }
        public IActionResult Index(int? page)
        {
            int pageSize = 6; // 9 items per page (3 rows of 3 items)
            int pageNumber = (page ?? 1);
            List<Ad> ads = _adRepository.GetAll();
            var paginatedAds = ads.ToPagedList(pageNumber, pageSize);
            return View(paginatedAds);
        }

        public IActionResult Details(int id)
        {
            // Get the ad by ID
            Ad ad = _adRepository.GetById(id);
            ViewBag.IsInWishlist = false;

            // Fetch and send the views count to the view
            int viewsCount = _adViewsDetailsRepository.GetViews().Count(v => v.adId == ad.id);
            ViewBag.ViewsCount = viewsCount;

            int phoneViewsCount = _adViewsDetailsRepository.GetPhoneViews().Count(v => v.adId == ad.id);
            ViewBag.PhoneViewsCount = phoneViewsCount;

            int whatsViewsCount = _adViewsDetailsRepository.GetWhatsappViews().Count(v => v.adId == ad.id);
            ViewBag.WhatsViewsCount = whatsViewsCount;

            // Check if the user is authenticated
            var userIdClaim = User.Claims.FirstOrDefault(u => u.Type == "id")?.Value;
            if (userIdClaim != null)
            {
                int userId = int.Parse(userIdClaim);
                ViewBag.userId = userId;

                // Check if the ad is in the user's wishlist
                bool isInWishlist = _wishlistRepository.isInWishList(userId, ad.id);
                ViewBag.IsInWishlist = isInWishlist;

                // Check if the user has already viewed this ad
                var existingView = _adViewsDetailsRepository.GetViews()
                    .FirstOrDefault(v => v.adId == ad.id && v.userId == userId);

                if (existingView == null)
                {
                    // If not viewed, log the view
                    var newView = new AdUserView
                    {
                        adId = ad.id,
                        userId = userId,
                        ad = ad,
                        user = _userRepository.GetById(userId)
                    };

                    _adViewsDetailsRepository.LogView(newView);
                    _adViewsDetailsRepository.Save();
                }
            }

            // Fetch related ads
            ViewBag.relatedAds = _adRepository.RelatedAds(ad.city).Take(2).ToList();

            return View(ad);
        }


        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult Features()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userClaim = User.Claims.FirstOrDefault(u => u.Type == "id").Value;
                int userId = int.Parse(userClaim);
                User user = _userRepository.GetById(userId);
                ViewBag.isFeatured = user.isFeatured;
            }
            return View();
        }

        public IActionResult Integrations()
        {
            return View();
        }
    }
}

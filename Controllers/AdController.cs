using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moghtareb.Models;
using Moghtareb.Repositories;
using System.Security.Claims;
using X.PagedList.Extensions;

namespace Moghtareb.Controllers
{
    public class AdController : Controller
    {
        private readonly IAdRepository _adRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAdViewsDetailsRepository _adViewsDetailsRepository;
        private readonly IAdReportRepository _adReportRepository;
        public AdController(IAdRepository adRepository, IUserRepository userRepository, IAdViewsDetailsRepository adViewsDetailsRepository, IAdReportRepository adReportRepository)
        {
            _adRepository = adRepository;
            _userRepository = userRepository;
            _adViewsDetailsRepository = adViewsDetailsRepository;
            _adReportRepository = adReportRepository;
        }
        public IActionResult Index(string title, double? minPrice, double? maxPrice, string city, int? bedrooms, int? page)
        {
            int pageSize = 9; // 9 items per page
            int pageNumber = (page ?? 1);

            // Start with IQueryable
            var ads = _adRepository.GetAll().AsQueryable();

            // Apply Filters
            if (!string.IsNullOrEmpty(title))
            {
                ads = ads.Where(a => a.title.Contains(title, StringComparison.OrdinalIgnoreCase));
            }

            if (minPrice.HasValue)
            {
                ads = ads.Where(a => a.price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                ads = ads.Where(a => a.price <= maxPrice.Value);
            }

            if (!string.IsNullOrEmpty(city))
            {
                ads = ads.Where(a => a.city.Equals(city, StringComparison.OrdinalIgnoreCase));
            }

            if (bedrooms.HasValue)
            {
                ads = ads.Where(a => a.bedrooms == bedrooms.Value);
            }

            // Pass the search filters back to the view for pagination
            ViewBag.Title = title;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.City = city;
            ViewBag.Bedrooms = bedrooms;

            // Paginate the filtered list
            var paginatedAds = ads.ToPagedList(pageNumber, pageSize);

            return View(paginatedAds);
        }

        [Authorize]
        public IActionResult Create()
        {
            var userClaim = User.Claims.FirstOrDefault(u => u.Type == "id").Value;
            int id = int.Parse(userClaim);
            User user = _userRepository.GetById(id);
            if (!user.isSecured)
            {
                return RedirectToAction("TwoFactorAuth", "Authentication");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ad ad, List<IFormFile> ImageUrls, string phone, string whatsapp)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var userId = User.Claims.FirstOrDefault(u => u.Type == "id").Value;
            int userIdParsed = int.Parse(userId);
            User user = _userRepository.GetById(userIdParsed);
            var userClaimsIdentity = User.Identity as ClaimsIdentity;
            if(user.phone != phone)
            {
                user.phone = phone;
                var oldPhoneClaim = userClaimsIdentity?.FindFirst("phone");
                if (oldPhoneClaim != null)
                {
                    userClaimsIdentity.RemoveClaim(oldPhoneClaim);
                }
                userClaimsIdentity?.AddClaim(new Claim("phone", phone));
            }
            if(user.whatsapp != whatsapp)
            {
                user.whatsapp = whatsapp;
                var oldWhatsClaim = userClaimsIdentity?.FindFirst("whatsapp");
                if (oldWhatsClaim != null)
                {
                    userClaimsIdentity.RemoveClaim(oldWhatsClaim);
                }
                userClaimsIdentity?.AddClaim(new Claim("whatsapp", whatsapp));
            }
            _userRepository.Save();
            await HttpContext.SignOutAsync();
            await HttpContext.SignInAsync(new ClaimsPrincipal(userClaimsIdentity));
            ad.Images = new List<AdImage>();

            if (ImageUrls != null && ImageUrls.Count > 0 && ImageUrls.Count <= 5)
            {
                foreach (var imageFile in ImageUrls)
                {
                    if (imageFile.Length > 0)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var filePath = Path.Combine("wwwroot/images", uniqueFileName);

                        var adImage = new AdImage
                        {
                            imageUrl = "/images/" + uniqueFileName, 
                            adId = ad.id 
                        };

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        ad.Images.Add(adImage);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("error","Please add at least one image out of 5 images");
                return View();
            }

            _adRepository.Create(ad);
            await _adRepository.SaveAsync();

            return RedirectToAction("Index", "Home");
        }


        public IActionResult Edit(int id)
        {
            Ad ad = _adRepository.GetById(id);
            return View(ad);
        }

        [HttpPost]
        public IActionResult Edit(Ad ad)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Ad adFromDb = _adRepository.GetById(ad.id);
            adFromDb.area = ad.area;
            adFromDb.bedrooms = ad.bedrooms;
            adFromDb.bathrooms = ad.bathrooms;
            adFromDb.level = ad.level;
            adFromDb.amenities = ad.amenities;
            adFromDb.title = ad.title;
            adFromDb.description = ad.description;
            adFromDb.governorate = ad.governorate;
            adFromDb.city = ad.city;
            adFromDb.address = ad.address;
            adFromDb.price = ad.price;
            adFromDb.rentalFrequency = ad.rentalFrequency;
            adFromDb.deposit = ad.deposit;
            adFromDb.insurance = ad.insurance;
            adFromDb.updatedAt = DateTime.UtcNow;
            _adRepository.Save();
            return RedirectToAction("Details", "Home", new { ad.id });
        }


        public IActionResult Rented(int id)
        {
            _adRepository.MarkAsRented(id);
            _adRepository.Save();
            return RedirectToAction("Details","Home", new { id });
        }

        public IActionResult Delete(int id)
        {
            _adRepository.Delete(id);
            _adRepository.Save();
            var userClaim = User.Claims.FirstOrDefault(u => u.Type == "id").Value;
            int userId = int.Parse(userClaim);
            return RedirectToAction("SellerProfile", "User", new {id = userId});
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddPhoneView(int id)
        {
            Ad ad = _adRepository.GetById(id);
            var userIdClaim = User.Claims.FirstOrDefault(u => u.Type == "id")?.Value;
            int userId = int.Parse(userIdClaim);
            // Check if the user has already viewed this ad
            var existingView = _adViewsDetailsRepository.GetPhoneViews()
                .FirstOrDefault(v => v.adId == ad.id && v.userId == userId);

            if (existingView == null)
            {
                // If not viewed, log the view
                var newView = new AdUserPhoneView
                {
                    adId = ad.id,
                    userId = userId,
                    ad = ad,
                    user = _userRepository.GetById(userId)
                };

                _adViewsDetailsRepository.LogPhoneView(newView);
                _adViewsDetailsRepository.Save();
            }
            return Ok();
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddWhatsView(int id)
        {
            Ad ad = _adRepository.GetById(id);
            var userIdClaim = User.Claims.FirstOrDefault(u => u.Type == "id")?.Value;
            int userId = int.Parse(userIdClaim);
            // Check if the user has already viewed this ad
            var existingView = _adViewsDetailsRepository.GetWhatsappViews()
                .FirstOrDefault(v => v.adId == ad.id && v.userId == userId);

            if (existingView == null)
            {
                // If not viewed, log the view
                var newView = new AdUserWhatsappView
                {
                    adId = ad.id,
                    userId = userId,
                    ad = ad,
                    user = _userRepository.GetById(userId)
                };

                _adViewsDetailsRepository.LogWhatsappView(newView);
                _adViewsDetailsRepository.Save();
            }
            return Ok();
        }

        [HttpPost]
        public IActionResult SubmitReport([FromBody] AdReport reportRequest)
        {
            if(ModelState.IsValid)
            {
                reportRequest.createdAt = DateTime.UtcNow;
                _adReportRepository.Create(reportRequest);
                _adReportRepository.Save();
                return Ok($"id:{reportRequest.adId} rep: {reportRequest.subject} com: {reportRequest.message}");
            }
            return BadRequest();
        }
    }
}

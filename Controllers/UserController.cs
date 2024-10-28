using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moghtareb.Models;
using Moghtareb.Repositories;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;
using X.PagedList.Extensions;

namespace Moghtareb.Controllers
{
    public class UserController : Controller
    {
        private IUserRepository _userRepository;
        private IAdRepository _adRepository;
        private IWishlistRepository _wishlistRepository;
        private ITransactionRepository _transactionRepository;
        public UserController(IUserRepository userRepository, IAdRepository adRepository, IWishlistRepository wishlistRepository, ITransactionRepository transactionRepository)
        {
            _userRepository = userRepository;
            _adRepository = adRepository;
            _wishlistRepository = wishlistRepository;
            _transactionRepository = transactionRepository;
        }
        [Authorize]
        public IActionResult Profile()
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == "id")!.Value;
            int userIdParsed = int.Parse(userId);
            User user = _userRepository.GetById(userIdParsed);
            return View(user);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Profile(User user, IFormFile ProfileImage)
        {
            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                // Define the folder where you want to save the uploaded image
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles");

                // Ensure the folder exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Create a unique file name for the uploaded image
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ProfileImage.FileName);

                // Create the path to save the file
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the image to the specified folder
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(fileStream);
                }

                // Set the image URL in the user model (relative path to the image)
                user.image = "/images/profiles/" + uniqueFileName;
            }

            // Fetch user from the database
            var userFromDb = await _userRepository.GetByIdAsync(user.id); // Ensure GetByIdAsync is async

            if (userFromDb != null)
            {
                // Update fields manually
                userFromDb.name = user.name;
                userFromDb.phone = user.phone;
                userFromDb.whatsapp = user.whatsapp;
                userFromDb.governorate = user.governorate;
                userFromDb.city = user.city;
                userFromDb.university = user.university;
                userFromDb.degree = user.degree;
                userFromDb.image = user.image ?? userFromDb.image; // Ensure the image is updated

                // Explicitly mark the entity as modified if needed
                _userRepository.Update(userFromDb);

                // Save changes
                await _userRepository.SaveAsync();
            }

            // Update the user's claims
            var userClaimsIdentity = User.Identity as ClaimsIdentity;
            if(User.Claims.FirstOrDefault(u => u.Type == "name").Value != user.name)
            {
                var oldNameClaim = userClaimsIdentity?.FindFirst("name");
                if (oldNameClaim != null)
                {
                    userClaimsIdentity.RemoveClaim(oldNameClaim);
                }

                userClaimsIdentity?.AddClaim(new Claim("name", user.name));
            }


            // Update the user's claims
            if (user.image != null && User.Claims.FirstOrDefault(u => u.Type == "image").Value != user.image)
            {
                var oldImageClaim = userClaimsIdentity?.FindFirst("image");
                if (oldImageClaim != null)
                {
                    userClaimsIdentity.RemoveClaim(oldImageClaim);
                }

                userClaimsIdentity?.AddClaim(new Claim("image", user.image ?? userFromDb.image));
            }

            if (userClaimsIdentity != null && !string.IsNullOrEmpty(user.phone))
            {
                // Find the existing "phone" claim (if any)
                var phoneClaim = userClaimsIdentity.FindFirst("phone");

                // If the phone claim doesn't exist, add a new one
                if (phoneClaim == null)
                {
                    userClaimsIdentity.AddClaim(new Claim("phone", user.phone));
                }
                else
                {
                    // If the phone claim exists and the phone number has changed, update it
                    if (phoneClaim.Value != user.phone)
                    {
                        // Remove the old claim
                        userClaimsIdentity.RemoveClaim(phoneClaim);

                        // Add the updated claim with the new phone number
                        userClaimsIdentity.AddClaim(new Claim("phone", user.phone));
                    }
                }
            }

            if (userClaimsIdentity != null && !string.IsNullOrEmpty(user.whatsapp))
            {
                // Find the existing "phone" claim (if any)
                var whatsappClaim = userClaimsIdentity.FindFirst("whatsapp");

                // If the phone claim doesn't exist, add a new one
                if (whatsappClaim == null)
                {
                    userClaimsIdentity.AddClaim(new Claim("whatsapp", user.whatsapp));
                }
                else
                {
                    // If the phone claim exists and the phone number has changed, update it
                    if (whatsappClaim.Value != user.whatsapp)
                    {
                        // Remove the old claim
                        userClaimsIdentity.RemoveClaim(whatsappClaim);

                        // Add the updated claim with the new phone number
                        userClaimsIdentity.AddClaim(new Claim("whatsapp", user.whatsapp));
                    }
                }
            }

            await HttpContext.SignOutAsync();
            await HttpContext.SignInAsync(new ClaimsPrincipal(userClaimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Test()
        {
            var name = User.Claims.FirstOrDefault(u => u.Type == "name");
            var image = User.Claims.FirstOrDefault(u => u.Type == "image");
            var phone = User.Claims.FirstOrDefault(u => u.Type == "phone");
            var whats = User.Claims.FirstOrDefault(u => u.Type == "whatsapp");
            return Content($"name: {name}\nimage: {image}\nphone: {phone}\nwhats: {whats}\n");
        }

        [Authorize]
        public IActionResult MyAds(int? page)
        {
            int pageSize = 9; // 9 items per page (3 rows of 3 items)
            int pageNumber = (page ?? 1);
            var userIdClaim = User.Claims.FirstOrDefault(u => u.Type == "id");
            int userId = int.Parse(userIdClaim.Value);
            List<Ad> ads = _userRepository.GetAds(int.Parse(User.Claims.FirstOrDefault(u => u.Type == "id").Value));
            var paginatedAds = ads.ToPagedList(pageNumber, pageSize);
            return View(paginatedAds);
        }

        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteAccount()
        {
            return View();
        }

        [Authorize]
        public IActionResult Wishlist()
        {
            return View();
        }

        public IActionResult SellerProfile(int? page,int? pageWishlist, int id)
        {
            User user = _userRepository.GetById(id);
            int pageSize = 9; // 9 items per page (3 rows of 3 items)
            int pageNumber = (page ?? 1);
            List<Ad> ads = _userRepository.GetAds(id);
            var paginatedAds = ads.ToPagedList(pageNumber, pageSize);
            ViewBag.ads = paginatedAds;
            int pageNumberWishlist = (pageWishlist ?? 1);
            List<AdUserWishlist> wishlist = user.Wishlist.ToList();
            var paginatedWishlist = wishlist.ToPagedList(pageNumberWishlist, pageSize);
            ViewBag.wishlist = paginatedWishlist;
            return View(user);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddToWishlist(string id)
        {
            int pasredId = int.Parse(id);
            var userIdClaim = User.Claims.FirstOrDefault(u => u.Type == "id").Value;
            int userId = int.Parse(userIdClaim);

            // Check if the ad is already in the wishlist
            var existingWishlistItem = _wishlistRepository.GetWishlist().FirstOrDefault(w => w.userId == userId && w.adId == pasredId);

            if (existingWishlistItem != null)
            {
                _wishlistRepository.Remove(userId, pasredId);
                _wishlistRepository.Save();
                return Json(new { success = true, message = "Removed From Wishlist" });
            }

            // Add the ad to the wishlist
            var wishlistItem = new AdUserWishlist
            {
                userId = userId,
                adId = pasredId,
            };

            _wishlistRepository.Add(wishlistItem);
            _wishlistRepository.Save();

            return Json(new { success = true, message = "Added to wishlist" });
        }

        [Authorize]
        public IActionResult Sponsor()
        {
            var userClaim = User.Claims.FirstOrDefault(u => u.Type == "id").Value;
            int userId = int.Parse(userClaim);
            User user = _userRepository.GetById(userId);
            string domain = "https://localhost:7181/";

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"User/Success",
                CancelUrl = domain + $"User/Failure",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = user.email
            };

            var sessionListItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = 5999,
                    Currency = "USD",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Moghtareb Premium Subscribtion",
                        Description = "Go Premium With Moghtareb Now!"
                    },
                },
                Quantity = 1
            };

            options.LineItems.Add(sessionListItem);

            var client = new StripeClient(Environment.GetEnvironmentVariable("STRIPE_SK"));
            var service = new SessionService(client);
            Session session = service.Create(options);

            TempData["Session"] = session.Id;

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public IActionResult Success()
        {
            var client = new StripeClient("sk_test_51P7JUzGOuS07aKfxnpJA9F2YtjOGDQl4bKVYb0whjroUYGPj1izMs8eGjoXy5xLPVFSFxXFDAXlVMGsXeyZGHQwh00uecNUEJN");
            var service = new SessionService(client);
            Session session = service.Get(TempData["Session"].ToString());
            if (session.PaymentStatus == "paid")
            {
                var userClaim = User.Claims.FirstOrDefault(u => u.Type == "id").Value;
                int userId = int.Parse(userClaim);
                User user = _userRepository.GetById(userId);
                user.isFeatured = true;
                var transaction = session.PaymentIntentId.ToString();
                _transactionRepository.Create(new Transaction
                {
                    stripeTransactionId = transaction,
                    type = "Premium Subscribtion",
                    userId = userId,
                    amount = (double)session.AmountTotal,
                });
                _userRepository.Save();
                _transactionRepository.Save();
                return View();
            }
            return RedirectToAction("Failure");
        }
        public IActionResult Failure()
        {
            return View();
        }
    }
}

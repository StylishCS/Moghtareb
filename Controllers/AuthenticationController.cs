using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Moghtareb.Models;
using Moghtareb.Repositories;
using Moghtareb.Services;
using Moghtareb.ViewModels;
using System.Security.Claims;

namespace Moghtareb.Controllers
{
    public class AuthenticationController : Controller
    {
        private IUserServices _userServices;
        private IUserRepository _userRepository;
        private IEmailSender _emailSender;
        public AuthenticationController(IUserServices userServices, IUserRepository userRepository, IEmailSender emailSender)
        {
            _userServices = userServices;
            _userRepository = userRepository;
            _emailSender = emailSender;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            if(ModelState.IsValid)
            {
                if (_userRepository.Exists(user.email))
                {
                    ModelState.AddModelError("email", "User Already Exist With This Email");
                    return View();
                }
                user.password = _userServices.HashPassword(user.password);
                _userRepository.Create(user);
                _userRepository.Save();
                ClaimsIdentity claims = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                claims.AddClaim(new Claim("id",user.id.ToString()));
                claims.AddClaim(new Claim("name",user.name));
                claims.AddClaim(new Claim("id", user.id.ToString()));
                claims.AddClaim(new Claim("name", user.name));
                claims.AddClaim(new Claim("image", user.image));
                if (user.phone != null)
                {
                    claims.AddClaim(new Claim("phone", user.phone));
                }
                if (user.whatsapp != null)
                {
                    claims.AddClaim(new Claim("whatsapp", user.whatsapp));
                }
                if (user.phone != null)
                {
                    claims.AddClaim(new Claim("phone", user.phone));
                }
                if(user.whatsapp != null)
                {
                    claims.AddClaim(new Claim("whatsapp", user.whatsapp));
                }
                ClaimsPrincipal principal = new ClaimsPrincipal(claims);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel userVM)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if(!_userRepository.Exists(userVM.email))
            {
                ModelState.AddModelError("error", "wrong email or password");
                return View();
            }
            User user = _userRepository.Get(userVM.email);
            if(!_userServices.VerifyPassword(userVM.password, user.password))
            {
                ModelState.AddModelError("error", "wrong email or password");
                return View();
            }
            if (user.isSecured)
            {
                Random r = new Random();
                int otp = r.Next(1000, 10000);

                // Hash the OTP
                string hashedValue = _userServices.HashPassword(otp.ToString());
                user.otp = hashedValue;
                _userRepository.Save();

                await _emailSender.SendEmailAsync(user.email, "Login Attempt",
                    $"Your Verification Code is {otp}");
                return RedirectToAction("VerifyOTP", new {user.id});
            }
            ClaimsIdentity claims = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            claims.AddClaim(new Claim("id", user.id.ToString()));
            claims.AddClaim(new Claim("name", user.name));
            claims.AddClaim(new Claim("image", user.image));
            if(user.phone != null)
            {
                claims.AddClaim(new Claim("phone", user.phone));
            }
            if(user.whatsapp != null)
            {
                claims.AddClaim(new Claim("whatsapp", user.whatsapp));
            }
            ClaimsPrincipal principal = new ClaimsPrincipal(claims);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult VerifyOTP(User user)
        {
            UserOtpViewModel model = new UserOtpViewModel();
            model.id = user.id;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyOTP(UserOtpViewModel model)
        {
            User user = _userRepository.GetById(model.id);
            bool valid = _userServices.VerifyPassword(model.otp.ToString(), user.otp);
            if(!valid)
            {
                ModelState.AddModelError("otp", "Wrong Verification Code");
                return View(model);
            }
            user.otp = null;
            _userRepository.Save();
            ClaimsIdentity claims = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            claims.AddClaim(new Claim("id", user.id.ToString()));
            claims.AddClaim(new Claim("name", user.name));
            claims.AddClaim(new Claim("image", user.image));
            if (user.phone != null)
            {
                claims.AddClaim(new Claim("phone", user.phone));
            }
            if (user.whatsapp != null)
            {
                claims.AddClaim(new Claim("whatsapp", user.whatsapp));
            }
            ClaimsPrincipal principal = new ClaimsPrincipal(claims);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> ResendOTP(int id)
        {
            User user = _userRepository.GetById(id);
            Random r = new Random();
            int otp = r.Next(1000, 10000);

            // Hash the OTP
            string hashedValue = _userServices.HashPassword(otp.ToString());
            user.otp = hashedValue;
            _userRepository.Save();

            await _emailSender.SendEmailAsync(user.email, "Login Attempt",
                $"Your Verification Code is {otp}");

            return Ok();
        }

        [Authorize]
        public IActionResult TwoFactorAuth()
        {
            var userClaim = User.Claims.FirstOrDefault(u => u.Type == "id").Value;
            int userId = int.Parse(userClaim);
            User user = _userRepository.GetById(userId);
            return View(user);
        }

        [Authorize]
        public async Task<IActionResult> SendMail(string email)
        {
            var userClaim = User.Claims.FirstOrDefault(u => u.Type == "id").Value;
            int userId = int.Parse(userClaim);
            User user = _userRepository.GetById(userId);

            if (user.email != email)
            {
                user.email = email;
            }

            Random r = new Random();
            int otp = r.Next(1000, 10000);

            // Hash the OTP
            string hashedValue = _userServices.HashPassword(otp.ToString());

            // Convert to Base64 URL-safe format
            string base64Hash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(hashedValue));
            string encodedHash = base64Hash.Replace("+", "-").Replace("/", "_").Replace("=", "");

            user.otp = hashedValue;
            _userRepository.Save();

            await _emailSender.SendEmailAsync(email, "Two Factor Authentication Request",
                $"Click the following link to proceed: https://localhost:7181/Authentication/Verify2FA/{encodedHash}");

            return Ok();
        }


        [Authorize]
        public IActionResult Verify2FA(string id)
        {
            var userClaim = User.Claims.FirstOrDefault(u => u.Type == "id").Value;
            int userId = int.Parse(userClaim);
            User user = _userRepository.GetById(userId);
            // Convert Base64 URL-safe string back to regular Base64 format
            string base64Hash = id.Replace("-", "+").Replace("_", "/");
            int padding = 4 - (base64Hash.Length % 4);
            if (padding != 4)
            {
                base64Hash += new string('=', padding);
            }

            // Decode the Base64 string back to its original hashed value
            string hashedValue = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(base64Hash));

            // Verify the hash
            bool valid = user.otp == hashedValue;
            if (!valid)
            {
                return RedirectToAction("Failure");
            }
            // user secure = true
            user.isSecured = true;
            // delete otp from user
            user.otp = null;
            _userRepository.Save();

            return RedirectToAction("Index", "Home");
        }
    }
}

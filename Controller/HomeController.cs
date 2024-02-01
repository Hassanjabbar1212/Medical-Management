using Medical.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Medical.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

namespace Medical.Controllers
{
    public class HomeController : Controller
    {
        private readonly Context _context;
        public HomeController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User loginPage)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginPage.Email && u.Password == loginPage.Password);

            if (user != null)
            { 
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Sid, Convert.ToString(user?.Id)),
                // Add other claims as needed
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return RedirectToAction("Profile", "Home");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Register(User user)
        {
            if(ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
            }
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(User password1)
        {
            var logedInId = User.FindFirstValue(ClaimTypes.Sid);

            if (int.TryParse(logedInId, out int userId))
            {
                var user = _context.Users.FirstOrDefault(a => a.Id == userId);
                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    // Update the existing user's password
                    user.Password = password1.Password;

                    // Save the changes to the database
                    _context.SaveChanges();
                }
            }

            return View();

        }
        public IActionResult Profile()
        {

            return View();
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                ModelState.AddModelError("Email", "Email is required.");
                return RedirectToAction("Index", "Home"); // Display the form again with an error message.
            }

            // Check if the email exists in your user database.
            // If it exists, generate a random password and send it via email.

            var user = _context.Users.FirstOrDefault(u => u.Email == Email);

            if (user != null)
            {
                // Generate a random password
                //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                //var random = new Random();
                //var randomPassword = new string(Enumerable.Repeat(chars, 12)
                //    .Select(s => s[random.Next(s.Length)]).ToArray());

                //// Update the user's password in the database with the random password.
                //user.Password = randomPassword;


                //_context.SaveChanges();
                var password = user.Password;
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("hassanjabbar2017@gmail.com", "poljeaszpdjiywtk"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("hassanjabbar2017@gmail.com"), // Replace with your Gmail email address.
                    Subject = "Your Password",
                    Body = $"Your password is: {password}",
                    IsBodyHtml = false,
                };

                mailMessage.To.Add(Email); // Use the provided email as the recipient.

                smtpClient.Send(mailMessage);

                return RedirectToAction("Login", "Home");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
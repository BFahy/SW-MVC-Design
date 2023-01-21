using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sams_Warehouse.Data;
using Sams_Warehouse.Models;

namespace Sams_Warehouse.Controllers
{
    public class UsersController : Controller
    {
        /**
        * Global variable declaration - used for DB calls.
        */
        private readonly ShoppingContext _context;

        public UsersController(ShoppingContext context)
        {
            _context = context;
        }
        #region Login/Logout
        /**
         * Returns login page for email/password entry.
         */
        public IActionResult Login()
        {
            return View();
        }

        /**
         * Submitting login information to determine if entry is valid, before authenticating user.
         * If successful, redirects to shopping/index.
         * Else displays error message to user via ViewBag.
         */
        [HttpPost]
        public IActionResult Login(User user)
        {
         User logged = _context.Users.Where(c => c.Email == user.Email && c.Password == user.Password)
        .FirstOrDefault();
                if (ModelState.IsValid)
                {
                    if (logged != null)
                    {
                        HttpContext.Session.SetString("User", logged.Username);
                        HttpContext.Session.SetString("Authenticated", "True");
                        HttpContext.Session.SetString("Id", "" + logged.Id);

                        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, logged.Username)
                    };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            AllowRefresh = true,
                            IsPersistent = true
                        };

                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        return RedirectToAction("Index", "Shopping");
                    }
                }
                ViewBag.ErrorMessage = "Invalid username or password";
                return View(user);
            }


        /**
         * Called to reset login status and redirect home.
         */
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Shopping");
        }

        #endregion

        #region Registration
        /**
         * Returning view for user to enter information related to registering their account.
         */
        public IActionResult Register()
        {
            return View();
        }

        /**
         * Post request to create a user account.
         * If successful, user is redirected to home page else displays ViewBag error.
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {

            if (!user.Email.Contains('@'))
            {
                ViewBag.ErrorMessage = "Enter a valid email address to continue.";
                return View(user);
            }

            if (!_context.Users.Any(x => x.Email == user.Email))
            {
                var newUser = new User
                {
                    Email = user.Email,
                    Username = user.Username,                    
                    Password = user.Password
                };
                _context.Users.Add(newUser);
                _context.SaveChanges();
                return RedirectToAction("Index", "Shopping");
            }

            ViewBag.ErrorMessage = "Email is already being used.";
            return View(user);
        }
        #endregion

        #region Updating name
        /**
         * Returning view for user to update their personal display name.
         */
        public IActionResult UpdateName()
        {
            return View();
        }
        
        /**
         * Update request to specified userId, for replacing username with new selection.
         */
        [HttpPut]
        public async Task<IActionResult> UpdateName([FromQuery] int userId, [FromBody] string newUsername)
        {
            User user = _context.Users.Where(c => c.Id == userId).FirstOrDefault();

            if (user == null)
            {
                ViewBag.ErrorMessage = "Something went wrong";
                return BadRequest();
            }

            if (newUsername.Length <= 0)
            {
                ViewBag.ErrorMessage = "Provide a new username";
                return BadRequest();
            }

            user.Username = newUsername;

            HttpContext.Session.SetString("User", newUsername);

            _context.SaveChanges();
            return Ok();
        }
        #endregion

        #region Unused
        /**
         * Returning view for authorisation check (Not used).
         */
        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basics.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }
        public IActionResult Authenticate()
        {
            var grandmaClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Bob"), 
                new Claim(ClaimTypes.Email, "Bob@fmail.com"),
                new Claim("Grandma.Says", "Very nice boy.")
            };
            var licenceClaims = new List<Claim>() 
            {
                new Claim(ClaimTypes.Name, "Bob R Too"),
                new Claim("DrivingLicense", "Bob@fmail.com") 
            };
            var licenseIdentity = new ClaimsIdentity(
                licenceClaims, "licenseIdentity");

            var grandmaIdentuty = new ClaimsIdentity(
                grandmaClaims, "Grandma Identity");

            var usePrincipal = new ClaimsPrincipal(new[] { grandmaIdentuty, 
                licenseIdentity });

            HttpContext.SignInAsync(usePrincipal);


           return RedirectToAction("Index");
        }

    }

}
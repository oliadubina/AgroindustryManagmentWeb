using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroindustryManagementWeb.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = "/"
            }, "OpenIdConnect");
        }
        public IActionResult Logout()
        {
            return SignOut(new AuthenticationProperties
            {
                RedirectUri = "/"
            }, "Cookies", "OpenIdConnect");
        }
        [Authorize]
        public IActionResult Dashboard()
        {
            // The 'User' object is automatically populated by the cookie 
            // created from the IdentityServer token.
            return View();
        }
    }
}

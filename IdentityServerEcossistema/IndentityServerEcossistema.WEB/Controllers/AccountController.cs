using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace IndentityServerEcossistema.WEB.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return new SignOutResult(new[] { "oidc", "Cookies" });
        }

        public IActionResult Login() =>
          Challenge(new AuthenticationProperties
          {
              RedirectUri = "/"
          });
    }
}

using DataAccessLayer.Concrete;
using EntityLayer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustomsTracking_.Controllers
{
    public class LoginController : Controller
    {
        Context c=new Context();
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task< IActionResult> Index(User p)
        {
            var data = c.Users.FirstOrDefault(x => x.Username == p.Username && x.Password == p.Password);
            if(data != null)
            {
                
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,p.Username)
                };
                var useridentity=new ClaimsIdentity(claims, "Login");
                ClaimsPrincipal principal = new ClaimsPrincipal(useridentity);
                await HttpContext.SignInAsync("CookieAuthentication", principal);
                return RedirectToAction("Index", "Home");

            }
            return View();
        }

    }
}
    
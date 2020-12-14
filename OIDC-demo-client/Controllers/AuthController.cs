using Microsoft.AspNetCore.Mvc;

namespace OIDC_demo_client.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
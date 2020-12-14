using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using OIDC_demo_client.Clients;
using OIDC_demo_client.Models;

namespace OIDC_demo_client.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApiClient client;

        public HomeController(
            ILogger<HomeController> logger,
            ApiClient client)
        {
            _logger = logger;
            this.client = client;
        }

        public async Task<IActionResult> Index()
        {
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            Debug.WriteLine($"Token: {identityToken}");
            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"{claim.Type}:{claim.Value}");
            }

            return View(new HomeViewModel { ImageName = "Nothing" });
        }

        [Authorize(Roles = "Old")]
        public async Task<IActionResult> Privacy()
        {
            var imageName = await client.GetImageName();
            return View(imageName);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}

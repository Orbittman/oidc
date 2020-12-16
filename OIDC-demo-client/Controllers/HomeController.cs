using System;
using System.Diagnostics;
using System.Threading.Tasks;
using IdentityModel.Client;
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
        private readonly IDPClient idpClient;

        public HomeController(
            ILogger<HomeController> logger,
            ApiClient client,
            IDPClient idpClient)
        {
            _logger = logger;
            this.client = client;
            this.idpClient = idpClient;
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

        // [Authorize("CanAccessRestricedItems")]
        public async Task<IActionResult> Privacy()
        {
            var imageName = await client.GetImageName();
            return View();
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

            var discoveryDocument = await idpClient.GetDiscoveryDocumentAsync();
            if (discoveryDocument.IsError)
            {
                throw new Exception(discoveryDocument.Error);
            }

            var tokensToRevoke = new[] { OpenIdConnectParameterNames.AccessToken, OpenIdConnectParameterNames.RefreshToken };
            foreach (var token in tokensToRevoke)
            {
                var tokenRevocationResponse = await idpClient.RevokeTokenAsync(
                    discoveryDocument.RevocationEndpoint,
                        "oidc-demo-client",
                        "secret",
                        await HttpContext.GetTokenAsync(token));

                if (tokenRevocationResponse.IsError)
                {
                    throw new Exception(tokenRevocationResponse.Error);
                }
            }
        }
    }
}

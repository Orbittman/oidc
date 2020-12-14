using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace OIDC_demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class DataController : Controller
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetImages()
        {
            var userIdentity = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            return Content(await GetIdentityInformation(userIdentity));
        }

        public async Task<string> GetIdentityInformation(string userIdentity)
        {
            var identityToken = userIdentity; // await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            return $"User: {identityToken}";
        }
    }
}

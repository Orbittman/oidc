using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OIDC_demo.Controllers
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.IdentityModel.Protocols.OpenIdConnect;

    [ApiController]
    [Route("[controller]")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class DataController : Controller
    {
        [HttpGet]
        [Authorize("IsOldEnough")]
        public async Task<IActionResult> GetImages()
        {
            var userIdentity = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            return Content(await GetIdentityInformation());
        }

        public async Task<string> GetIdentityInformation()
        {
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            return $"User: {identityToken}";
        }
    }
}

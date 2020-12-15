using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace OIDC_demo_API.Authorisation
{
    public class IsOldEnough : IAuthorizationRequirement
    {
        public int MinAge { get; init; }

        public IsOldEnough (int minAge)
        {
            MinAge = minAge;
        }
    }

    public class IsOldEnoughHandler : AuthorizationHandler<IsOldEnough>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsOldEnough requirement)
        {
            _ = int.TryParse(context.User.Claims.FirstOrDefault(x => x.Type == "age")?.Value, out var age);
            if(age >= requirement.MinAge)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask; ;
        }
    }
}

using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace OIDC_demo_client.Clients
{
    public class IDPClient
    {
        private readonly HttpClient client;

        public IDPClient(HttpClient client)
        {
            this.client = client;
        }

        public async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync()
        {
            return await client.GetDiscoveryDocumentAsync();
        }

        internal Task<TokenResponse> RequestRefreshTokenAsync(string tokenEndpoint, string clientId, string secret, string refreshToken)
        {
            return client.RequestRefreshTokenAsync(new RefreshTokenRequest { Address = tokenEndpoint, ClientId = clientId, ClientSecret = secret, RefreshToken = refreshToken });
        }
    }
}

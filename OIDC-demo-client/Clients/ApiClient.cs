using System.Net.Http;
using System.Threading.Tasks;

namespace OIDC_demo_client.Clients
{
    public class ApiClient
    {
        private readonly HttpClient client;

        public ApiClient(HttpClient client, BearerTokenHandler tokenHandler)
        {
            this.client = client;
        }

        public async Task<string> GetImageName()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "data");
            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}

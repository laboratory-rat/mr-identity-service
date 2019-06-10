using MRIdentityClient.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Threading.Tasks;

namespace MRIdentityClient.Tools
{
    public class RequestSender
    {
        protected HttpClient _client => new HttpClient();
        protected JsonSerializerSettings _settings => new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        public async Task<IdentityResponse<T>> Put<T>(string url, object data = null)
            where T : class, new()
        {
            var content = data != null ? JsonConvert.SerializeObject(data, _settings) : string.Empty;
            var result = new IdentityResponse<T>();

            var response = await _client.PutAsync(url, new StringContent(content));
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                result.Error = JsonConvert.DeserializeObject<ApiError>(responseString, _settings);
            else
                result.Response = JsonConvert.DeserializeObject<T>(responseString, _settings);

            return result;
        }
    }
}

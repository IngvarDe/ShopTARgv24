using Nancy.Json;
using ShopTARgv24.Core.Dto.ChuckNorrisDto;
using ShopTARgv24.Core.ServiceInterface;
using System.Net;
using System.Text.Json;


namespace ShopTARgv24.ApplicationServices.Services
{
    public class ChuckNorrisServices : IChuckNorrisServices
    {

        private readonly HttpClient _httpClient;

        public ChuckNorrisServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<ChuckNorrisRootDto> ChuckNorrisResultHttpClient()
        {
            var response = await _httpClient.GetAsync("https://api.chucknorris.io/jokes/random");
            //annab veateate, kui response ei ole edukas
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                //PropertyNameCaseInsensitive = true lets you ignore exact casing of JSON property names.
                PropertyNameCaseInsensitive = true
            };

            var joke = JsonSerializer.Deserialize<ChuckNorrisRootDto>(json, options);
            //The null-forgiving operator (!) is safe here since API always returns JSON.
            return joke!;
        }

        public async Task<ChuckNorrisResultDto> ChuckNorrisResult(ChuckNorrisResultDto dto)
        {
            var url = "https://api.chucknorris.io/jokes/random";

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);
                ChuckNorrisRootDto chuckNorrisResult = new JavaScriptSerializer().Deserialize<ChuckNorrisRootDto>(json);

                //dto.Categories = chuckNorrisResult.Categories[0];
                dto.CreatedAt = chuckNorrisResult.CreatedAt;
                dto.IconUrl = chuckNorrisResult.IconUrl;
                dto.Id = chuckNorrisResult.Id;
                dto.UpdatedAt = chuckNorrisResult.UpdatedAt;
                dto.Url = chuckNorrisResult.Url;
                dto.Value = chuckNorrisResult.Value;
            }

            return dto;
        }
    }
}

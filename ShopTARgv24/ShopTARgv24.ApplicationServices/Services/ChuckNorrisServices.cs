using Nancy.Json;
using ShopTARgv24.Core.Dto.ChuckNorrisDto;
using ShopTARgv24.Core.ServiceInterface;
using System.Net;


namespace ShopTARgv24.ApplicationServices.Services
{
    public class ChuckNorrisServices : IChuckNorrisServices
    {

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

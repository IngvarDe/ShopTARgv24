using ShopTARgv24.Core.Dto.ChuckNorrisDto;


namespace ShopTARgv24.Core.ServiceInterface
{
    public interface IChuckNorrisServices
    {
        Task<ChuckNorrisRootDto> ChuckNorrisResultHttpClient();
        Task<ChuckNorrisResultDto> ChuckNorrisResult(ChuckNorrisResultDto dto);
    }
}

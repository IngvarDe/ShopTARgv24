using Microsoft.AspNetCore.Mvc;
using ShopTARgv24.Core.Dto.ChuckNorrisDto;
using ShopTARgv24.Core.ServiceInterface;
using ShopTARgv24.Models.ChuckNorris;


namespace ShopTARgv24.Controllers
{
    public class ChuckNorrisController : Controller
    {
        private readonly IChuckNorrisServices _chuckNorrisServices;

        public ChuckNorrisController
            (
                IChuckNorrisServices chuckNorrisServices
            )
        {
            _chuckNorrisServices = chuckNorrisServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchChuckNorrisJokes(ChuckNorrisViewModel model)
        {
            return RedirectToAction(nameof(Joke));
        }

        [HttpGet]
        public async Task<IActionResult>Joke()
        {
            //ChuckNorrisResultDto dto = new();

            var joke = await _chuckNorrisServices.ChuckNorrisResultHttpClient();
            //_chuckNorrisServices.ChuckNorrisResult(joke);
            ChuckNorrisViewModel vm = new();

            //vm.Categories = joke.Categories;
            vm.CreatedAt = joke.CreatedAt;
            vm.IconUrl = joke.IconUrl;
            vm.Id = joke.Id;
            vm.UpdatedAt = joke.UpdatedAt;
            vm.Url = joke.Url;
            vm.Value = joke.Value;

            return View(vm);
        }
    }
}

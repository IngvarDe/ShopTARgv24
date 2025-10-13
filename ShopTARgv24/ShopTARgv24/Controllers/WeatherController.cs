﻿using Microsoft.AspNetCore.Mvc;
using ShopTARgv24.Core.ServiceInterface;

namespace ShopTARgv24.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherForecastServices _weatherForecastServices;

        public WeatherController
            (
                IWeatherForecastServices weatherForecastServices
            )
        {
            _weatherForecastServices = weatherForecastServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        //teha action SearchCity
        [HttpPost]
        public IActionResult SearchCity()
        {
            return View();
        }
    }
}

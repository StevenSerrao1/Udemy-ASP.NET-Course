using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Weather_App_1.Models;

namespace Weather_App_1.Controllers
{
    public class HomeController : Controller
    {
        // * Initialize P/RO field for DI
        private readonly IWeatherService _cities; 
        
        public HomeController(IWeatherService cities)
        {
            _cities = cities;
        }

        [Route("/")]
        public IActionResult AllCities()
        {
            // Sending viewbag assignment of cities for dynamic assignment of "AllCities.cshtml"
            var allCities = _cities.GetWeatherDetails();

            return View(allCities);
        }

        [Route("/weather/{cityCode}")]
        public IActionResult City(string? cityCode)
        {
            // Check if cityCode parameter is present
            if (string.IsNullOrEmpty(cityCode))
            {
                // Handle the case when cityCode is missing or empty
                return BadRequest("City code is required");
            }

            CityWeather? stad = _cities.GetWeatherByCityCode(cityCode);

            // Sending MODEL so that "City.cshtml" view contains strongly typed version
            return View(stad);
        }
    }
}

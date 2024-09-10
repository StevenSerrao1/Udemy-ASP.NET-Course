using Microsoft.AspNetCore.Mvc;
using Weather_App_1.Models;

namespace Weather_App_1.Controllers
{
    public class HomeController : Controller
    {
        // * Updated cities List and action methods according to DRY principle
        List<CityWeather> cities = new List<CityWeather>
        {
            new CityWeather { CityUniqueCode = "LDN", CityName = "London", DateAndTime = Convert.ToDateTime("2030-01-01 8:00"), TemperatureCelsius = 11 },
            new CityWeather { CityUniqueCode = "NYC", CityName = "New York", DateAndTime = Convert.ToDateTime("2030-01-01 3:00"), TemperatureCelsius = 26 },
            new CityWeather { CityUniqueCode = "PAR", CityName = "Paris", DateAndTime = Convert.ToDateTime("2030-01-01 9:00"), TemperatureCelsius = -1 }
        };

        [Route("/")]
        [Route("/weather")]
        public IActionResult AllCities()
        {
            // Sending viewbag assignment of cities for dynamic assignment of "AllCities.cshtml"
            ViewBag.Cities = cities;

            return View();
        }

        [Route("/weather/{cityCode}")]
        public IActionResult City(string? cityCode)
        {
            CityWeather? ciadad = cities.FirstOrDefault(c => c.CityUniqueCode == cityCode);

            // Sending MODEL so that "City.cshtml" view contains strongly typed version
            return View(ciadad);
        }
    }
}

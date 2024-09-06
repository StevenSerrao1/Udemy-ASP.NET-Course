using Microsoft.AspNetCore.Mvc;
using Weather_App_1.Models;

namespace Weather_App_1.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        [Route("/weather")]
        public IActionResult AllCities()
        {
            List<CityWeather> cities = new List<CityWeather>
            {
               new CityWeather { CityUniqueCode = "LDN", CityName = "London", DateAndTime = Convert.ToDateTime("2030-01-01 8:00"), TemperatureCelsius = 1 },
               new CityWeather { CityUniqueCode = "NYC", CityName = "New York", DateAndTime = Convert.ToDateTime("2030-01-01 3:00"), TemperatureCelsius = 16 },
               new CityWeather { CityUniqueCode = "PAR", CityName = "Paris", DateAndTime = Convert.ToDateTime("2030-01-01 9:00"), TemperatureCelsius = 28 }
            };

            // Sending viewbag assignment of cities for dynamic assignment of "AllCities.cshtml"
            ViewBag.Cities = cities;

            return View();
        }

        [Route("/weather/{cityCode}")]
        public IActionResult City(string? cityCode)
        {
            List<CityWeather> cities = new List<CityWeather>
            {
               new CityWeather { CityUniqueCode = "LDN", CityName = "London", DateAndTime = Convert.ToDateTime("2030-01-01 8:00"), TemperatureCelsius = 1 },
               new CityWeather { CityUniqueCode = "NYC", CityName = "New York", DateAndTime = Convert.ToDateTime("2030-01-01 3:00"), TemperatureCelsius = 16 },
               new CityWeather { CityUniqueCode = "PAR", CityName = "Paris", DateAndTime = Convert.ToDateTime("2030-01-01 9:00"), TemperatureCelsius = 28 }
            };

            CityWeather? ciadad = cities.FirstOrDefault(c => c.CityUniqueCode == cityCode);

            // Sending MODEL so that "City.cshtml" view contains strongly typed version
            return View(ciadad);
        }
    }
}

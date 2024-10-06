using Microsoft.AspNetCore.Mvc;
using Weather_App_1.Models;

namespace Weather_App_1.ViewComponents
{
    public class CityWeatherViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(CityWeather city)
        {
            CityWeatherViewModel viewModel = new CityWeatherViewModel()
            {
                CityWeather = city,
                TemperatureClass = GetTemperatureClass(city.TemperatureCelsius)
            };

            return View(viewModel);
        }

        string GetTemperatureClass(int temp)
        {
            if (temp <= 7) return "blue-back";
            else if (temp <= 23) return "green-back";
            else return "orange-back";
        }

    }
}

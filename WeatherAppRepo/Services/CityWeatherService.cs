using Interfaces;
using Weather_App_1.Models;

namespace Services
{
    public class CityWeatherService : IWeatherService
    {
        private readonly List<CityWeather> _cities;

        public CityWeatherService()
        {
            _cities = new List<CityWeather>
             {
                new CityWeather { CityUniqueCode = "LDN", CityName = "London", DateAndTime = Convert.ToDateTime("2030-01-01 8:00"), TemperatureCelsius = 11 },
                new CityWeather { CityUniqueCode = "NYC", CityName = "New York", DateAndTime = Convert.ToDateTime("2030-01-01 3:00"), TemperatureCelsius = 26 },
                new CityWeather { CityUniqueCode = "PAR", CityName = "Paris", DateAndTime = Convert.ToDateTime("2030-01-01 9:00"), TemperatureCelsius = -1 }
             };
        }

        public CityWeather? GetWeatherByCityCode(string CityCode)
        {
            CityWeather? ciadad = _cities.FirstOrDefault(c => c.CityUniqueCode == CityCode);

            // Check if cityCode corresponds to a valid city
            if (ciadad == null)
            {
                // Handle the case when cityCode is invalid
                throw new Exception("Invalid city code");
            }

            return ciadad;
        }

        public List<CityWeather> GetWeatherDetails()
        {
            return _cities;
        }
    }
}

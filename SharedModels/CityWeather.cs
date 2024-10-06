// the namespace was not updated to reflect the new location, this due to the pros outweighing th cons
// of simply NOT doing so
namespace Weather_App_1.Models
{
    public class CityWeather
    {
        public string? CityUniqueCode { get; set; }
        public string? CityName { get; set; }
        public DateTime DateAndTime { get; set; }
        public int TemperatureCelsius { get; set; }

    }
}

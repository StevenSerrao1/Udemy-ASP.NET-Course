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

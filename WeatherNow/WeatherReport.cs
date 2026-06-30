
namespace WeatherNow
{
    internal class WeatherReport
    {
        public string City { get; set; }
        public double Temperature { get; set; }
        public string Condition { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public int RainChance { get; set; }

        //Constructor for WeatherReport class
        public WeatherReport(string city, double temperature, string condition, int humidity, double windSpeed, int rainChance)
        {
            City = city;
            Temperature = temperature;
            Condition = condition;
            Humidity = humidity;
            WindSpeed = windSpeed;
            RainChance = rainChance;
        }


    }

   
}

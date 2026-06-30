//Importing libraries
using System.Runtime.InteropServices;
using WeatherNow;
using System.Net.Http;
using System.Text.Json;


Console.WriteLine("Welcome to WeatherNow");

// Fetching weather data from the Open-Meteo API
HttpClient client = new HttpClient();



while (true)
{

    Console.Write("\nEnter a City (Or type 1 to exit): ");

    string city = Console.ReadLine().Trim().ToLower();
    //Exit Condition
    if (city == "1")
    {
        Console.WriteLine("Exiting the application. Goodbye!");
        break;
    }
    //Check for Blank Entries
    if (city == "")
    {
        Console.WriteLine("Please Enter a City Name");
        return;
    }

    WeatherReport report = await CreateWeatherReport(city, client);
    



    // Call the PrintWeather function to display the weather report
    PrintWeather(report);


    // Function to Print Weather Report
    static void PrintWeather(WeatherReport report)
    {
        Console.WriteLine("\n WeatherNow Report");
        Console.WriteLine("-----------------");
        Console.WriteLine("City: " + report.City.ToUpper());
        Console.WriteLine("Temperature: " + report.Temperature + "°F");
        Console.WriteLine("Condition: " + report.Condition);
        Console.WriteLine("Humidity: " + report.Humidity + "%");
        Console.WriteLine("Wind Speed: " + report.WindSpeed + " mph");
        Console.WriteLine("Chance of Rain Today: " + report.RainChance + "%");
        Console.WriteLine("-----------------");
    }
    

    

    static async Task<WeatherReport> CreateWeatherReport(string city, HttpClient client)
    {
        Coordinates coordinates = await GetCoordinates(city, client);

        // Fetching weather data for the coordinates
        string weatherUrl = "https://api.open-meteo.com/v1/forecast?latitude="
            + coordinates.Latitude
            + "&longitude="
            + coordinates.Longitude
            + "&current=temperature_2m,relative_humidity_2m,weather_code,wind_speed_10m"
            + "&daily=precipitation_probability_max"
            + "&temperature_unit=fahrenheit"
            + "&wind_speed_unit=mph"
            + "&forecast_days=1";
        string weatherResponse = await client.GetStringAsync(weatherUrl);

        // Parse the JSON response
        JsonDocument weatherData = JsonDocument.Parse(weatherResponse);
        JsonElement currentWeather = weatherData.RootElement.GetProperty("current");
        JsonElement dailyWeather = weatherData.RootElement.GetProperty("daily");

        // Extract the temperature from the JSON 
        double temperature = currentWeather.GetProperty("temperature_2m").GetDouble();
        int humidity = currentWeather.GetProperty("relative_humidity_2m").GetInt32();
        int weatherCode = currentWeather.GetProperty("weather_code").GetInt32();
        double windSpeed = currentWeather.GetProperty("wind_speed_10m").GetDouble();

        int rainChance = dailyWeather.GetProperty("precipitation_probability_max")[0].GetInt32();

        return new WeatherReport(city, temperature, GetConditionFromCode(weatherCode,temperature), humidity, windSpeed, rainChance);
    }

    static async Task<Coordinates> GetCoordinates(string city, HttpClient client)
    {
        // Fetching weather data for Phoenix, Arizona
        string geoResponse = await client.GetStringAsync("https://geocoding-api.open-meteo.com/v1/search?name=" + city + "&count=10");

        // Parsing the JSON response to extract latitude and longitude
        JsonDocument geoData = JsonDocument.Parse(geoResponse);
        JsonElement results = geoData.RootElement.GetProperty("results");
        JsonElement firstResult = results[0];


        // Extracting the temperature from the JSON response
        double latitude = firstResult.GetProperty("latitude").GetDouble();
        double longitude = firstResult.GetProperty("longitude").GetDouble();

        return new Coordinates(latitude, longitude);
    }

    static string GetConditionFromCode(int weatherCode, double temperature)
    {
        if (weatherCode ==0 && temperature >=85)
        {
            return "Sunny";
        }
        else if (weatherCode == 0)
        {
            return "Clear";
        }
        else if (weatherCode == 1 || weatherCode == 2 || weatherCode == 3)
        {
            return "Partly Cloudy";
        }
        else if (weatherCode == 45 || weatherCode == 48)
        {
            return "Foggy";
        }
        else if (weatherCode >= 51 && weatherCode <= 67)
        {
            return "Rainy";
        }
        else if (weatherCode >= 71 && weatherCode <= 77)
        {
            return "Snowy";
        }
        else if (weatherCode >= 80 && weatherCode <= 82)
        {
            return "Rain Showers";
        }
        else if (weatherCode >= 95)
        {
            return "Thunderstorm";
        }
        else
        {
            return "Unknown";
        }
    }

}

  



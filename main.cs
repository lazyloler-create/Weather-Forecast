using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

class Program
{
    static async Task GetCurrentWeather(HttpClient client, string url, string city)
    {
        try
        {
            var response =              await client.GetStringAsync(url);
            var wData = JsonDocument.Parse(response);
            double temp = wData.RootElement.GetProperty("main").GetProperty("temp").GetDouble();
            Console.WriteLine($"Current temperature in {city} is {temp}°C");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error fetching weather data: {ex.Message}");
        }
    }

    static async Task Get5HourForecast(HttpClient client, string city, string apiKey)
    {
        try
        {
            int count = 0;
            string forecastUrl = $"https://api.openweathermap.org/data/2.5/forecast?q={city}&appid={apiKey}&units=metric";
            var forecastResponse = await client.GetStringAsync(forecastUrl);
            var fData = JsonDocument.Parse(forecastResponse);
            Console.WriteLine($"\n 5-hour forecast for {city}:");
            foreach (var item in fData.RootElement.GetProperty("list").EnumerateArray())
            {
                if (count >= 5) break;
                string dateTime = item.GetProperty("dt_txt").GetString() ?? "";
                string time = dateTime.Split(' ')[1]; //this is to extract the time from the date and time
                double temp = item.GetProperty("main").GetProperty("temp").GetDouble();
                Console.WriteLine($"{time}: {temp}°C");
                count++;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching weather forecast: {ex.Message}");
        }
    }

    static async Task Get5DayForecast(HttpClient client, string city, string apiKey)
    {
        try
        {
            string forecastUrl = $"https://api.openweathermap.org/data/2.5/forecast?q={city}&appid={apiKey}&units=metric";
            var forecastResponse = await client.GetStringAsync(forecastUrl);
            var fData = JsonDocument.Parse(forecastResponse);
            
            Console.WriteLine($"\n5-Day forecast for {city}:");
            
            string currentDay = "";
            int dayCount = 0;
            
            foreach (var item in fData.RootElement.GetProperty("list").EnumerateArray())
            {
                if (dayCount >= 5) break;
                
                string dateTime = item.GetProperty("dt_txt").GetString() ?? "";
                string date = dateTime.Split(' ')[0];

                if (date != currentDay)
                {
                    Console.WriteLine($"\n--- {date} ---");
                    currentDay = date;
                    dayCount++;
                }
                string time = dateTime.Split(' ')[1];
                double temp = item.GetProperty("main").GetProperty("temp").GetDouble();
                Console.WriteLine($"  {time}: {temp}°C");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching weather forecast: {ex.Message}");
        }
    }
    static async Task Main()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        string apiKey = config["OpenWeatherApiKey"] ?? "";
        if (string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("Error: API key not found in appsettings.json");
            return;
        }
        
        Console.WriteLine("═══════════════════════════════════════════════════════");
        Console.WriteLine("\t\t★ Weather checker ★");
        Console.WriteLine("═══════════════════════════════════════════════════════");

        Console.WriteLine("Options: ");
        Console.WriteLine("1. Get current weather");
        Console.WriteLine("2. Get 5-hour forecast");
        Console.WriteLine("3. Get 5-day forecast");

        Console.WriteLine("Enter your choice (1, 2, or 3):");
        int choice = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter a city name:");
        string? city = Console.ReadLine();
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";
        using HttpClient client = new HttpClient();

        switch(choice)
        {
            case 1:
                await GetCurrentWeather(client, url, city);
                break;
            case 2:
                await Get5HourForecast(client, city, apiKey);
                break;
            case 3:
                await Get5DayForecast(client, city, apiKey);        
                break;
        }      
    }
}
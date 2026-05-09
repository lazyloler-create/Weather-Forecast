# Weather Fetcher

A C# console application that fetches weather data from OpenWeatherMap API.

## Features
- Get current weather for any city
- 5-hour weather forecast
- 5-day weather forecast

## Setup

1. **Get an API Key**
   - Sign up at [OpenWeatherMap](https://openweathermap.org/)
   - Get your free API key

2. **Configure the Application**
   - Copy `appsettings.example.json` to `appsettings.json`
   - Replace `YOUR_API_KEY_HERE` with your actual API key

3. **Run the Application**
   ```bash
   dotnet run main.cs
   ```

## Usage
- Select option 1, 2, or 3
- Enter a city name
- View the weather forecast

## Security
⚠️ **Never commit `appsettings.json` to version control** - it contains your API key!
The file is listed in `.gitignore` to prevent accidental commits.

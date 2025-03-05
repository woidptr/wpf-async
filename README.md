# WPF Async Programming Guide

## Introduction
This project demonstrates how to use asynchronous programming (`async`/`await`) in a WPF (Windows Presentation Foundation) application to ensure the UI remains responsive while executing long-running operations such as fetching data from an external API.

---

## Theoretical Background: Async/Await in WPF

### Why Async/Await is Important in WPF
- **Prevents UI Freezing:** WPF applications run on a single UI thread. If a long-running operation (like fetching weather data) blocks this thread, the entire UI becomes unresponsive.
- **Enhances User Experience:** By using `async`/`await`, operations can run in the background, allowing users to interact with the application while waiting for data.
- **Automatic Context Switching:** The `await` keyword allows the UI thread to continue processing while the awaited task runs. Once completed, the result is returned to the UI thread automatically.

### How Async/Await Works
- `async` is used to define an asynchronous method.
- `await` pauses execution of an async method until the awaited task completes, allowing the UI thread to remain free.
- `Task.Run()` can be used to offload CPU-bound operations to a background thread.
- `ConfigureAwait(false)` is used in library code to avoid unnecessary UI thread capturing.

### Common Pitfalls
- **Blocking Calls:** Calling `.Result` or `.Wait()` on an async method will block the UI thread and may lead to deadlocks.
- **Using `async void` Incorrectly:** `async void` should only be used for event handlers, as it does not return a `Task` and prevents robust error handling.
- **Updating UI from Background Threads:** UI elements can only be updated from the main thread. Always ensure UI updates happen on the UI thread (which `await` does automatically unless overridden).

---

## How This Project Implements Async in WPF

### Project Overview
This project is a simple weather dashboard that allows users to input a city name and fetch weather data asynchronously from the OpenWeatherMap API.  

### Step-by-Step Code Explanation

#### 1. **Application Entry Point** (Files: `App.xaml` / `App.xaml.cs`)
- `App.xaml` defines the application-wide settings and the startup window (`MainWindow.xaml`).
- `App.xaml.cs` contains the partial class `App`, which inherits from `Application` and serves as the entry point for the application.

#### 2. **Main Window UI** (File: `MainWindow.xaml`)
Defines the UI components:
- **TextBox (`CityName`)** – User input field for entering the city name.
- **Button (`Search_Click`)** – Triggers the weather API request.
- **TextBlocks (`CityNameText`, `TemperatureText`, `FeelsLikeText`, `HumidityText`)** – Displays weather information.

#### 3. **Main Window Logic** (File: `MainWindow.xaml.cs`)
Handles user interaction and async data retrieval:

```csharp
private async void Search_Click(object sender, RoutedEventArgs e)
{
    string city = CityName.Text;
    try
    {
        WeatherModel weather = await WeatherService.GetWeatherInfo(city);

        CityNameText.Text = city;
        TemperatureText.Text = string.Format("Temperature: {0:N2}", weather.main.temp);
        FeelsLikeText.Text = string.Format("Feels like: {0}", weather.main.feelsLike);
        HumidityText.Text = string.Format("Humidity: {0}", weather.main.humidity);
    }
    catch (Exception ex)
    {
        Trace.WriteLine("Error fetching weather data!");
    }
}
```

- **`await WeatherService.GetWeatherInfo(city);`** – Calls an async method to fetch weather data.
- **Error Handling** – Uses `try/catch` to prevent crashes if the API request fails.
- **UI Updates** – Happens safely on the UI thread after the async call completes.

#### 4. **Weather Data Fetching** (File: `Services/WeatherService.cs`)
This class makes an asynchronous API request to fetch weather data:

```csharp
public static class WeatherService
{
    private const string API_KEY = "YOUR_API_KEY";
    private const string baseUrl = "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric";

    public static async Task<WeatherModel> GetWeatherInfo(string city)
    {
        string url = string.Format(baseUrl, city, API_KEY);
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to fetch information");
        }

        string jsonResponse = await response.Content.ReadAsStringAsync();
        WeatherModel weather = JsonSerializer.Deserialize<WeatherModel>(jsonResponse);

        return weather;
    }
}
```

- **Asynchronous HTTP Request** – Uses `await client.GetAsync(url)` to avoid blocking the UI thread.
- **Exception Handling** – Checks if the response is successful; otherwise, it throws an exception.
- **Asynchronous Read** – `await response.Content.ReadAsStringAsync()` reads the response without blocking.
- **Deserialization** – `JsonSerializer.Deserialize<WeatherModel>(jsonResponse)` converts JSON into C# objects.

#### 5. **Data Models** (Folder: `Models`, File: `WeatherModel.cs`)
These classes represent the JSON response from the API. `WeatherModel` replaces the previous `WeatherStruct`:

```csharp
namespace MyApp.Models
{
    public class WeatherModel
    {
        public WeatherMain main { get; set; }
    }

    public class WeatherMain
    {
        public float temp { get; set; }
        [JsonPropertyName("feels_like")]
        public float feelsLike { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
    }
}
```

- Maps API response properties to C# objects.
- Uses `[JsonPropertyName(\"feels_like\")]` to map JSON names to .NET property names.
- Notice these are now **classes** (not structs) called **`WeatherModel`** and **`WeatherMain`**.

#### 6. **Project Configuration** (Files: `WeatherDashboard.csproj`, `WeatherDashboard.sln`)
- `WeatherDashboard.csproj` – Defines build settings, dependencies, and the target framework.
- `WeatherDashboard.sln` – The Visual Studio solution file that ties everything together.

---

## Running the Project

### Requirements
- **Visual Studio (not VS Code)** with the `.NET Desktop Development` workload installed.
- .NET SDK installed on your machine.

### Steps to Run
1. **Clone the repository** (if using Git):
   ```bash
   git clone <repository-url>
   ```
2. **Open the project in Visual Studio**:
   - Open `WeatherDashboard.sln`.
3. **Set the Startup Project**:
   - In the Solution Explorer, right-click `WeatherDashboard` → `Set as Startup Project`.
4. **Build and Run**:
   - Press **F5** or click **Start** to run the project in Debug mode.
5. **Enter a City Name** and **Click Search**:
   - The application will fetch weather data asynchronously and update the UI.

---

## Conclusion
This project illustrates how `async`/`await` can keep a WPF application responsive while performing long-running operations such as fetching data from a web API. By following best practices (avoiding blocking calls, handling exceptions, and structuring async workflows properly), developers can create smooth and user-friendly WPF applications.

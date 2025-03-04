using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeatherDashboard.Models;
using WeatherDashboard.Services;

namespace WeatherDashboard;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly WeatherService _weatherService;

    public MainWindow()
    {
        InitializeComponent();

        this._weatherService = new WeatherService();
    }

    // cpu-bound
    private void UpdateSunPosition(long sunriseUnix, long sunsetUnix, long currentUnix)
    {
        bool isDaytime = currentUnix > sunriseUnix && currentUnix < sunsetUnix;

        if (!isDaytime)
        {
            DayLine.Stroke = new SolidColorBrush(Color.FromRgb(170, 170, 200));
            Sun.Visibility = Visibility.Collapsed;
        }

        // Ensure current time is within bounds
        currentUnix = Math.Max(sunriseUnix, Math.Min(currentUnix, sunsetUnix));

        // Calculate progress of the sun across the day
        double progress = (double)(currentUnix - sunriseUnix) / (sunsetUnix - sunriseUnix);

        // Define the X-axis range
        double startX = 0;  // Start of the line
        double endX = 300;   // End of the line
        double sunX = startX + progress * (endX - startX);

        // Position the sun
        Canvas.SetLeft(Sun, sunX - Sun.Width / 2);
        Canvas.SetTop(Sun, 100 - Sun.Height / 2); // Keep Y constant
    }

    private async void Search_Click(object sender, RoutedEventArgs e)
    {
        string city = CityName.Text;

        try
        {
            WeatherModel weather = await _weatherService.GetWeatherInfo(city);

            CityNameText.Text = weather.name;
            TemperatureText.Text = string.Format("Temperature: {0:N2}", weather.main.temp);
            FeelsLikeText.Text = $"Feels like: {weather.main.feelsLike}";
            HumidityText.Text = $"Humidity: {weather.main.humidity}";

            UpdateSunPosition(weather.sys.sunrise, weather.sys.sunset, weather.datetime);

            string currentTime = _weatherService.ConvertTime(weather.datetime, weather.timezoneOffset);
            CurrentTimeText.Text = $"Current time in {weather.name}: {currentTime}";

            SunCanvas.Visibility = Visibility.Visible;

            string sunriseTime = _weatherService.ConvertTime(weather.sys.sunrise, weather.timezoneOffset);
            SunriseText.Text = sunriseTime;

            string sunsetTime = _weatherService.ConvertTime(weather.sys.sunset, weather.timezoneOffset);
            SunsetText.Text = sunsetTime;
        }
        catch (Exception ex)
        {
            Trace.WriteLine("Error!");
        }
    }
}
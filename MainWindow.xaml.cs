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

    private async void Search_Click(object sender, RoutedEventArgs e)
    {
        string city = CityName.Text;

        try
        {
            WeatherModel weather = await _weatherService.GetWeatherInfo(city);

            CityNameText.Text = city;
            TemperatureText.Text = string.Format("Temperature: {0:N2}", weather.main.temp);
            FeelsLikeText.Text = $"Feels like: {weather.main.feelsLike}";
            HumidityText.Text = $"Humidity: {weather.main.humidity}";

            string currentTime = _weatherService.ConvertTime(weather.datetime, weather.timezoneOffset);
            CurrentTimeText.Text = $"Current time in {weather.name}: {currentTime}";

            string sunriseTime = _weatherService.ConvertTime(weather.sys.sunrise, weather.timezoneOffset);
            SunriseTimeText.Text = $"Sunrise time in {weather.name}: {sunriseTime}";

            string sunsetTime = _weatherService.ConvertTime(weather.sys.sunset, weather.timezoneOffset);
            SunsetTimeText.Text = $"Sunset time in {weather.name}: {sunsetTime}";
        }
        catch (Exception ex)
        {
            Trace.WriteLine("Error!");
        }
    }
}
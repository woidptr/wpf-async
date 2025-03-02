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

namespace WeatherDashboard;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void Search_Click(object sender, RoutedEventArgs e)
    {
        string city = CityName.Text;

        try
        {
            WeatherStruct weather = await WeatherHandler.GetWeatherInfo(city);

            CityNameText.Text = city;
            TemperatureText.Text = string.Format("Temperature: {0:N2}", weather.main.temp);
            FeelsLikeText.Text = string.Format("Feels like: {0}", weather.main.feelsLike);
            HumidityText.Text = string.Format("Humidity: {0}", weather.main.humidity);

            string currentTime = WeatherHandler.ConvertTime(weather.datetime, weather.timezoneOffset);
            CurrentTimeText.Text = string.Format("Current time in {0}: {1}", weather.name, currentTime);

            string sunriseTime = WeatherHandler.ConvertTime(weather.sys.sunrise, weather.timezoneOffset);
            SunriseTimeText.Text = string.Format("Sunrise time in {0}: {1}", weather.name, sunriseTime);

            string sunsetTime = WeatherHandler.ConvertTime(weather.sys.sunset, weather.timezoneOffset);
            SunsetTimeText.Text = string.Format("Sunset time in {0}: {1}", weather.name, sunsetTime);
        }
        catch (Exception ex)
        {
            Trace.WriteLine("Error!");
        }
    }
}
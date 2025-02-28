using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
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

        WeatherStruct weather = await WeatherHandler.GetWeatherInfo(city);

        TemperatureText.Text = string.Format("Temperature: {0:N2}", weather.main.temp);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json.Linq;

namespace CidadesSC
{
    
    public partial class MainWindow : Window
    {
        private const string ApiKey = "1e589e8ae37ee0c563091009690cc6cf";
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void GetWeatherButton_Click(object sender, RoutedEventArgs e)
        {
            string cityName = CityTextBox.Text.Trim();
            if(string.IsNullOrEmpty(cityName) )
            {
                MessageBox.Show("Digite o nome da cidade");
                return;
            }
            string apiUrl = $"http://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={ApiKey}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        JObject weatherData = JObject.Parse(content);
                        string description = weatherData["weather"][0]["description"].ToString();
                        double temperature = Convert.ToDouble(weatherData["main"]["temp"]);

                        double temperatureKelvin = Convert.ToDouble(weatherData["main"]["temp"]);
                        double temperatureCelsius = temperatureKelvin - 273.15;

                        string resultText = $"Clima em {cityName}: {description}\nTemperatura: {temperatureCelsius:F1}°C";
                        WeatherTextBlock.Text = resultText;
                    }
                    else
                    {
                        MessageBox.Show("Não foi possível obter o clima da cidade.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro: " + ex.Message);
                }
            }
        }
        private void CityTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            CityTextBox.Text = ""; // Limpar o texto de dica quando o TextBox receber foco
        }

    }
}
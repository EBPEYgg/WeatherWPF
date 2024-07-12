using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net;
using WeatherWPF.Model;

namespace WeatherWPF.ViewModel
{
    /// <summary>
    /// Класс, описывающий ViewModel для главного окна.
    /// </summary>
    public partial class MainVM : ObservableObject
    {
        /// <summary>
        /// API key.
        /// </summary>
        private const string _apiKey = "9fbca1cd22b74a98808145032241207";

        /// <summary>
        /// Текущее время.
        /// </summary>
        [ObservableProperty]
        private DateTime _dateTimeNow;

        /// <summary>
        /// Название города.
        /// </summary>
        [ObservableProperty]
        private string _cityName;

        /// <summary>
        /// Скорость ветра.
        /// </summary>
        [ObservableProperty]
        private double _windSpeed;

        /// <summary>
        /// Влажность.
        /// </summary>
        [ObservableProperty]
        private double _humidity;

        /// <summary>
        /// Возвращает и задает команду для выхода из приложения.
        /// </summary>
        public RelayCommand QuitCommand { get; set; }

        /// <summary>
        /// Конструктор класса <see cref="MainVM"/>.
        /// </summary>
        public MainVM()
        {
            QuitCommand = new RelayCommand(Quit);
            DateTimeNow = DateTime.Now;
        }

        /// <inheritdoc cref="Application.Shutdown()"/>
        private void Quit()
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Метод, который получает JSON файл с погодой
        /// при помощи запроса на сервер.
        /// </summary>
        private async void GetWeather()
        {
            if (string.IsNullOrEmpty(CityName))
            {

                return;
            }

            string apiUrl = $"http://api.weatherapi.com/v1/current.json?key={_apiKey}&q={CityName}";

            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(apiUrl);
                request.Method = "GET";

                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string jsonResponse = reader.ReadToEnd();
                            //WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(jsonResponse);
                            //DisplayWeatherData(weatherData);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                return;
            }
        }

        private void DisplayWeatherData(WeatherData weatherData)
        {
            CityName = weatherData.Location.Name;
            WindSpeed = weatherData.CurrentWeather.WindKph;
            Humidity = weatherData.CurrentWeather.Humidity;
        }
    }
}
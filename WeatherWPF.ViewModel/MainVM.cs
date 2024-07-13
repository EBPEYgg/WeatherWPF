using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherWPF.Model;

namespace WeatherWPF.ViewModel
{
    /// <summary>
    /// Класс, описывающий ViewModel для главного окна.
    /// </summary>
    public partial class MainVM : ObservableObject
    {
        /// <summary>
        /// API ключ сервиса погоды.
        /// </summary>
        private const string _apiKeyWeather = "9fbca1cd22b74a98808145032241207";

        /// <summary>
        /// API ключ сервиса, предоставляющий информацию 
        /// о времени и дате для различных часовых поясов.
        /// </summary>
        private const string _apiKeyTimeZoneDB = "CUHLWKFUKN7W";

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
        /// Западная долгота города.
        /// </summary>
        private string longitude;

        /// <summary>
        /// Северная широта города.
        /// </summary>
        private string latitude;

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
        /// Преобразованное значение влажности для слайдера (от 0 до 10).
        /// </summary>
        [ObservableProperty]
        private double _humiditySlider;

        /// <summary>
        /// Количество градусов.
        /// </summary>
        [ObservableProperty]
        private string _tempC;

        /// <summary>
        /// Облачность.
        /// </summary>
        [ObservableProperty]
        private string _conditionText;

        /// <summary>
        /// Изображение облачности.
        /// </summary>
        [ObservableProperty]
        private string _conditionIcon;

        /// <summary>
        /// Возвращает и задает команду для выхода из приложения.
        /// </summary>
        public RelayCommand QuitCommand { get; set; }

        public RelayCommand GetWeatherCommand { get; set; }

        public RelayCommand ConvertToFahrenheitCommand { get; set; }

        /// <summary>
        /// Конструктор класса <see cref="MainVM"/>.
        /// </summary>
        public MainVM()
        {
            QuitCommand = new RelayCommand(Quit);
            GetWeatherCommand = new RelayCommand(GetWeather);
            CityName = "London";
            GetWeatherCommand.Execute(this);
        }

        /// <inheritdoc cref="Environment.Exit(int)"/>
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
                Console.WriteLine($"{CityName} was not found.");
            }

            string apiUrl = $"http://api.weatherapi.com/v1/current.json?key={_apiKeyWeather}&q={CityName}";

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
                            WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(jsonResponse);
                            DisplayWeatherData(weatherData);
                            //GetTimeFromCoordinatesCommand.Execute(this);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Метод, который отображает полученные данные о погоде на View.
        /// </summary>
        /// <param name="weatherData">Данные о погоде.</param>
        private void DisplayWeatherData(WeatherData weatherData)
        {
            TempC = Convert.ToInt32(weatherData.CurrentWeather.TempC) + "°c";
            WindSpeed = weatherData.CurrentWeather.WindKph;
            Humidity = weatherData.CurrentWeather.Humidity;
            HumiditySlider = Humidity / 10;
            ConditionText = weatherData.CurrentWeather.Condition.Text;
            ConditionIcon = "https:" + weatherData.CurrentWeather.Condition.Icon;

            longitude = weatherData.Location.Lon.ToString("0.00", CultureInfo.InvariantCulture);
            latitude = weatherData.Location.Lat.ToString("0.00", CultureInfo.InvariantCulture);
            GetTimeFromCoordinates(longitude, latitude);
        }

        /// <summary>
        /// Метод, который получает JSON файл с временем и датой 
        /// для указанного города при помощи запроса на сервер.
        /// </summary>
        private async void GetTimeFromCoordinates(string longitude, string latitude)
        {
            if (string.IsNullOrEmpty(CityName))
            {
                Console.WriteLine($"{CityName} was not found.");
            }

            string apiUrl = $"http://api.timezonedb.com/v2.1/get-time-zone?key={_apiKeyTimeZoneDB}&format=json&by=position&lat={latitude}&lng={longitude}";

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
                            string jsonResponse = await reader.ReadToEndAsync();
                            Model.TimeZoneInfo timeZoneInfo = JsonConvert.DeserializeObject<Model.TimeZoneInfo>(jsonResponse);
                            UpdateTimeZoneInfo(timeZoneInfo);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void UpdateTimeZoneInfo(Model.TimeZoneInfo timeZoneInfo)
        {
            if (DateTime.TryParse(timeZoneInfo.Formatted, out DateTime dateTime))
            {
                DateTimeNow = dateTime;
            }
        }
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.Timers;
using WeatherWPF.Model;

namespace WeatherWPF.ViewModel
{
    /// <summary>
    /// Класс, описывающий ViewModel для главного окна.
    /// </summary>
    public partial class MainVM : ObservableObject
    {
        #region API
        /// <summary>
        /// API ключ сервиса погоды.
        /// </summary>
        private const string _apiKeyWeather = "9fbca1cd22b74a98808145032241207";

        /// <summary>
        /// API ключ сервиса, предоставляющий информацию 
        /// о времени и дате для различных часовых поясов.
        /// </summary>
        private const string _apiKeyTimeZoneDB = "CUHLWKFUKN7W";
        #endregion

        #region Fields
        /// <summary>
        /// Черный цвет.
        /// </summary>
        private string _blackColor = "#1A1A1A";

        /// <summary>
        /// Белый цвет.
        /// </summary>
        private string _whiteColor = "#FFFFFF";

        /// <summary>
        /// Западная долгота города.
        /// </summary>
        private string longitude;

        /// <summary>
        /// Северная широта города.
        /// </summary>
        private string latitude;

        /// <summary>
        /// Количество градусов по Цельсию.
        /// </summary>
        private double _tempC;

        /// <summary>
        /// Количество градусов по Фаренгейту.
        /// </summary>
        private double _tempF;

        /// <summary>
        /// Количество градусов утром в 6 часов по Фаренгейту.
        /// </summary>
        private double _morningTempF;

        /// <summary>
        /// Количество градусов днем в 12 часов по Фаренгейту.
        /// </summary>
        private double _dayTempF;

        /// <summary>
        /// Количество градусов вечером в 18 часов по Фаренгейту.
        /// </summary>
        private double _eveningTempF;

        /// <summary>
        /// Количество градусов ночью в 0 часов по Фаренгейту.
        /// </summary>
        private double _nightTempF;

        /// <summary>
        /// Количество градусов утром в 6 часов по Цельсию.
        /// </summary>
        private double _morningTempC;

        /// <summary>
        /// Количество градусов днем в 12 часов по Цельсию.
        /// </summary>
        private double _dayTempC;

        /// <summary>
        /// Количество градусов вечером в 18 часов по Цельсию.
        /// </summary>
        private double _eveningTempC;

        /// <summary>
        /// Количество градусов ночью в 0 часов по Цельсию.
        /// </summary>
        private double _nightTempC;

        /// <summary>
        /// Таймер.
        /// </summary>
        private System.Timers.Timer _timer;
        #endregion

        #region Flags
        /// <summary>
        /// Флаг. Температура по Цельсию.
        /// </summary>
        private bool _isTempInCelsius = true;

        /// <summary>
        /// Флаг. Виджет прогноза погоды на неделю.
        /// </summary>
        private bool _isWeekForecast = true;
        #endregion

        #region Observable Property
        /// <summary>
        /// Текущее время в формате YYYY-MM-DD hh:mm:ss.
        /// </summary>
        [ObservableProperty]
        private string _dateTimeNow;

        /// <summary>
        /// Полное название локации.
        /// </summary>
        [ObservableProperty]
        private string _location;

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
        /// Направление ветра.
        /// </summary>
        [ObservableProperty]
        private string _windDir;

        /// <summary>
        /// Иконка направления ветра.
        /// </summary>
        [ObservableProperty]
        private int _windDirectionAngle;

        /// <summary>
        /// Влажность в процентах.
        /// </summary>
        [ObservableProperty]
        private double _humidity;

        /// <summary>
        /// Преобразованное значение влажности для слайдера (от 0 до 10).
        /// </summary>
        [ObservableProperty]
        private double _humiditySlider;

        /// <summary>
        /// Средняя видимость в километрах.
        /// </summary>
        [ObservableProperty]
        private double _visibilityKm;

        /// <summary>
        /// Преобразованное значение средней видимости для слайдера (от 0 до 10).
        /// </summary>
        [ObservableProperty]
        private double _visibilityKmSlider;

        /// <summary>
        /// УФ-индекс.
        /// </summary>
        [ObservableProperty]
        private string _uvIndex;

        /// <summary>
        /// Вероятность того, что сегодня пойдет дождь.
        /// </summary>
        [ObservableProperty]
        private string _chanceOfRainToday;

        /// <summary>
        /// Количество градусов.
        /// </summary>
        [ObservableProperty]
        private string _temp;

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
        /// Изображение облачности утром в 6 часов.
        /// </summary>
        [ObservableProperty]
        private string _conditionMorningIcon;

        /// <summary>
        /// Изображение облачности днем в 12 часов.
        /// </summary>
        [ObservableProperty]
        private string _conditionDayIcon;

        /// <summary>
        /// Изображение облачности вечером в 6 часов.
        /// </summary>
        [ObservableProperty]
        private string _conditionEveningIcon;

        /// <summary>
        /// Изображение облачности ночью в 12 часов.
        /// </summary>
        [ObservableProperty]
        private string _conditionNightIcon;

        /// <summary>
        /// Индекс качества воздуха по стандарту US - EPA.
        /// </summary>
        [ObservableProperty]
        private int _usEpaIndex;

        /// <summary>
        /// Словесная оценка качества воздуха по стандарту US - EPA.
        /// </summary>
        [ObservableProperty]
        private string _airQualityAssessment;

        /// <summary>
        /// Преобразованное значение индекса качества воздуха для слайдера (от 1 до 10).
        /// </summary>
        [ObservableProperty]
        private double _usEpaIndexSlider;

        [ObservableProperty]
        /// <summary>
        /// Цвет фона кнопки температуры по Цельсию.
        /// </summary>
        private string _tempCelsiusButtonBackground = "#1A1A1A";

        [ObservableProperty]
        /// <summary>
        /// Цвет текста на кнопке температуры по Цельсию.
        /// </summary>
        private string _tempCelsiusButtonForeground = "White";

        /// <summary>
        /// Цвет фона кнопки температуры по Фаренгейту.
        /// </summary>
        [ObservableProperty]
        private string _tempFahrenheitButtonBackground = "White";

        /// <summary>
        /// Цвет текста на кнопке температуры по Фаренгейту.
        /// </summary>
        [ObservableProperty]
        private string _tempFahrenheitButtonForeground = "#1A1A1A";

        /// <summary>
        /// Видимость раздела с прогнозом погоды на неделю.
        /// </summary>
        [ObservableProperty]
        private bool _weekWeatherVisibility = true;

        /// <summary>
        /// Видимость раздела с прогнозом погоды на день.
        /// </summary>
        [ObservableProperty]
        private bool _todayWeatherVisibility = false;

        /// <summary>
        /// Температура утром в 6 часов.
        /// </summary>
        [ObservableProperty]
        private string _morningTemp;

        /// <summary>
        /// Температура днем в 12 часов.
        /// </summary>
        [ObservableProperty]
        private string _dayTemp;

        /// <summary>
        /// Температура вечером в 18 часов.
        /// </summary>
        [ObservableProperty]
        private string _eveningTemp;

        /// <summary>
        /// Температура ночью в 0 часов.
        /// </summary>
        [ObservableProperty]
        private string _nightTemp;

        /// <summary>
        /// Максимальная температура за день.
        /// </summary>
        [ObservableProperty]
        private string _maxTemp;

        /// <summary>
        /// Минимальная температура за день.
        /// </summary>
        [ObservableProperty]
        private string _minTemp;

        /// <summary>
        /// Прогноз погоды на неделю.
        /// </summary>
        [ObservableProperty]
        private List<DailyForecast> _weekForecast;

        /// <summary>
        /// Время восхода солнца.
        /// </summary>
        [ObservableProperty]
        private string _sunriseTime;

        /// <summary>
        /// Время захода солнца.
        /// </summary>
        [ObservableProperty]
        private string _sunsetTime;
        #endregion

        /// <summary>
        /// Конструктор класса <see cref="MainVM"/>.
        /// </summary>
        public MainVM()
        {
            CityName = "London";
            GetWeatherCommand.Execute(this);

            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        #region Commands
        /// <inheritdoc cref="Environment.Exit(int)"/>
        [RelayCommand]
        private void Quit()
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Команда, которая получает JSON файл с погодой
        /// при помощи запроса на сервер.
        /// </summary>
        [RelayCommand]
        private async Task GetWeather()
        {
            if (string.IsNullOrEmpty(CityName))
            {
                Console.WriteLine($"{CityName} was not found.");
            }

            string apiUrl = $"http://api.weatherapi.com/v1/forecast.json?key={_apiKeyWeather}&q={CityName}&days=7&aqi=yes";

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

                            if (weatherData != null
                                && weatherData.CurrentWeather != null 
                                && weatherData.CurrentWeather.Condition != null)
                            {
                                if (weatherData.Forecast != null 
                                    && weatherData.Forecast.Forecastday.Count > 0)
                                {
                                    WeekForecast = weatherData.Forecast.Forecastday.Select(day => new DailyForecast
                                    {
                                        Date = day.Date,
                                        MaxTempC = day.Day.MaxTempC,
                                        MinTempC = day.Day.MinTempC,
                                        MaxTempF = day.Day.MaxTempF,
                                        MinTempF = day.Day.MinTempF
                                    }).ToList();
                                }
                                DisplayWeatherData(weatherData);
                                DisplayTimeData(weatherData);
                            }
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
        /// Команда, которая преобразует температуру из Фаренгейта в Цельсия.
        /// </summary>
        [RelayCommand]
        private void ToggleTempToCelsius()
        {
            if (!_isTempInCelsius)
            {
                _isTempInCelsius = true;
                Temp = Convert.ToInt32(_tempC) + "°c";
                MorningTemp = $"+{Convert.ToInt32(_morningTempC)}°";
                DayTemp = $"+{Convert.ToInt32(_dayTempC)}°";
                EveningTemp = $"+{Convert.ToInt32(_eveningTempC)}°";
                NightTemp = $"+{Convert.ToInt32(_nightTempC)}°";

                TempCelsiusButtonBackground = _blackColor;
                TempCelsiusButtonForeground = _whiteColor;
                TempFahrenheitButtonBackground = _whiteColor;
                TempFahrenheitButtonForeground = _blackColor;
            }
        }

        /// <summary>
        /// Команда, которая преобразует температуру из Цельсия в Фаренгейта.
        /// </summary>
        [RelayCommand]
        private void ToggleTempToFahrenheit()
        {
            if (_isTempInCelsius)
            {
                _isTempInCelsius = false;
                Temp = Convert.ToInt32(_tempF) + "°F";
                MorningTemp = $"+{Convert.ToInt32(_morningTempF)}°";
                DayTemp = $"+{Convert.ToInt32(_dayTempF)}°";
                EveningTemp = $"+{Convert.ToInt32(_eveningTempF)}°";
                NightTemp = $"+{Convert.ToInt32(_nightTempF)}°";

                TempFahrenheitButtonBackground = _blackColor;
                TempFahrenheitButtonForeground = _whiteColor;
                TempCelsiusButtonBackground = _whiteColor;
                TempCelsiusButtonForeground = _blackColor;
            }
        }

        /// <summary>
        /// Команда, которая переключает виджет погоды из состояния 
        /// прогноз на сегодня на недельный прогноз и наоборот.
        /// </summary>
        [RelayCommand]
        private void ToggleToTodayForecast()
        {
            if (!_isWeekForecast)
            {
                _isWeekForecast = true;
                WeekWeatherVisibility = !WeekWeatherVisibility;
                TodayWeatherVisibility = !TodayWeatherVisibility;
            }
        }

        /// <summary>
        /// Команда, которая переключает виджет погоды из состояния 
        /// прогноз на сегодня на недельный прогноз и наоборот.
        /// </summary>
        [RelayCommand]
        private void ToggleToWeekForecast()
        {
            if (_isWeekForecast)
            {
                _isWeekForecast = false;
                TodayWeatherVisibility = !TodayWeatherVisibility;
                WeekWeatherVisibility = !WeekWeatherVisibility;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Метод, который отображает полученные данные о погоде на View.
        /// </summary>
        /// <param name="weatherData">Данные о погоде.</param>
        private void DisplayWeatherData(WeatherData weatherData)
        {
            _tempC = weatherData.CurrentWeather.TempC;
            _tempF = weatherData.CurrentWeather.TempF;
            Temp = Convert.ToInt32(_tempC) + "°c";

            var todayForecast = weatherData.Forecast.Forecastday[0];
            _morningTempF = todayForecast.Hour[6].TempF;
            _dayTempF = todayForecast.Hour[12].TempF;
            _eveningTempF = todayForecast.Hour[18].TempF;
            _nightTempF = todayForecast.Hour[0].TempF;

            _morningTempC = todayForecast.Hour[6].TempC;
            _dayTempC = todayForecast.Hour[12].TempC;
            _eveningTempC = todayForecast.Hour[18].TempC;
            _nightTempC = todayForecast.Hour[0].TempC;

            MorningTemp = $"+{Convert.ToInt32(todayForecast.Hour[6].TempC)}°";
            DayTemp = $"+{Convert.ToInt32(todayForecast.Hour[12].TempC)}°";
            EveningTemp = $"+{Convert.ToInt32(todayForecast.Hour[18].TempC)}°";
            NightTemp = $"+{Convert.ToInt32(todayForecast.Hour[0].TempC)}°";

            WindSpeed = weatherData.CurrentWeather.WindKph;
            WindDir = weatherData.CurrentWeather.WindDirection;
            WindDirectionAngle = GetWindDirectionAngle(weatherData.CurrentWeather.WindDegree);

            SunriseTime = todayForecast.Astro.Sunrise;
            SunsetTime = todayForecast.Astro.Sunset;

            Humidity = weatherData.CurrentWeather.Humidity;
            HumiditySlider = Humidity / 10;

            ConditionText = weatherData.CurrentWeather.Condition.Text;
            ConditionIcon = "https:" + weatherData.CurrentWeather.Condition.Icon;
            ConditionMorningIcon = "https:" + todayForecast.Hour[6].Condition.Icon;
            ConditionDayIcon = "https:" + todayForecast.Hour[12].Condition.Icon;
            ConditionEveningIcon = "https:" + todayForecast.Hour[18].Condition.Icon;
            ConditionNightIcon = "https:" + todayForecast.Hour[0].Condition.Icon;

            VisibilityKm = weatherData.CurrentWeather.VisibilityKm;
            UvIndex = $"Average is {weatherData.CurrentWeather.UvIndex}";
            ChanceOfRainToday = $"Rain - {weatherData.Forecast.Forecastday[0].Day.DailyChanceOfRain}%";

            UsEpaIndex = weatherData.CurrentWeather.AirQuality.UsEpaIndex;
            AirQualityAssessment = GetAirQualityAssessment(UsEpaIndex);
        }

        #region Display time
        /// <summary>
        /// Метод, который отображает полученные данные о локальном времени на View.
        /// </summary>
        /// <param name="weatherData"></param>
        private void DisplayTimeData(WeatherData weatherData)
        {
            Location = weatherData.Location.Name + ", " + weatherData.Location.Country;
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
                            DateTimeNow = timeZoneInfo.Formatted;
                            StartRealTimeClock(timeZoneInfo.Formatted);
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
        /// Метод, который обновляет время каждую секунду.
        /// </summary>
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.TryParse(DateTimeNow, out DateTime currentDateTime))
            {
                currentDateTime = currentDateTime.AddSeconds(1);
                DateTimeNow = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        /// <summary>
        /// Метод, который запускает таймер с первоначальным значением времени.
        /// </summary>
        /// <param name="initialTime">Первоначальное значение времени.</param>
        private void StartRealTimeClock(string initialTime)
        {
            if (DateTime.TryParse(initialTime, out DateTime initialDateTime))
            {
                DateTimeNow = initialDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        #endregion

        /// <summary>
        /// Метод, который разворачивает иконку в зависимости от направления ветра.
        /// </summary>
        /// <param name="windDegree">Направление ветра в градусах.</param>
        /// <returns>Угол поворота иконки.</returns>
        // TODO: доделать
        private int GetWindDirectionAngle(double windDegree)
        {
            return windDegree switch
            {
                >= 348.75 or < 11.25 => 0,     // N (337.5° - 22.5°)
                >= 11.25 and < 33.75 => 23,    // NNE (22.5° - 45°)
                >= 33.75 and < 56.25 => 45,    // NE (45° - 67.5°)
                >= 56.25 and < 78.75 => 68,    // ENE (67.5° - 90°)
                >= 78.75 and < 101.25 => 90,   // E (90° - 112.5°)
                >= 101.25 and < 123.75 => 113, // ESE (112.5° - 135°)
                >= 123.75 and < 146.25 => 135, // SE (135° - 157.5°)
                >= 146.25 and < 168.75 => 158, // SSE (157.5° - 180°)
                >= 168.75 and < 191.25 => 180, // S (180° - 202.5°)
                >= 191.25 and < 213.75 => 203, // SSW (202.5° - 225°)
                >= 213.75 and < 236.25 => 225, // SW (225° - 247.5°)
                >= 236.25 and < 258.75 => 248, // WSW (247.5° - 270°)
                >= 258.75 and < 281.25 => 270, // W (270° - 292.5°)
                >= 281.25 and < 303.75 => 293, // WNW (292.5° - 315°)
                >= 303.75 and < 326.25 => 315, // NW (315° - 337.5°)
                >= 326.25 and < 348.75 => 338, // NNW (337.5° - 360°)
                _ => 0                         // N по умолчанию
            };
        }

        /// <summary>
        /// Метод, который в зависимости от индекса качества воздуха 
        /// по стандарту US - EPA дает словесную оценку качества воздуха.
        /// </summary>
        /// <param name="usEpaIndex">Индекс качества воздуха по стандарту US - EPA.</param>
        /// <returns>Словесная оценка качества воздуха.</returns>
        private string GetAirQualityAssessment(int usEpaIndex)
        {
            return usEpaIndex switch
            {
                1 => "Good",
                2 => "Moderate",
                3 => "Unhealthy for Sensitive Groups",
                4 => "Unhealthy",
                5 => "Very Unhealthy",
                6 => "Hazardous",
                _ => "Unknown",
            };
        }
        #endregion
    }
}
﻿using CommunityToolkit.Mvvm.ComponentModel;
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
        /// Минимальная температура утром по Цельсию.
        /// </summary>
        private double _minMorningTempC;

        /// <summary>
        /// Максимальная температура утром по Цельсию.
        /// </summary>
        private double _maxMorningTempC;

        /// <summary>
        /// Минимальная температура днем по Цельсию.
        /// </summary>
        private double _minDayTempC;

        /// <summary>
        /// Максимальная температура днем по Цельсию.
        /// </summary>
        private double _maxDayTempC;

        /// <summary>
        /// Минимальная температура вечером по Цельсию.
        /// </summary>
        private double _minEveningTempC;

        /// <summary>
        /// Максимальная температура вечером по Цельсию.
        /// </summary>
        private double _maxEveningTempC;

        /// <summary>
        /// Минимальная температура ночью по Цельсию.
        /// </summary>
        private double _minNightTempC;

        /// <summary>
        /// Максимальная температура ночью по Цельсию.
        /// </summary>
        private double _maxNightTempC;

        /// <summary>
        /// Минимальная температура утром по Фаренгейту.
        /// </summary>
        private double _minMorningTempF;

        /// <summary>
        /// Максимальная температура утром по Фаренгейту.
        /// </summary>
        private double _maxMorningTempF;

        /// <summary>
        /// Минимальная температура днем по Фаренгейту.
        /// </summary>
        private double _minDayTempF;

        /// <summary>
        /// Максимальная температура днем по Фаренгейту.
        /// </summary>
        private double _maxDayTempF;

        /// <summary>
        /// Минимальная температура вечером по Фаренгейту.
        /// </summary>
        private double _minEveningTempF;

        /// <summary>
        /// Максимальная температура вечером по Фаренгейту.
        /// </summary>
        private double _maxEveningTempF;

        /// <summary>
        /// Минимальная температура ночью по Фаренгейту.
        /// </summary>
        private double _minNightTempF;

        /// <summary>
        /// Максимальная температура ночью по Фаренгейту.
        /// </summary>
        private double _maxNightTempF;

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
        /// Получены ли данные о погоде.
        /// </summary>
        [ObservableProperty]
        private bool _isWeatherDataAvailable;

        /// <summary>
        /// Текущее время в формате YYYY-MM-DD hh:mm:ss.
        /// </summary>
        [ObservableProperty]
        private string _dateTimeNow;

        /// <summary>
        /// Текущий день недели.
        /// </summary>
        [ObservableProperty]
        private string _currentDayOfWeek;

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
        /// Словесная оценка влажности на улице.
        /// </summary>
        [ObservableProperty]
        private string _humidityAssessment;

        /// <summary>
        /// Оценка влажности в виде смайла.
        /// </summary>
        [ObservableProperty]
        private string _humiditySmileIcon;

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
        /// Словесная оценка видимости на улице.
        /// </summary>
        [ObservableProperty]
        private string _visibilityAssessment;

        /// <summary>
        /// Оценка видимости в виде смайла.
        /// </summary>
        [ObservableProperty]
        private string _visibilitySmileIcon;

        /// <summary>
        /// УФ-индекс.
        /// </summary>
        [ObservableProperty]
        private double _uvIndex;

        /// <summary>
        /// Вероятность того, что сегодня пойдет дождь.
        /// </summary>
        [ObservableProperty]
        private double _chanceOfRainToday;

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
        /// Преобразованное значение индекса качества воздуха для слайдера (от 1 до 10).
        /// </summary>
        [ObservableProperty]
        private double _usEpaIndexSlider;

        /// <summary>
        /// Словесная оценка качества воздуха по стандарту US - EPA.
        /// </summary>
        [ObservableProperty]
        private string _airQualityAssessment;

        /// <summary>
        /// Оценка качества воздуха в виде смайла.
        /// </summary>
        [ObservableProperty]
        private string _airQualitySmileIcon;

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
        /// Диапазон значений температуры за утро.
        /// </summary>
        [ObservableProperty]
        private string _morningTempRange;

        /// <summary>
        /// Диапазон значений температуры за день.
        /// </summary>
        [ObservableProperty]
        private string _dayTempRange;

        /// <summary>
        /// Диапазон значений температуры за вечер.
        /// </summary>
        [ObservableProperty]
        private string _eveningTempRange;

        /// <summary>
        /// Диапазон значений температуры за ночь.
        /// </summary>
        [ObservableProperty]
        private string _nightTempRange;

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

        /// <summary>
        /// Высота текствого блока словесной оценки 
        /// качества воздуха и соответствующего смайла.
        /// </summary>
        [ObservableProperty]
        private int _heightAirQualityTextBlock;
        #endregion

        /// <summary>
        /// Конструктор класса <see cref="MainVM"/>.
        /// </summary>
        public MainVM()
        {
            IsWeatherDataAvailable = false;
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
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    string jsonResponse = reader.ReadToEnd();
                    WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(jsonResponse);

                    if (weatherData != null
                        && weatherData.CurrentWeather.Condition != null
                        && weatherData.Forecast != null
                        && weatherData.Forecast.Forecastday.Count > 0)
                    {
                        WeekForecast = weatherData.Forecast.Forecastday.Select(day => new DailyForecast
                        {
                            Date = day.Date,
                            MaxTempC = day.Day.MaxTempC,
                            MinTempC = day.Day.MinTempC,
                            MaxTempF = day.Day.MaxTempF,
                            MinTempF = day.Day.MinTempF,
                            Condition = day.Day.Condition
                        }).ToList();

                        IsWeatherDataAvailable = true;
                        DisplayWeatherData(weatherData);
                        DisplayTimeData(weatherData);
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
                UpdateTemperatureDisplay();
                InvertColorTempButton();
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
                UpdateTemperatureDisplay();
                InvertColorTempButton();
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
            DisplayTodayForecast(weatherData);
            Location = weatherData.Location.Name + ", " + weatherData.Location.Country;

            _tempC = weatherData.CurrentWeather.TempC;
            _tempF = weatherData.CurrentWeather.TempF;
            Temp = FormatTemperature(_tempC);
            ChanceOfRainToday = weatherData.Forecast.Forecastday[0].Day.DailyChanceOfRain;
            ConditionText = weatherData.CurrentWeather.Condition.Text;
            ConditionIcon = "https:" + weatherData.CurrentWeather.Condition.Icon;

            foreach (var forecast in WeekForecast)
            {
                forecast.UpdateTemperatureDisplay(_isTempInCelsius);
            }

            UvIndex = weatherData.CurrentWeather.UvIndex;

            WindSpeed = weatherData.CurrentWeather.WindKph;
            WindDir = weatherData.CurrentWeather.WindDirection;
            WindDirectionAngle = GetWindDirectionAngle(weatherData.CurrentWeather.WindDegree);

            SunriseTime = weatherData.Forecast.Forecastday[0].Astro.Sunrise;
            SunsetTime = weatherData.Forecast.Forecastday[0].Astro.Sunset;

            Humidity = weatherData.CurrentWeather.Humidity;
            HumiditySlider = Humidity / 10;
            HumidityAssessment = GetHumidityAssessment(Humidity);
            HumiditySmileIcon = GetHumiditySmileIcon(Humidity);

            VisibilityKm = weatherData.CurrentWeather.VisibilityKm;
            VisibilityAssessment = GetVisibilityAssessment(VisibilityKm);
            VisibilitySmileIcon = GetVisibilitySmileIcon(VisibilityKm);

            UsEpaIndex = weatherData.CurrentWeather.AirQuality.UsEpaIndex;
            if (UsEpaIndex == 3) HeightAirQualityTextBlock = 30;
            else HeightAirQualityTextBlock = 18;
            AirQualityAssessment = GetAirQualityAssessment(UsEpaIndex);
            AirQualitySmileIcon = GetAirSmileIcon(UsEpaIndex);
        }

        /// <summary>
        /// Метод, который отображает сегодняшний прогноз погоды на соответстующей вкладке.
        /// </summary>
        /// <param name="weatherData">Данные о погоде.</param>
        private void DisplayTodayForecast(WeatherData weatherData)
        {
            var todayForecast = weatherData.Forecast.Forecastday[0];

            _minMorningTempC = todayForecast.Hour.Take(6).Min(h => h.TempC);
            _maxMorningTempC = todayForecast.Hour.Take(6).Max(h => h.TempC);
            _minDayTempC = todayForecast.Hour.Skip(6).Take(6).Min(h => h.TempC);
            _maxDayTempC = todayForecast.Hour.Skip(6).Take(6).Max(h => h.TempC);
            _minEveningTempC = todayForecast.Hour.Skip(12).Take(6).Min(h => h.TempC);
            _maxEveningTempC = todayForecast.Hour.Skip(12).Take(6).Max(h => h.TempC);
            _minNightTempC = todayForecast.Hour.Skip(18).Take(6).Min(h => h.TempC);
            _maxNightTempC = todayForecast.Hour.Skip(18).Take(6).Max(h => h.TempC);

            _minMorningTempF = todayForecast.Hour.Take(6).Min(h => h.TempF);
            _maxMorningTempF = todayForecast.Hour.Take(6).Max(h => h.TempF);
            _minDayTempF = todayForecast.Hour.Skip(6).Take(6).Min(h => h.TempF);
            _maxDayTempF = todayForecast.Hour.Skip(6).Take(6).Max(h => h.TempF);
            _minEveningTempF = todayForecast.Hour.Skip(12).Take(6).Min(h => h.TempF);
            _maxEveningTempF = todayForecast.Hour.Skip(12).Take(6).Max(h => h.TempF);
            _minNightTempF = todayForecast.Hour.Skip(18).Take(6).Min(h => h.TempF);
            _maxNightTempF = todayForecast.Hour.Skip(18).Take(6).Max(h => h.TempF);

            MorningTempRange = FormatTemperatureRange(_minMorningTempC, _maxMorningTempC);
            DayTempRange = FormatTemperatureRange(_minDayTempC, _maxDayTempC);
            EveningTempRange = FormatTemperatureRange(_minEveningTempC, _maxEveningTempC);
            NightTempRange = FormatTemperatureRange(_minNightTempC, _maxNightTempC);

            ConditionMorningIcon = todayForecast.Hour[6].Condition.IconUrl;
            ConditionDayIcon = todayForecast.Hour[12].Condition.IconUrl;
            ConditionEveningIcon = todayForecast.Hour[18].Condition.IconUrl;
            ConditionNightIcon = todayForecast.Hour[0].Condition.IconUrl;
        }

        #region Display time
        /// <summary>
        /// Метод, который отображает полученные данные о локальном времени на View.
        /// </summary>
        /// <param name="weatherData"></param>
        private void DisplayTimeData(WeatherData weatherData)
        {
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
                CurrentDayOfWeek = currentDateTime.DayOfWeek.ToString();
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

        /// <summary>
        /// Метод, который в зависимости от индекса качества воздуха 
        /// определяет путь к подходящему смайлу.
        /// </summary>
        /// <param name="usEpaIndex">Индекс качества воздуха.</param>
        /// <returns>Путь к иконке смайла.</returns>
        private string GetAirSmileIcon(int usEpaIndex)
        {
            return usEpaIndex switch
            {
                1 or 2 => "Resources/like.png",
                3 => "Resources/neutral_32x32.png",
                4 or 5 or 6 => "Resources/dislike.png",
                _ => "Unknown",
            };
        }

        /// <summary>
        /// Метод, который в зависимости от влажности 
        /// воздуха дает ее словесную оценку.
        /// </summary>
        /// <param name="humidity">Влажность воздуха.</param>
        /// <returns>Словесная оценка влажности воздуха.</returns>
        private string GetHumidityAssessment(double humidity)
        {
            return humidity switch
            {
                >= 0 and <30 => "Air is dry",
                >=30 and <=70 => "Normal",
                >70 and <=100 => "Air is saturated",
                _ => "Unknown",
            };
        }

        /// <summary>
        /// Метод, который в зависимости от влажности воздуха 
        /// определяет путь к подходящему смайлу.
        /// </summary>
        /// <param name="humidity">Влажность воздуха.</param>
        /// <returns>Путь к иконке смайла.</returns>
        private string GetHumiditySmileIcon(double humidity)
        {
            return humidity switch
            {
                >= 0 and <30 => "Resources/dislike.png",
                >=30 and <=70 => "Resources/like.png",
                >70 and <=100 => "Resources/dislike.png",
                _ => "Unknown",
            };
        }

        /// <summary>
        /// Метод, который в зависимости от дальности 
        /// видимости дает ее словесную оценку.
        /// </summary>
        /// <param name="visibilityKm">Видимость.</param>
        /// <returns>Словесная оценка текущей видимости.</returns>
        private string GetVisibilityAssessment(double visibilityKm)
        {
            return visibilityKm switch
            {
                >=0 and <10 => "Bad",
                >=10 and <16 => "Average",
                >=16 => "Beatiful",
                _ => "Unknown",
            };
        }

        /// <summary>
        /// Метод, который в зависимости от дальности видимости определяет путь к подходящему смайлу.
        /// </summary>
        /// <param name="visibilityKm">Видимость.</param>
        /// <returns>Путь к иконке смайла.</returns>
        private string GetVisibilitySmileIcon(double visibilityKm)
        {
            return visibilityKm switch
            {
                >=0 and <10 => "Resources/dislike.png",
                >=10 and <16 => "Resources/like.png",
                >=16 => "Resources/happy.png",
                _ => "Unknown",
            };
        }

        /// <summary>
        /// Метод, который преобразует значение температуры в нужный вид.
        /// </summary>
        /// <param name="temperature">Значение температуры.</param>
        /// <returns>Строка, содержащая знак температуры, 
        /// целочисленное значение температуры и знак градуса.</returns>
        private string FormatTemperature(double temperature)
        {
            var sign = temperature > 0 ? "+" : "-";
            if (Math.Round(temperature) == 0) sign = "";
            return $"{sign}{Math.Round(temperature)}°";
        }

        /// <summary>
        /// Метод, который преобразует значения температур в нужный вид.
        /// </summary>
        /// <param name="minTemp">Минимальная температура за определенное время суток.</param>
        /// <param name="maxTemp">Максимальная температура за определенное время суток.</param>
        /// <returns>Строка формата '+{minTemp}°...+{maxTemp}°'.</returns>
        private string FormatTemperatureRange(double minTemp, double maxTemp)
        {
            return $"+{Math.Round(minTemp)}°...+{Math.Round(maxTemp)}°";
        }

        /// <summary>
        /// Метод, который меняет отображаемую температуру 
        /// в зависимости от выбранной температурной шкалы.
        /// </summary>
        private void UpdateTemperatureDisplay()
        {
            if (_isTempInCelsius)
            {
                Temp = FormatTemperature(_tempC);
                MorningTempRange = FormatTemperatureRange(_minMorningTempC, _maxMorningTempC);
                DayTempRange = FormatTemperatureRange(_minDayTempC, _maxDayTempC);
                EveningTempRange = FormatTemperatureRange(_minEveningTempC, _maxEveningTempC);
                NightTempRange = FormatTemperatureRange(_minNightTempC, _maxNightTempC);
            }
            else
            {
                Temp = FormatTemperature(_tempF);
                MorningTempRange = FormatTemperatureRange(_minMorningTempF, _maxMorningTempF);
                DayTempRange = FormatTemperatureRange(_minDayTempF, _maxDayTempF);
                EveningTempRange = FormatTemperatureRange(_minEveningTempF, _maxEveningTempF);
                NightTempRange = FormatTemperatureRange(_minNightTempF, _maxNightTempF);
            }

            foreach (var forecast in WeekForecast)
            {
                forecast.UpdateTemperatureDisplay(_isTempInCelsius);
            }
        }

        /// <summary>
        /// Метод, который инвертирует цвета кнопок, отвечающих за тип температурной шкалы.
        /// </summary>
        private void InvertColorTempButton()
        {
            TempCelsiusButtonBackground = _isTempInCelsius ? _blackColor : _whiteColor;
            TempCelsiusButtonForeground = _isTempInCelsius ? _whiteColor : _blackColor;
            TempFahrenheitButtonBackground = _isTempInCelsius ? _whiteColor : _blackColor;
            TempFahrenheitButtonForeground = _isTempInCelsius ? _blackColor : _whiteColor;
        }
        #endregion
    }
}
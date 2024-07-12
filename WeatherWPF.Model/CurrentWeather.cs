using Newtonsoft.Json;

namespace WeatherWPF.Model
{
    public class CurrentWeather
    {
        [JsonProperty("temp_c")]
        public double TempC { get; set; }

        [JsonProperty("condition")]
        public Condition Condition { get; set; }

        [JsonProperty("wind_kph")]
        public double WindKph { get; set; }

        [JsonProperty("humidity")]
        public double Humidity { get; set; }
    }
}
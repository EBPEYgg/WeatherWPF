using Newtonsoft.Json;

namespace WeatherWPF.Model
{
    public class WeatherData
    {
        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("current")]
        public CurrentWeather CurrentWeather { get; set; }

        [JsonProperty("forecast")]
        public Forecast Forecast { get; set; }
    }
}
using Newtonsoft.Json;

namespace WeatherWPF.Model
{
    public class Forecast
    {
        [JsonProperty("forecastday")]
        public List<Forecastday> Forecastday { get; set; }
    }
}
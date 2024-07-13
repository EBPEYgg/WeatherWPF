using Newtonsoft.Json;

namespace WeatherWPF.Model
{
    public class AirQuality
    {
        [JsonProperty("us-epa-index")]
        public int UsEpaIndex { get; set; } = 1;
    }
}
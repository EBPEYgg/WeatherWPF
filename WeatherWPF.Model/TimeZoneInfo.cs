using Newtonsoft.Json;

namespace WeatherWPF.Model
{
    public class TimeZoneInfo
    {
        [JsonProperty("formatted")]
        public string Formatted { get; set; }
    }
}
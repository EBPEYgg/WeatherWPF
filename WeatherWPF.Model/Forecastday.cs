using Newtonsoft.Json;

namespace WeatherWPF.Model
{
    public class Forecastday
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("day")]
        public Day Day { get; set; }

        [JsonProperty("hour")]
        public List<Hour> Hour { get; set; }
    }
}
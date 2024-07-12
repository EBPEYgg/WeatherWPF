using Newtonsoft.Json;

namespace WeatherWPF.Model
{
    public class Location
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
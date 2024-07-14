using Newtonsoft.Json;

namespace WeatherWPF.Model
{
    public class Astro
    {
        [JsonProperty("sunrise")]
        public string Sunrise { get; set; }

        [JsonProperty("sunset")]
        public string Sunset { get; set; }
    }
}
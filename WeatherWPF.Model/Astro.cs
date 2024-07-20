using Newtonsoft.Json;

namespace WeatherWPF.Model
{
    public class Astro
    {
        [JsonProperty("sunrise")]
        public string Sunrise { get; set; } = "00:00 AM";

        [JsonProperty("sunset")]
        public string Sunset { get; set; } = "00:00 PM";
    }
}
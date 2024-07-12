using Newtonsoft.Json;

namespace WeatherWPF.Model
{
    public class Condition
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
    }
}
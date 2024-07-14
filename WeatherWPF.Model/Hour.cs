using Newtonsoft.Json;

namespace WeatherWPF.Model
{
    public class Hour
    {
        [JsonProperty("temp_c")]
        public double TempC { get; set; } = 0;

        [JsonProperty("temp_f")]
        public double TempF { get; set; } = 0;
    }
}
using Newtonsoft.Json;

namespace WeatherWPF.Model
{
    public class Day
    {
        [JsonProperty("maxtemp_c")]
        public double MaxTempC { get; set; }

        [JsonProperty("mintemp_c")]
        public double MinTempC { get; set; }

        [JsonProperty("maxtemp_f")]
        public double MaxTempF { get; set; }

        [JsonProperty("mintemp_f")]
        public double MinTempF { get; set; }

        [JsonProperty("daily_chance_of_rain")]
        public double DailyChanceOfRain { get; set; }
    }
}
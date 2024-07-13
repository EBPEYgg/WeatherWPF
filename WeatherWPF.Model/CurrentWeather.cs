using Newtonsoft.Json;

namespace WeatherWPF.Model
{
    public class CurrentWeather
    {
        [JsonProperty("temp_c")]
        public double TempC { get; set; } = 0;

        [JsonProperty("temp_f")]
        public double TempF { get; set; } = 0;

        [JsonProperty("condition")]
        public Condition Condition { get; set; }

        [JsonProperty("wind_kph")]
        public double WindKph { get; set; } = 0;

        [JsonProperty("wind_degree")]
        public int WindDegree { get; set; } = 0;

        [JsonProperty("wind_dir")]
        public string WindDirection { get; set; } = "";

        [JsonProperty("humidity")]
        public double Humidity { get; set; } = 0;

        [JsonProperty("vis_km")]
        public double VisibilityKm { get; set; } = 0;

        [JsonProperty("uv")]
        public double UvIndex { get; set; } = 0;

        [JsonProperty("daily_chance_of_rain")]
        public int ChanceOfRainToday { get; set; }

        [JsonProperty("air_quality")]
        public AirQuality AirQuality { get; set; }
    }
}
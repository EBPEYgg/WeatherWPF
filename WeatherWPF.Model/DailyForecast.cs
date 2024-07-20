using CommunityToolkit.Mvvm.ComponentModel;
using System.Globalization;

namespace WeatherWPF.Model
{
    public partial class DailyForecast : ObservableObject
    {
        public string Date { get; set; }

        public string DayOfWeek
        {
            get
            {
                if (DateTime.TryParse(Date, out DateTime dateTime))
                {
                    return dateTime.ToString("ddd", CultureInfo.InvariantCulture);
                }
                return string.Empty;
            }
        }

        public Condition Condition { get; set; }

        public string ConditionIconUrl { get => $"https:{Condition?.Icon}"; }

        public double MaxTempC { get; set; }

        public double MinTempC { get; set; }

        public double MaxTempF { get; set; }

        public double MinTempF { get; set; }

        [ObservableProperty]
        public string _maxTemp;

        [ObservableProperty]
        public string _minTemp;

        public void UpdateTemperatureDisplay(bool isTempInCelsius)
        {
            MaxTemp = $"+{Convert.ToInt32(isTempInCelsius ? MaxTempC : MaxTempF)}°";
            MinTemp = $"+{Convert.ToInt32(isTempInCelsius ? MinTempC : MinTempF)}°";
        }
    }
}
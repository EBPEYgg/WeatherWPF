using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WeatherWPF.View.Services
{
    /// <summary>
    /// Класс, описывающий конвертер значения для свойства Visibility.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Метод, представляющий собой конвертер, который преобразует 
        /// логические значения в значения перечисления Visibility.
        /// </summary>
        /// <param name="value">Булевое значение, подлежащее преобразованию.</param>
        /// <param name="targetType">Тип, в который будет преобразовано значение.</param>
        /// <param name="parameter">Необязательный параметр, используемый при преобразовании.</param>
        /// <param name="culture">Язык, который будет использоваться в конвертере.</param>
        /// <returns>Visibility.Visible если булевое значение true, 
        /// иначе Visibility.Collapsed.</returns>
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && boolValue)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        /// <summary>
        /// ConverterBack не поддерживается.
        /// </summary>
        /// <param name="value">Значение для обратного преобразования.</param>
        /// <param name="targetType">Тип, к которому значение будет преобразовано обратно.</param>
        /// <param name="parameter">Необязательный параметр, используемый при преобразовании.</param>
        /// <param name="culture">Язык, который будет использоваться в конвертере.</param>
        /// <returns>Вызывает исключение NotSupportedException.</returns>
        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
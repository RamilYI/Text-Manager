using System;
using System.Windows.Data;

namespace TechTest.HelperClasses
{
    /// <summary>
    /// Класс, определяющий тип объекта
    /// </summary>
    [ValueConversion(typeof(object), typeof(string))]
    public class ObjectToTypeConverter : IValueConverter
    {
        /// <summary>
        /// Поймать тип объекта и вернуть его значение
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>Возвращает "Документ", если тип - Document,
        /// если же тип - Task, то возвращает "Задача"</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var typeName = (value == null || value.GetType().Name == "NamedObject") ? "" : value.GetType().Name;
            return typeName == "Document" ? "Документ" : "Задача";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}
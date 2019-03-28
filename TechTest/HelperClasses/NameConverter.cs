using System;
using System.Globalization;
using System.Windows.Data;
using TechTest.Models;

namespace TechTest.HelperClasses
{
    /// <summary>
    /// Класс, использующийся для конвертации имён документов
    /// </summary>
    public class NameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType().GetProperty("Uuid") != null) return ((Document) value).Name + ".doc";
            return ((Task)value).Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
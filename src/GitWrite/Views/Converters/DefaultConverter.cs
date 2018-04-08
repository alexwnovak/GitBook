using System;
using System.Globalization;
using System.Windows.Data;

namespace GitWrite.Views.Converters
{
   public class DefaultConverter : IValueConverter
   {
      public static DefaultConverter Instance { get; } = new DefaultConverter();
      public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) => value;
      public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) => value;
   }
}

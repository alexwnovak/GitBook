using System;
using System.Globalization;
using System.Windows.Data;

namespace GitWrite.Views.Converters
{
   public class RemainingCharactersConverter : IValueConverter
   {
      public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
      {
         int currentLength = (int) value;
         return 72 - currentLength;
      }

      public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }
   }
}

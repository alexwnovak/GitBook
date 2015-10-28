using System;
using System.Globalization;
using System.Windows.Data;

namespace GitWrite
{
   public class TextLengthInversionConverter : IValueConverter
   {
      public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
      {
         int textLength = (int) value;

         if ( textLength < 0 || textLength > 72 )
         {
            return 0;
         }

         return 72 - textLength;
      }

      public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }
   }
}

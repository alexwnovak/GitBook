using System;
using System.Globalization;
using System.Windows.Data;

namespace GitWrite.Views.Converters
{
   public class StringLengthToProgressConverter : IValueConverter
   {
      public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
      {
         if ( value == null )
         {
            return 0.0;
         }

         double doubleValue;

         if ( !double.TryParse( value.ToString(), out doubleValue ) )
         {
            return 0.0;
         }

         return doubleValue;
      }

      public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }
   }
}

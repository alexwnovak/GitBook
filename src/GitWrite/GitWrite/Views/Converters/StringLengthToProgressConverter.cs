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

         throw new NotImplementedException();
      }

      public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }
   }
}

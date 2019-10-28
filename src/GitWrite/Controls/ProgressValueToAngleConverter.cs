using System;
using System.Globalization;
using System.Windows.Data;

namespace GitWrite.Controls
{
   public class ProgressValueToAngleConverter : IMultiValueConverter
   {
      public object Convert( object[] values, Type targetType, object parameter, CultureInfo culture )
      {
         double value = (double) values[0];
         double minimum = (double) values[1];
         double maximum = (double) values[2];

         double percentage = value / ( maximum - minimum );
         double angle = 360 - 360 * percentage;

         return angle;
      }

      public object[] ConvertBack( object value, Type[] targetTypes, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }
   }
}

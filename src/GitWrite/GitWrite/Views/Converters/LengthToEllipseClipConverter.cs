using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GitWrite.Views.Converters
{
   public class LengthToEllipseClipConverter : IValueConverter
   {
      public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
      {
         double width = (double) value;
         double halfWidth = width / 2;

         var center = new Point( halfWidth, halfWidth );
         return new EllipseGeometry( center, halfWidth, halfWidth );
      }

      public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }
   }
}

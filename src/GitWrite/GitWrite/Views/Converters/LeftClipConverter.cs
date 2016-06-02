using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GitWrite.Views.Converters
{
   public class LeftClipConverter : IMultiValueConverter
   {
      private const double _margin = 30;

      public object Convert( object[] values, Type targetType, object parameter, CultureInfo culture )
      {
         var overlayWidth = (double) values[0];
         var textWidth = (double) values[1];

         var clip = Geometry.Parse( $"M {overlayWidth - _margin},0 H {textWidth} V 50 H {overlayWidth - _margin}" );
         clip.Freeze();

         return clip;
      }

      public object[] ConvertBack( object value, Type[] targetTypes, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }
   }
}

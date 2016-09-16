using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GitWrite.Views.Converters
{
   public class RadialClipConverter : IMultiValueConverter
   {
      private Geometry CreateWholeClip( double radius )
      {
         var centerPoint = new Point( radius, radius );
         return new EllipseGeometry( centerPoint, radius, radius );
      }

      public object Convert( object[] values, Type targetType, object parameter, CultureInfo culture )
      {
         double size = (double) values[0];
         double progress = (double) values[1];

         double radius = size / 2;
         double angle = 360 * progress;

         Geometry clipGeometry;

         if ( angle >= 360 )
         {
            clipGeometry = CreateWholeClip( radius );
            clipGeometry.Freeze();

            return clipGeometry;
         }

         double radians = ( angle - 90 ) * Math.PI / 180;
         
         double x = radius * Math.Cos( radians ) + radius;
         double y = radius * Math.Sin( radians ) + radius;

         clipGeometry = Geometry.Parse( $"M {radius},{radius} V 0 A {size},{size} 0 1 1 {x},{y}" );
         clipGeometry.Freeze();

         return clipGeometry;
      }

      public object[] ConvertBack( object value, Type[] targetTypes, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }
   }
}

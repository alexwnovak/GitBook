using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GitWrite.Views.Converters
{
   public class BorderToGeometryConverter : IMultiValueConverter
   {
      private const double _cornerRadius = 14;

      public object Convert( object[] values, Type targetType, object parameter, CultureInfo culture )
      {
         if ( values.Length != 2 || !( values[0] is double ) || !( values[1] is double ) )
         {
            return DependencyProperty.UnsetValue;
         }

         double width = (double) values[0];
         double height = (double) values[1];

         var stringBuilder = new StringBuilder();
         stringBuilder.Append( $"M 0,{_cornerRadius} A {_cornerRadius},{_cornerRadius} 45 0 1 {_cornerRadius},0 " );
         stringBuilder.Append( $"H {width - _cornerRadius} A {_cornerRadius},{_cornerRadius} 45 0 1 {width},{_cornerRadius} " );
         stringBuilder.Append( $"V {height - _cornerRadius} A {_cornerRadius},{_cornerRadius} 45 0 1 {width - _cornerRadius},{height} " );
         stringBuilder.Append( $"H {_cornerRadius} A {_cornerRadius},{_cornerRadius} 45 0 1 0,{height - _cornerRadius}" );

         var clip = Geometry.Parse( stringBuilder.ToString() );
         clip.Freeze();

         return clip;
      }

      public object[] ConvertBack( object value, Type[] targetTypes, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }
   }
}

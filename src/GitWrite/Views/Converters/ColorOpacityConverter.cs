using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace GitWrite.Views.Converters
{
   public class ColorOpacityConverter : MarkupExtension, IValueConverter
   {
      public double Opacity { get; set; } = 1.0;

      public override object ProvideValue( IServiceProvider serviceProvider ) => this;

      public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
      {
         var brush = (SolidColorBrush) value;

         var translucentColor = Color.FromArgb( (byte) (255.0 * Opacity), brush.Color.R, brush.Color.G, brush.Color.B );
         return new SolidColorBrush( translucentColor );
      }

      public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }
   }
}

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace GitWrite.Views.Converters
{
   public class TextLengthToVisibilityConverter : MarkupExtension, IValueConverter
   {
      private static readonly TextLengthToVisibilityConverter _instance = new TextLengthToVisibilityConverter();

      public override object ProvideValue( IServiceProvider serviceProvider ) => _instance;

      public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
      {
         if ( !( value is int ) )
         {
            return Visibility.Collapsed;
         }

         return (int) value == 0 ? Visibility.Visible : Visibility.Collapsed;
      }

      public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }
   }
}

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GitWrite.Views.Converters
{
   public class BoolToCaretBrushConverter : IValueConverter
   {
      public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
      {
         bool boolValue = (bool) value;

         if ( boolValue )
         {
            return Application.Current.Resources["HiddenCaretBrush"];
         }

         return Application.Current.Resources["CaretBrush"];
      }

      public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }
   }
}

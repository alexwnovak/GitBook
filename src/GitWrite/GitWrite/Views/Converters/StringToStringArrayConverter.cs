using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace GitWrite.Views.Converters
{
   public class StringToStringArrayConverter : MarkupExtension, IValueConverter
   {
      public override object ProvideValue( IServiceProvider serviceProvider ) => this;

      public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
      {
         if ( value == null )
         {
            return string.Empty;
         }

         if ( !( value is string[] stringArray ) )
         {
            return string.Empty;
         }

         if ( stringArray.Length == 0 )
         {
            return string.Empty;
         }

         return string.Join( Environment.NewLine, stringArray );
      }

      public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }
   }
}

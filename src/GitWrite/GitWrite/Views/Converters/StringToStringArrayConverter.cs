using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace GitWrite.Views.Converters
{
   public class StringToStringArrayConverter : MarkupExtension, IValueConverter
   {
      private static readonly string[] _emptyStringArray = new string[0];
      private static readonly string[] _splitToken = { Environment.NewLine };

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
         if ( value == null )
         {
            return _emptyStringArray;
         }

         if ( !( value is string stringValue ) )
         {
            return _emptyStringArray;
         }

         if ( string.IsNullOrEmpty( stringValue ) )
         {
            return _emptyStringArray;
         }

         return stringValue.Split( _splitToken, StringSplitOptions.None );
      }
   }
}

using System;
using System.Globalization;
using System.Windows.Data;
using GitWrite.Services;

namespace GitWrite.Views.Converters
{
   public class StringLengthToProgressConverter : IValueConverter
   {
      private readonly int _maxLength;

      public StringLengthToProgressConverter()
      {
         //var appSettings = SimpleIoc.Default.GetInstance<IApplicationSettings>();
         //_maxLength = (int) appSettings.GetSetting( "MaxCommitLength" );

         _maxLength = 72;
      }

      public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
      {
         if ( value == null )
         {
            return 0.0;
         }

         double doubleValue;

         if ( !double.TryParse( value.ToString(), out doubleValue ) )
         {
            return 0.0;
         }

         return 100 * doubleValue / _maxLength;
      }

      public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }
   }
}

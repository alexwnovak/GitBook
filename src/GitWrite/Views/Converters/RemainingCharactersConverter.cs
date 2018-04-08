using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace GitWrite.Views.Converters
{
   public class RemainingCharactersConverter : MarkupExtension, IMultiValueConverter
   {
      private static readonly RemainingCharactersConverter _instance = new RemainingCharactersConverter();
      public override object ProvideValue( IServiceProvider serviceProvider ) => _instance;

      public object Convert( object[] values, Type targetType, object parameter, CultureInfo culture )
      {
         int maxLength = (int) values[0];
         int currentLength = (int) values[1];

         return ( maxLength - currentLength ).ToString();
      }

      public object[] ConvertBack( object value, Type[] targetTypes, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }
   }
}

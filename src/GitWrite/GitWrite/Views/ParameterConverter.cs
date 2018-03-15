using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace GitWrite.Views
{
   public class ParameterConverter : MarkupExtension, IMultiValueConverter
   {
      private static readonly ParameterConverter _instance = new ParameterConverter();

      public override object ProvideValue( IServiceProvider serviceProvider ) => _instance;

      public object Convert( object[] values, Type targetType, object parameter, CultureInfo culture )
      {
         return new ObservableCollection<object>( values );
      }

      public object[] ConvertBack( object value, Type[] targetTypes, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }
   }
}

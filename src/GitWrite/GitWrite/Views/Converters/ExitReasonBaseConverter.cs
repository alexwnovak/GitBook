using System;
using System.Globalization;
using System.Windows.Data;
using GitWrite.ViewModels;

namespace GitWrite.Views.Converters
{
   public abstract class ExitReasonBaseConverter : IValueConverter
   {
      public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
         => OnConvert( (ExitReason) value );

      protected abstract object OnConvert( ExitReason exitReason );

      public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }
   }
}

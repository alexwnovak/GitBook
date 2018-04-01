using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Services;

namespace GitWrite.Views.Converters
{
   public class RemainingCharactersConverter : MarkupExtension, IValueConverter
   {
      private readonly int _maxLength;

      public RemainingCharactersConverter()
      {
         var appSettings = SimpleIoc.Default.GetInstance<IApplicationSettings>();
         _maxLength = (int) appSettings.GetSetting( "MaxCommitLength" );
      }

      public RemainingCharactersConverter( int maxLength )
      {
         _maxLength = maxLength;
      }

      public override object ProvideValue( IServiceProvider serviceProvider ) => this;

      public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
      {
         return _maxLength - (int) value;
      }

      public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }
   }
}

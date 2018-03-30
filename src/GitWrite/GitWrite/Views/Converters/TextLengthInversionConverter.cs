using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Services;

namespace GitWrite.Views.Converters
{
   public class TextLengthInversionConverter : MarkupExtension, IValueConverter
   {
      private readonly int _maxLength;

      public TextLengthInversionConverter()
      {
         var appSettings = SimpleIoc.Default.GetInstance<IApplicationSettings>();
         _maxLength = (int) appSettings.GetSetting( "CommitMaxLength" );
      }

      public TextLengthInversionConverter( int maxLength )
      {
         _maxLength = maxLength;
      }

      public override object ProvideValue( IServiceProvider serviceProvider ) => this;

      public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
      {
         int textLength = (int) value;

         if ( textLength < 0 || textLength > _maxLength )
         {
            return 0;
         }

         return _maxLength - textLength;
      }

      public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }
   }
}

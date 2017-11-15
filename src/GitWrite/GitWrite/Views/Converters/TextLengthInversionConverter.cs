using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Services;

namespace GitWrite.Views.Converters
{
   public class ValueConverter<TFrom, TTo> : MarkupExtension, IValueConverter
   {
      private static readonly ValueConverter<TFrom, TTo> _sharedInstance = new ValueConverter<TFrom, TTo>();

      public bool IsShared
      {
         get;
         set;
      }

      protected ValueConverter()
      {
      }

      public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
      {
         if ( !(value is TFrom) )
         {
            return null;
         }

         return ConvertCore( (TFrom) value );
      }

      protected virtual TTo ConvertCore( TFrom value )
      {
         return default( TTo );
      }

      public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }

      public override object ProvideValue( IServiceProvider serviceProvider )
         => IsShared ? _sharedInstance : this;
   }

   public class NewConverter : ValueConverter<int, string>
   {
      protected override string ConvertCore( int value )
      {
         return "69";
      }
   }

   public class TextLengthInversionConverter : MarkupExtension, IValueConverter
   {
      private static readonly TextLengthInversionConverter _sharedInstance = new TextLengthInversionConverter();

      private readonly int _maxLength;

      public bool IsShared
      {
         get;
         set;
      }

      public TextLengthInversionConverter()
      {
         var appSettings = SimpleIoc.Default.GetInstance<IApplicationSettings>();
         _maxLength = appSettings.MaxCommitLength;
      }

      public TextLengthInversionConverter( IApplicationSettings appSettings )
      {
         _maxLength = appSettings.MaxCommitLength;
      }

      public override object ProvideValue( IServiceProvider serviceProvider )
      {
         return IsShared ? _sharedInstance : this;
      }

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

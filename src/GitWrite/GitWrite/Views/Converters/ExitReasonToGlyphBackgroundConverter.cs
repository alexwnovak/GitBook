using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GitWrite.Views.Converters
{
   public class ExitReasonToGlyphBackgroundConverter : IValueConverter
   {
      public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
      {
         var exitReason = (ExitReason) value;

         switch ( exitReason )
         {
            case ExitReason.AcceptCommit:
               return Application.Current.Resources["AcceptCommitGlyphBackgroundColor"];
            case ExitReason.AbortCommit:
               return Application.Current.Resources["AbortCommitGlyphBackgroundColor"];
         }

         throw new ArgumentException( $"Unknown exit reason: {exitReason}" );
      }

      public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }
   }
}

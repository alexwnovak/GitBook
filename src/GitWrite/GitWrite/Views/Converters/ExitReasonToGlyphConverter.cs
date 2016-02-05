using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GitWrite.Views.Converters
{
   public class ExitReasonToGlyphConverter : IValueConverter
   {
      public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
      {
         var exitReason = (ExitReason) value;

         switch ( exitReason )
         {
            case ExitReason.AcceptCommit:
              return Application.Current.Resources["AcceptCommitGlyph"];
            case ExitReason.AbortCommit:
              return Application.Current.Resources["AbortCommitGlyph"];
         }

         throw new ArgumentException( $"Unknown exit reason: {exitReason}" );
      }

      public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
      {
         throw new NotImplementedException();
      }
   }
}

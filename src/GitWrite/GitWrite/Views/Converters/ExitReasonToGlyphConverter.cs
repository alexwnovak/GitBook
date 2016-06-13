using System;
using System.Windows;
using GitWrite.ViewModels;

namespace GitWrite.Views.Converters
{
   public class ExitReasonToGlyphConverter : ExitReasonBaseConverter
   {
      protected override object OnConvert( ExitReason exitReason )
      {
         switch ( exitReason )
         {
            case ExitReason.Save:
              return Application.Current.Resources["AcceptCommitGlyph"];
            case ExitReason.Discard:
              return Application.Current.Resources["AbortCommitGlyph"];
         }

         throw new ArgumentException( $"Unknown exit reason: {exitReason}" );
      }
   }
}

using System;
using System.Windows;

namespace GitWrite.Views.Converters
{
   public class ExitReasonToGlyphConverter : ExitReasonBaseConverter
   {
      protected override object OnConvert( ExitReason exitReason )
      {
         switch ( exitReason )
         {
            case ExitReason.AcceptCommit:
              return Application.Current.Resources["AcceptCommitGlyph"];
            case ExitReason.AbortCommit:
              return Application.Current.Resources["AbortCommitGlyph"];
         }

         throw new ArgumentException( $"Unknown exit reason: {exitReason}" );
      }
   }
}

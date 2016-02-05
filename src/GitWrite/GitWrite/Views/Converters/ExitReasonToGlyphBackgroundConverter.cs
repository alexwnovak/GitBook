using System;
using System.Windows;

namespace GitWrite.Views.Converters
{
   public class ExitReasonToGlyphBackgroundConverter : ExitReasonBaseConverter
   {
      protected override object OnConvert( ExitReason exitReason )
      {
         switch ( exitReason )
         {
            case ExitReason.AcceptCommit:
               return Application.Current.Resources["AcceptCommitGlyphBackgroundColor"];
            case ExitReason.AbortCommit:
               return Application.Current.Resources["AbortCommitGlyphBackgroundColor"];
         }

         throw new ArgumentException( $"Unknown exit reason: {exitReason}" );
      }
   }
}

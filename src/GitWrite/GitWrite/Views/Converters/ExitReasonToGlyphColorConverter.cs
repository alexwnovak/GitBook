using System;
using System.Windows;
using GitWrite.ViewModels;

namespace GitWrite.Views.Converters
{
   public class ExitReasonToGlyphColorConverter : ExitReasonBaseConverter
   {
      protected override object OnConvert( ExitReason exitReason )
      {
         switch ( exitReason )
         {
            case ExitReason.Save:
               return Application.Current.Resources["AcceptCommitGlyphBackgroundColor"];
            case ExitReason.Discard:
               return Application.Current.Resources["AbortCommitGlyphBackgroundColor"];
         }

         throw new ArgumentException( $"Unknown exit reason: {exitReason}" );
      }
   }
}

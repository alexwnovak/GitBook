using System;
using System.Windows;
using GitWrite.ViewModels;

namespace GitWrite.Views.Converters
{
   public class ExitReasonToBackgroundConverter : ExitReasonBaseConverter
   {
      protected override object OnConvert( ExitReason exitReason )
      {
         switch ( exitReason )
         {
            case ExitReason.Save:
               return Application.Current.Resources["AcceptCommitBackgroundColor"];
            case ExitReason.Discard:
               return Application.Current.Resources["AbortCommitBackgroundColor"];
         }

         throw new ArgumentException( $"Unknown exit reason: {exitReason}" );
      }
   }
}

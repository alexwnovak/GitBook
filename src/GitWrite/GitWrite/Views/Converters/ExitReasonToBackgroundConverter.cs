using System;
using System.Globalization;
using System.Windows;

namespace GitWrite.Views.Converters
{
   public class ExitReasonToBackgroundConverter : ExitReasonBaseConverter
   {
      protected override object OnConvert( ExitReason exitReason )
      {
         switch ( exitReason )
         {
            case ExitReason.AcceptCommit:
               return Application.Current.Resources["AcceptCommitBackgroundColor"];
            case ExitReason.AbortCommit:
               return Application.Current.Resources["AbortCommitBackgroundColor"];
         }

         throw new ArgumentException( $"Unknown exit reason: {exitReason}" );
      }
   }
}

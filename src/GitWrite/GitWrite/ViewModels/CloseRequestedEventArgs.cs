using System;

namespace GitWrite.ViewModels
{
   public class CloseRequestedEventArgs : EventArgs
   {
      public ExitReason ConfirmationResult
      {
         get;
      }

      public CloseRequestedEventArgs( ExitReason confirmationResult )
      {
         ConfirmationResult = confirmationResult;
      }
   }
}

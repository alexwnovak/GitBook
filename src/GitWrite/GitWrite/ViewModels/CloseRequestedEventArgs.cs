using System;
using GitWrite.Services;

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

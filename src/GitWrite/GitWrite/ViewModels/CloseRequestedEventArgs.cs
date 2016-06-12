using System;
using GitWrite.Views.Controls;

namespace GitWrite.ViewModels
{
   public class CloseRequestedEventArgs : EventArgs
   {
      public ConfirmationResult ConfirmationResult
      {
         get;
      }

      public CloseRequestedEventArgs( ConfirmationResult confirmationResult )
      {
         ConfirmationResult = confirmationResult;
      }
   }
}

using System;
using GitWrite.Services;

namespace GitWrite.ViewModels
{
   public class ShutdownEventArgs : EventArgs
   {
      public ExitReason ExitReason
      {
         get;
      }

      public ShutdownEventArgs( ExitReason exitReason )
      {
         ExitReason = exitReason;
      }
   }
}

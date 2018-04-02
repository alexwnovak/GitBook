using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using GitWrite.Services;
using GitWrite.ViewModels;

namespace GitWrite.Messages
{
   public class ExitRequestedMessage : MessageBase
   {
      private readonly TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();

      public Task Task => _tcs.Task;

      public ExitReason ExitReason
      {
         get;
      }

      public ExitRequestedMessage( ExitReason exitReason )
      {
         ExitReason = exitReason;
      }
      
      public void Complete() => _tcs.SetResult( true );
   }
}

using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace GitWrite.ViewModels
{
   public class GitWriteViewModelBase : ViewModelBase
   {
      public event EventHandler<ShutdownEventArgs> ShutdownRequested;

      protected virtual void OnShutdownRequested( object sender, ShutdownEventArgs e ) => ShutdownRequested?.Invoke( sender, e );

      protected virtual Task OnSaveAsync()
      {
         return Task.FromResult( true );
      }

      protected virtual Task OnDiscardAsync()
      {
         return Task.FromResult( true );
      }
   }
}

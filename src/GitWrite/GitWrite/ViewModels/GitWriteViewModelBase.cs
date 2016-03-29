using System;
using GalaSoft.MvvmLight;

namespace GitWrite.ViewModels
{
   public class GitWriteViewModelBase : ViewModelBase
   {
      public event EventHandler<ShutdownEventArgs> ShutdownRequested;

      protected virtual void OnShutdownRequested( object sender, ShutdownEventArgs e ) => ShutdownRequested?.Invoke( sender, e );
   }
}

using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GitWrite.Services;

namespace GitWrite.ViewModels
{
   public class GitWriteViewModelBase : ViewModelBase
   {
      protected IViewService ViewService
      {
         get;
      }

      protected IAppService AppService
      {
         get;
      }

      public bool IsDirty
      {
         get;
         set;
      }

      private bool _isExiting;
      public bool IsExiting
      {
         get => _isExiting;
         set
         {
            Set( () => IsExiting, ref _isExiting, value );
         }
      }

      private ExitReason _exitReason;
      public ExitReason ExitReason
      {
         get => _exitReason;
         set
         {
            Set( () => ExitReason, ref _exitReason, value );
         }
      }

      public RelayCommand PasteCommand
      {
         get;
         protected internal set;
      }

      public event AsyncEventHandler<ShutdownEventArgs> ShutdownRequested;

      protected virtual async Task OnShutdownRequested( object sender, ShutdownEventArgs e )
      {
         var handler = ShutdownRequested;

         if ( handler != null )
         {
            await handler( sender, e );
         }
      } 

      public GitWriteViewModelBase( IViewService viewService, IAppService appService, IMessenger messenger )
         : base( messenger )
      {
         ViewService = viewService;
         AppService = appService;
      }

      protected Task RaiseAsync( AsyncEventHandler ev, object sender, EventArgs e )
      {
         var handler = ev;

         if ( handler != null )
         {
            return handler( sender, e );
         }

         return Task.CompletedTask;
      }

      protected Task RaiseAsync<T>( AsyncEventHandler<T> ev, object sender, T e ) where T: EventArgs
      {
         var handler = ev;

         if ( handler != null )
         {
            return handler( sender, e );
         }

         return Task.CompletedTask;
      }
   }
}

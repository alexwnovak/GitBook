using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
         get
         {
            return _isExiting;
         }
         set
         {
            Set( () => IsExiting, ref _isExiting, value );
         }
      }

      private ExitReason _exitReason;
      public ExitReason ExitReason
      {
         get
         {
            return _exitReason;
         }
         set
         {
            Set( () => ExitReason, ref _exitReason, value );
         }
      }

      public RelayCommand AbortCommand
      {
         get;
         protected internal set;
      }

      public RelayCommand SaveCommand
      {
         get;
         protected internal set;
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

      public GitWriteViewModelBase( IViewService viewService, IAppService appService )
      {
         ViewService = viewService;
         AppService = appService;

         AbortCommand = new RelayCommand( OnAbort );
         SaveCommand = new RelayCommand( OnSave );
      }

      private async void OnAbort()
      {
         if ( IsExiting )
         {
            return;
         }

         ExitReason = ExitReason.Discard;
         ExitReason exitReason = ExitReason.Discard;

         if ( IsDirty )
         {
            var confirmationResult = ViewService.ConfirmExit();

            if ( confirmationResult == ExitReason.Cancel )
            {
               return;
            }

            if ( confirmationResult == ExitReason.Save )
            {
               ExitReason = ExitReason.Save;

               bool shouldReallyExit = await OnSaveAsync();

               if ( !shouldReallyExit )
               {
                  return;
               }

               exitReason = ExitReason.Save;
            }
            else
            {
               await OnDiscardAsync();
               exitReason = ExitReason.Discard;
               IsExiting = true;
            }
         }

         if ( exitReason == ExitReason.Discard )
         {
            await OnDiscardAsync();
         }

         IsExiting = true;
         await OnShutdownRequested( this, new ShutdownEventArgs( exitReason ) );

         AppService.Shutdown();
      }

      private async void OnSave()
      {
         ExitReason = ExitReason.Save;
         bool shouldContinue = await OnSaveAsync();

         if ( !shouldContinue )
         {
            return;
         }

         IsExiting = true;
         await OnShutdownRequested( this, new ShutdownEventArgs( ExitReason.Save ) );

         AppService.Shutdown();
      }

      protected virtual Task<bool> OnSaveAsync()
      {
         return Task.FromResult( true );
      }

      protected virtual Task<bool> OnDiscardAsync()
      {
         return Task.FromResult( true );
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

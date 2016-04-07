using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitWrite.Services;
using GitWrite.Views;
using GitWrite.Views.Controls;

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

      public bool IsExiting
      {
         get;
         set;
      }

      public bool IsConfirming
      {
         get;
         set;
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
      }

      private async void OnAbort()
      {
         if ( IsExiting || IsConfirming )
         {
            return;
         }

         ExitReason exitReason = ExitReason.Abort;

         if ( IsDirty )
         {
            IsConfirming = true;

            var confirmationResult = await ViewService.ConfirmExitAsync();

            if ( confirmationResult == ConfirmationResult.Cancel )
            {
               IsConfirming = false;
               return;
            }

            if ( confirmationResult == ConfirmationResult.Save )
            {
               await OnSaveAsync();
               exitReason = ExitReason.Accept;
            }
            else
            {
               await OnDiscardAsync();
               exitReason = ExitReason.Abort;
            }
         }

         if ( exitReason == ExitReason.Abort )
         {
            await OnDiscardAsync();
         }

         IsExiting = true;
         await OnShutdownRequested( this, new ShutdownEventArgs( exitReason ) );

         AppService.Shutdown();
      }

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

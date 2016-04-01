using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Services;
using GitWrite.Views;
using GitWrite.Views.Controls;

namespace GitWrite.ViewModels
{
   public class GitWriteViewModelBase : ViewModelBase
   {
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

      protected virtual async Task OnShutdownRequested( object sender, ShutdownEventArgs e ) => await ShutdownRequested?.Invoke( sender, e );

      public GitWriteViewModelBase()
      {
         IsDirty = true;
         AbortCommand = new RelayCommand( OnAbort );
      }

      private async void OnAbort()
      {
         if ( IsExiting || IsConfirming )
         {
            return;
         }

         ExitReason exitReason = ExitReason.AbortCommit;

         if ( IsDirty )
         {
            IsConfirming = true;

            var viewService = SimpleIoc.Default.GetInstance<IViewService>();
            var confirmationResult = await viewService.ConfirmExitAsync();

            if ( confirmationResult == ConfirmationResult.Cancel )
            {
               IsConfirming = false;
               return;
            }

            if ( confirmationResult == ConfirmationResult.Save )
            {
               await OnSaveAsync();
               exitReason = ExitReason.AcceptCommit;
            }
            else
            {
               await OnDiscardAsync();
               exitReason = ExitReason.AbortCommit;
            }
         }

         IsExiting = true;
         await OnShutdownRequested( this, new ShutdownEventArgs( exitReason ) );

         var appService = SimpleIoc.Default.GetInstance<IAppService>();
         appService.Shutdown();
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

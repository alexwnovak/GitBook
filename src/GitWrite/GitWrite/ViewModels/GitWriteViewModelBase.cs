using System;
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

      public event EventHandler<ShutdownEventArgs> ShutdownRequested;

      protected virtual void OnShutdownRequested( object sender, ShutdownEventArgs e ) => ShutdownRequested?.Invoke( sender, e );

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

            ExitReason exitReason;

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

            OnShutdownRequested( this, new ShutdownEventArgs( exitReason ) );
         }
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

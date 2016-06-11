using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitWrite.Views.Controls;

namespace GitWrite.ViewModels
{
   public class ConfirmationViewModel : ViewModelBase
   {
      public RelayCommand SaveCommand
      {
         get;
      }

      public event EventHandler<CloseRequestedEventArgs> CloseRequested; 

      public ConfirmationViewModel()
      {
         SaveCommand = new RelayCommand( () => OnCloseRequested( this, new CloseRequestedEventArgs( ConfirmationResult.Save ) ) );;
      }

      protected virtual void OnCloseRequested( object sender, CloseRequestedEventArgs e )
         => CloseRequested?.Invoke( sender, e );   
   }
}

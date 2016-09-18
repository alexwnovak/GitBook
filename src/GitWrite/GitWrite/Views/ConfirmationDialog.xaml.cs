using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using GitWrite.ViewModels;

namespace GitWrite.Views
{
   public partial class ConfirmationDialog : Window
   {
      private readonly ConfirmationViewModel _viewModel;
      private ExitReason _confirmationResult;
      private bool _hasPlayedExitAnimation;

      public ConfirmationDialog( Window owner )
      {
         InitializeComponent();
         Owner = owner;

         _viewModel = (ConfirmationViewModel) DataContext;
         _viewModel.CloseRequested += OnCloseRequested;
      }

      public new ExitReason ShowDialog()
      {
         base.ShowDialog();

         return _confirmationResult;
      }

      private void OnCloseRequested( object sender, CloseRequestedEventArgs e )
      {
         _confirmationResult = e.ConfirmationResult;

         Close();
      }

      private void DialogHeader_OnMouseDown( object sender, MouseButtonEventArgs e )
      {
         if ( e.LeftButton == MouseButtonState.Pressed )
         {
            DragMove();
         }
      }

      private void ConfirmationDialog_OnKeyDown( object sender, KeyEventArgs e )
      {
         if ( e.Key == Key.Escape )
         {
            OnCloseRequested( this, new CloseRequestedEventArgs( ExitReason.Cancel ) );
         }
      }

      private void ConfirmationDialog_OnClosing( object sender, CancelEventArgs e )
      {
         if ( _hasPlayedExitAnimation )
         {
            return;
         }

         _hasPlayedExitAnimation = true;
         e.Cancel = true;

         var exitStoryboard = (Storyboard) Resources["ExitStoryboard"];

         exitStoryboard.Completed += ( _, __ ) => Close();
         exitStoryboard.Begin();
      }
   }
}

using System.Windows;
using System.Windows.Input;
using GitWrite.ViewModels;
using GitWrite.Views.Controls;

namespace GitWrite.Views
{
   public partial class ConfirmationDialog : Window
   {
      private readonly ConfirmationViewModel _viewModel;
      private ExitReason _confirmationResult;

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
   }
}

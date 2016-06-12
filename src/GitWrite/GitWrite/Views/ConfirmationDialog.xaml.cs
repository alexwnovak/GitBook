using System.Windows;
using GitWrite.ViewModels;
using GitWrite.Views.Controls;

namespace GitWrite.Views
{
   public partial class ConfirmationDialog : Window
   {
      private readonly ConfirmationViewModel _viewModel;
      private ConfirmationResult _confirmationResult;

      public ConfirmationDialog()
      {
         InitializeComponent();

         _viewModel = (ConfirmationViewModel) DataContext;
         _viewModel.CloseRequested += OnCloseRequested;
      }

      public new ConfirmationResult ShowDialog()
      {
         base.ShowDialog();

         return _confirmationResult;
      }

      private void OnCloseRequested( object sender, CloseRequestedEventArgs e )
      {
         _confirmationResult = e.ConfirmationResult;

         Close();
      }
   }
}

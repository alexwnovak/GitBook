using System;
using System.Windows;
using System.Windows.Input;
using GitWrite.ViewModels;

namespace GitWrite.Views
{
   public partial class CommitWindow : WindowBase
   {
      private readonly CommitViewModel _viewModel;

      public double HelpScrollDistance
      {
         get
         {
            return (double) GetValue( HelpScrollDistanceProperty );
         }
         set
         {
            SetValue( HelpScrollDistanceProperty, value );
         }
      }

      public static readonly DependencyProperty HelpScrollDistanceProperty = DependencyProperty.Register( nameof( HelpScrollDistance ),
         typeof( double ),
         typeof( CommitWindow ),
         new PropertyMetadata( 0.0 ) );

      public CommitWindow()
      {
         InitializeComponent();

         _viewModel = (CommitViewModel) DataContext;
         _viewModel.ExpansionRequested += OnExpansionRequested;
         _viewModel.CollapseRequested += OnCollapseRequested;
         _viewModel.HelpRequested += OnHelpRequested;
         _viewModel.CollapseHelpRequested += OnCollapseHelpRequested;
      }

      private void OnHelpRequested( object sender, EventArgs e )
      {
         HelpScrollDistance = -ActualHeight;

         this.PlayStoryboard( "ActivateHelpStoryboard" );
      }

      private void OnCollapseHelpRequested( object sender, EventArgs e )
         => this.PlayStoryboard( "CollapseHelpStoryboard" );

      private void OnExpansionRequested( object sender, EventArgs eventArgs )
         => VisualStateManager.GoToElementState( MainGrid, "Expanded", false );

      private void OnCollapseRequested( object sender, EventArgs eventArgs )
         => VisualStateManager.GoToElementState( MainGrid, "Collapsed", false );

      private void CommitWindow_OnPreviewCanExecute( object sender, CanExecuteRoutedEventArgs e )
      {
         //if ( e.Command == ApplicationCommands.Paste )
         //{
         //   e.CanExecute = false;
         //   e.Handled = true;
         //}
      }
   }
}

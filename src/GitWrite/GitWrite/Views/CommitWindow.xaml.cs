using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using GitWrite;
using GitWrite.ViewModels;
using GitWrite.Views.Controls;

namespace GitWrite.Views
{
   public partial class CommitWindow : Window
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
         _viewModel.AsyncExitRequested += OnAsyncExitRequested;
         _viewModel.ConfirmExitRequested += OnConfirmExitRequested;
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

      private Task OnAsyncExitRequested( object sender, EventArgs e )
      {
         var exitPanel = new ExitPanel
         {
            Height = OuterBorder.ActualHeight,
            VerticalAlignment = VerticalAlignment.Top
         };

         CommitText.SelectionLength = 0;
         SecondaryCommitText.SelectionLength = 0;

         MainGrid.Children.Add( exitPanel );

         return exitPanel.ShowAsync();
      }

      private async Task<ConfirmationResult> OnConfirmExitRequested( object sender, EventArgs e )
      {
         var confirmPanel = new ConfirmPanel
         {
            Height = OuterBorder.ActualHeight,
            VerticalAlignment = VerticalAlignment.Top,
         };

         MainGrid.Children.Add( confirmPanel );

         var previousFocusElement = Keyboard.FocusedElement;
         confirmPanel.Focus();

         var confirmationResult = await confirmPanel.ShowAsync();

         if ( confirmationResult == ConfirmationResult.Cancel )
         {
            MainGrid.Children.Remove( confirmPanel );
            previousFocusElement.Focus();
         }

         return confirmationResult;
      }

      private void OnExpansionRequested( object sender, EventArgs eventArgs )
         => VisualStateManager.GoToElementState( MainGrid, "Expanded", true );

      private void CommitWindow_OnLoaded( object sender, RoutedEventArgs e )
      {
         _viewModel.ViewLoaded();
      }
   }
}

using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using GitWrite.ViewModels;
using GitWrite.Views.Controls;

namespace GitWrite.Views
{
   public partial class CommitWindow : Window
   {
      private readonly CommitViewModel _viewModel;

      public CommitWindow()
      {
         InitializeComponent();

         _viewModel = (CommitViewModel) DataContext;
         _viewModel.ExpansionRequested += OnExpansionRequested;
         _viewModel.AsyncExitRequested += OnAsyncExitRequested;
         _viewModel.HelpRequested += OnHelpRequested;
      }

      private void OnHelpRequested( object sender, EventArgs e )
      {
      }

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

      private void OnExpansionRequested( object sender, EventArgs eventArgs )
         => (Resources["ExpandedStateStoryboard"] as Storyboard)?.Begin();

      private void CommitWindow_OnLoaded( object sender, RoutedEventArgs e )
      {
         Left = ( SystemParameters.FullPrimaryScreenWidth - Width) / 2;

         if ( !string.IsNullOrEmpty( _viewModel.ShortMessage ) )
         {
            CommitText.SelectionStart = CommitText.Text.Length;
         }

         ( Resources["WindowEntranceStoryboard"] as Storyboard )?.Begin();

         _viewModel.ViewLoaded();
      }
   }
}

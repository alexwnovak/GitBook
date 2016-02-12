using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using GitWrite.ViewModels;

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
      }

      private Task OnAsyncExitRequested( object sender, EventArgs e )
         => ExitPanel.ShowAsync();

      private void OnExpansionRequested( object sender, EventArgs eventArgs )
         => (Resources["ExpandedStateStoryboard"] as Storyboard)?.Begin();

      private void CommitWindow_OnLoaded( object sender, RoutedEventArgs e )
      {
         double screenWidth = SystemParameters.FullPrimaryScreenWidth;
         double screenHeight = SystemParameters.FullPrimaryScreenHeight;

         Left = (screenWidth - Width) / 2;
         Top = 0.7 * (screenHeight - Height) / 2;

         if ( !string.IsNullOrEmpty( _viewModel.ShortMessage ) )
         {
            CommitText.SelectionStart = CommitText.Text.Length;
         }
      }
   }
}

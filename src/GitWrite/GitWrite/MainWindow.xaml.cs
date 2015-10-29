using System;
using System.Windows;
using System.Windows.Media.Animation;
using GitWrite.ViewModels;

namespace GitWrite
{
   public partial class MainWindow : Window
   {
      private MainViewModel _viewModel;

      public MainWindow()
      {
         InitializeComponent();
         Height = 75;

         _viewModel = (MainViewModel) DataContext;
         _viewModel.ExpansionRequested += OnExpansionRequested;
      }

      private void OnExpansionRequested( object sender, EventArgs eventArgs )
         => (Resources["ExpandedStateStoryboard"] as Storyboard)?.Begin();

      private void MainWindow_OnLoaded( object sender, RoutedEventArgs e )
      {
         double screenWidth = SystemParameters.FullPrimaryScreenWidth;
         double screenHeight = SystemParameters.FullPrimaryScreenHeight;

         Left = ( screenWidth - Width ) / 2;
         Top = 0.7 * ( screenHeight - Height ) / 2;
      }
   }
}

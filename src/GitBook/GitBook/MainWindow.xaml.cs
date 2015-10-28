using System.Windows;

namespace GitBook
{
   public partial class MainWindow : Window
   {
      public MainWindow()
      {
         InitializeComponent();
         Height = 75;
      }

      private void MainWindow_OnLoaded( object sender, RoutedEventArgs e )
      {
         double screenWidth = SystemParameters.FullPrimaryScreenWidth;
         double screenHeight = SystemParameters.FullPrimaryScreenHeight;

         Left = ( screenWidth - Width ) / 2;
         Top = 0.7 * ( screenHeight - Height ) / 2;
      }
   }
}

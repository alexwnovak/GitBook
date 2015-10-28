using System.Windows;
using System.Windows.Media.Animation;

namespace GitWrite.Services
{
   public class AppService : IAppService
   {
      public void BeginStoryboard( string storyboardName )
      {
         var storyboard = (Storyboard) Application.Current.MainWindow.Resources[storyboardName];

         storyboard.Begin();
      }

      public MessageBoxResult DisplayMessageBox( string message, MessageBoxButton buttons )
      {
         return MessageBox.Show( message, null, buttons );
      }

      public void Shutdown()
      {
         Application.Current.Shutdown();
      }
   }
}

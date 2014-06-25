using System.Windows;

namespace GitBook.Service
{
   public class AppService : IAppService
   {
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

using System.Windows;

namespace GitBook.Service
{
   public class AppService : IAppService
   {
      public MessageBoxResult DisplayMessageBox( string message )
      {
         return MessageBox.Show( message );
      }

      public void Shutdown()
      {
         Application.Current.Shutdown();
      }
   }
}

using System.Windows;

namespace GitWrite.Services
{
   public class AppService : IAppService
   {
      public MessageBoxResult DisplayMessageBox( string message, MessageBoxButton buttons )
      {
         return MessageBox.Show( message, null, buttons );
      }
   }
}

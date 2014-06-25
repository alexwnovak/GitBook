using System.Windows;

namespace GitBook.Service
{
   public class AppService : IAppService
   {
      public MessageBoxResult DisplayMessageBox( string message )
      {
         throw new System.NotImplementedException();
      }

      public void Shutdown()
      {
         Application.Current.Shutdown();
      }
   }
}

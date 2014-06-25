using System.Windows;

namespace GitBook.Service
{
   public class AppService : IAppService
   {
      public void Shutdown()
      {
         Application.Current.Shutdown();
      }
   }
}

using System.Windows;

namespace GitBook.Service
{
   public interface IAppService
   {
      MessageBoxResult DisplayMessageBox( string message, MessageBoxButton buttons );

      void Shutdown();
   }
}

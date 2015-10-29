using System.Windows;

namespace GitWrite.Services
{
   public interface IAppService
   {
      MessageBoxResult DisplayMessageBox( string message, MessageBoxButton buttons );

      void Shutdown();
   }
}

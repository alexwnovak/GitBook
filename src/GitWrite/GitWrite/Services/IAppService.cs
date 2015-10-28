using System.Windows;

namespace GitWrite.Services
{
   public interface IAppService
   {
      void BeginStoryboard( string storyboardName );

      MessageBoxResult DisplayMessageBox( string message, MessageBoxButton buttons );

      void Shutdown();
   }
}

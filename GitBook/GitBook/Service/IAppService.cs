using System.Windows;

namespace GitBook.Service
{
   public interface IAppService
   {
      void BeginStoryboard( string storyboardName );

      MessageBoxResult DisplayMessageBox( string message, MessageBoxButton buttons );

      void Shutdown();
   }
}

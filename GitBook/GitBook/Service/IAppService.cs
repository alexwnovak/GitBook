using System.Windows;

namespace GitBook.Service
{
   public interface IAppService
   {
      MessageBoxResult DisplayMessageBox( string message );

      void Shutdown();
   }
}

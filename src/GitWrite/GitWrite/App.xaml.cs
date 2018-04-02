using System.Windows;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace GitWrite
{
   public partial class App : Application
   {
      private void Application_OnStartup( object sender, StartupEventArgs e )
      {
         ServiceLocator.SetLocatorProvider( () => SimpleIoc.Default );
         StartupUri = new CommitController( e.Args[0] ).GetStartupUri();
      }
   }
}

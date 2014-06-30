using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using GitBook.Service;
using GitBook.ViewModel;
using Microsoft.Practices.ServiceLocation;

namespace GitBook
{
   public partial class App : Application
   {
      private void App_OnStartup( object sender, StartupEventArgs e )
      {
         ServiceLocator.SetLocatorProvider( () => SimpleIoc.Default );

         SimpleIoc.Default.Register<MainViewModel>();
         SimpleIoc.Default.Register<IAppService, AppService>();
         SimpleIoc.Default.Register<IEnvironmentAdapter, EnvironmentAdapter>();
         SimpleIoc.Default.Register<ICommitFileReader, CommitFileReader>();

         // Load the commit file

         var appController = new AppController();

         appController.Start( e.Args );
      }
   }
}

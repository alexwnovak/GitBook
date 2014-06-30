using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using GitBook.Service;
using GitBook.ViewModel;
using Microsoft.Practices.ServiceLocation;

namespace GitBook
{
   public partial class App : Application
   {
      public static ICommitDocument CommitDocument
      {
         get;
         set;
      }

      private void App_OnStartup( object sender, StartupEventArgs e )
      {
         ServiceLocator.SetLocatorProvider( () => SimpleIoc.Default );

         SimpleIoc.Default.Register<MainViewModel>();
         SimpleIoc.Default.Register<IAppService, AppService>();
         SimpleIoc.Default.Register<IEnvironmentAdapter, EnvironmentAdapter>();
         SimpleIoc.Default.Register<ICommitFileReader, CommitFileReader>();
         SimpleIoc.Default.Register<IFileAdapter, FileAdapter>();

         // Load the commit file

         var appController = new AppController();

         appController.Start( e.Args );
      }
   }
}

using System;
using System.Windows;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Services;
using GitWrite.ViewModels;

namespace GitWrite
{
   public partial class App : Application
   {
      public static ICommitDocument CommitDocument
      {
         get;
         set;
      }

      private void Application_OnStartup( object sender, StartupEventArgs e )
      {
         ServiceLocator.SetLocatorProvider( () => SimpleIoc.Default );

         SimpleIoc.Default.Register<CommitViewModel>();
         SimpleIoc.Default.Register<IAppService, AppService>();
         SimpleIoc.Default.Register<IEnvironmentAdapter, EnvironmentAdapter>();
         SimpleIoc.Default.Register<ICommitFileReader, CommitFileReader>();
         SimpleIoc.Default.Register<IFileAdapter, FileAdapter>();

         StartupUri = new Uri( "MainWindow.xaml", UriKind.Relative );

         // Load the commit file

         var appController = new AppController();

         appController.Start( e.Args );
      }
   }
}

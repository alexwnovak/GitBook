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

         // Load the commit file

         var appController = new AppController();

         appController.Start( e.Args );

         // Set the startup UI and we're off

         StartupUri = GetStartupWindow( appController.ApplicationMode );
      }

      private static Uri GetStartupWindow( ApplicationMode applicationMode )
      {
         switch ( applicationMode )
         {
            case ApplicationMode.Commit:
               return new Uri( @"Views\CommitWindow.xaml", UriKind.Relative );
            case ApplicationMode.InteractiveRebase:
               return new Uri( @"Views\InteractiveRebaseWindow.xaml", UriKind.Relative );
         }

         throw new ArgumentException( $"Unknown application mode: {applicationMode}", nameof( applicationMode ) );
      }
   }
}

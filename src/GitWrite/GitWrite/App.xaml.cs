using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Services;
using GitWrite.Themes;
using GitWrite.ViewModels;
using GitWrite.Views;

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
         InitializeDependencies();
         InitializeTheme();

         // Load the commit file

         var appController = new AppController( new EnvironmentAdapter(), new CommitFileReader( new FileAdapter() ) );

         appController.Start( e.Args );

         // Set the startup UI and we're off

         switch ( appController.ApplicationMode )
         {
            case ApplicationMode.InteractiveRebase:
            case ApplicationMode.EditPatch:
            case ApplicationMode.Unknown:
               PassThrough( e.Args );
               Shutdown();
               return;
         }

         StartupUri = GetStartupWindow( appController.ApplicationMode );
      }

      private void InitializeDependencies()
      {
         ServiceLocator.SetLocatorProvider( () => SimpleIoc.Default );
         SimpleIoc.Default.Register<CommitViewModel>();
         SimpleIoc.Default.Register<InteractiveRebaseViewModel>();
         SimpleIoc.Default.Register<IRegistryService, RegistryService>();
         SimpleIoc.Default.Register<IApplicationSettings>( () => new ApplicationSettings( new RegistryService() ) );
         SimpleIoc.Default.Register<IAppService, AppService>();
         SimpleIoc.Default.Register<IClipboardService, ClipboardService>();
         SimpleIoc.Default.Register<IEnvironmentAdapter, EnvironmentAdapter>();
         SimpleIoc.Default.Register<ICommitFileReader>( () => new CommitFileReader( new FileAdapter() ) );
         SimpleIoc.Default.Register<IFileAdapter, FileAdapter>();
         SimpleIoc.Default.Register<IStoryboardHelper, StoryboardHelper>();
      }

      private void InitializeTheme()
      {
         ThemeSwitcher.Initialize();

         var appSettings = SimpleIoc.Default.GetInstance<IApplicationSettings>();
         ThemeSwitcher.SwitchTo( appSettings.Theme );
      }

      private void PassThrough( string[] arguments )
      {
         const string notepadPlusPlusPath = @"C:\Program Files (x86)\Notepad++\notepad++.exe";
         string argumentLine = "-multiInst -notabbar -nosession -noPlugin " + arguments.Aggregate( ( i, j ) => $"{i} {j}" );

         var startInfo = new ProcessStartInfo( notepadPlusPlusPath, argumentLine );

         Process.Start( startInfo )?.WaitForExit();
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

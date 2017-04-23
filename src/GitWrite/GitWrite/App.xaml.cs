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
using GitWrite.Views.Controls;
using Resx = GitWrite.Properties.Resources;

namespace GitWrite
{
   public partial class App : Application
   {
      private void Application_OnStartup( object sender, StartupEventArgs e )
      {
         InitializeDependencies();
         InitializeTheme();

         var appController = SimpleIoc.Default.GetInstance<AppController>();
         var applicationMode = appController.Start( e.Args );

         EnsureFileExists( e.Args[0] );

         switch ( applicationMode )
         {
            case ApplicationMode.Commit:
            {
               CommitPath( e.Args[0] );
               break;
            }
            case ApplicationMode.Rebase:
            {
               RebasePath( e.Args[0] );
               break;
            }
            case ApplicationMode.EditPatch:
            case ApplicationMode.Unknown:
               PassThrough( e.Args );
               Shutdown();
               return;
         }

         StartupUri = GetStartupWindow( appController.ApplicationMode );
      }

      private void EnsureFileExists( string fileName )
      {
         var fileAdapter = SimpleIoc.Default.GetInstance<IFileAdapter>();

         if ( !fileAdapter.Exists( fileName ) || fileAdapter.GetFileSize( fileName ) <= 0 )
         {
            string title = Resx.FileLoadErrorTitle;
            string message = $"{Resx.FileLoadErrorMessage}{Environment.NewLine}{fileName}";

            var dialog = new StyledDialog();
            dialog.ShowDialog( title, message, DialogButtons.OK, w =>
            {
               w.ShowInTaskbar = true;
               w.Title = Resx.ApplicationName;
            } );

            var environmentService = new EnvironmentAdapter();
            environmentService.Exit( 1 );
         }
      }

      private void CommitPath( string fileName )
      {
         CommitDocument commitDocument = null;

         try
         {
            var commitDocumentReader = SimpleIoc.Default.GetInstance<ICommitFileReader>();
            commitDocument = commitDocumentReader.FromFile( fileName );
         }
         catch ( GitFileLoadException )
         {
            Shutdown();
         }

         SimpleIoc.Default.Register<ICommitDocument>( () => commitDocument );
         SimpleIoc.Default.Register<IGitService>( () => new GitService( null ) );
      }

      private void RebasePath( string fileName )
      {
         RebaseDocument document = null;

         try
         {
            var documentReader = SimpleIoc.Default.GetInstance<RebaseFileReader>();
            document = documentReader.FromFile( fileName );
         }
         catch ( GitFileLoadException )
         {
            Shutdown();
         }

         SimpleIoc.Default.Register( () => document );
      }

      private void InitializeDependencies()
      {
         ServiceLocator.SetLocatorProvider( () => SimpleIoc.Default );

         SimpleIoc.Default.Register<IApplicationSettings, ApplicationSettings>();
         SimpleIoc.Default.Register<ICommitFileReader, CommitFileReader>();
         SimpleIoc.Default.Register<IRebaseFileWriter, RebaseFileWriter>();
         SimpleIoc.Default.Register<IStoryboardHelper, StoryboardHelper>();
         SimpleIoc.Default.Register<IFileAdapter, FileAdapter>();
         SimpleIoc.Default.Register<IAppService, AppService>();
         SimpleIoc.Default.Register<IClipboardService, ClipboardService>();
         SimpleIoc.Default.Register<IRegistryService, RegistryService>();
         SimpleIoc.Default.Register<IEnvironmentAdapter, EnvironmentAdapter>();

         SimpleIoc.Default.Register<RebaseFileReader>();

         SimpleIoc.Default.Register<AppController>();
         SimpleIoc.Default.Register<CommitViewModel>();
         SimpleIoc.Default.Register<RebaseViewModel>();
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
            case ApplicationMode.Rebase:
               return new Uri( @"Views\RebaseWindow.xaml", UriKind.Relative );
         }

         throw new ArgumentException( $"Unknown application mode: {applicationMode}", nameof( applicationMode ) );
      }
   }
}

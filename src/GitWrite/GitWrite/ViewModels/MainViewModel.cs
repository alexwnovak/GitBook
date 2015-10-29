using System;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Resources;
using GitWrite.Services;

namespace GitWrite.ViewModels
{
   public class MainViewModel : ViewModelBase
   {
      public RelayCommand<KeyEventArgs> CommitNotesKeyDownCommand
      {
         get;
      }

      public string CommitText
      {
         get;
         set;
      }

      public string ExtraCommitText
      {
         get;
         set;
      }

      private bool _hasActivatedExpandedState;

      public event EventHandler ExpansionRequested;
       
      public MainViewModel()
      {
         CommitNotesKeyDownCommand = new RelayCommand<KeyEventArgs>( OnCommitNotesKeyDown );
      }

      protected virtual void OnExpansionRequested( object sender, EventArgs e ) => ExpansionRequested?.Invoke( sender, e );

      public void OnCommitNotesKeyDown( KeyEventArgs e )
      {
         switch ( e.Key )
         {
            case Key.Enter:
               SaveCommit();
               break;
            case Key.Escape:
               CancelCommit();
               break;
            case Key.Tab:
               ExpandUI();
               break;
            case Key.F1:
               ShowHelp();
               break;
         }
      }

      private void SaveCommit()
      {
         App.CommitDocument.ShortMessage = CommitText;
         App.CommitDocument.LongMessage = ExtraCommitText;

         App.CommitDocument.Save();

         var environmentAdapter = SimpleIoc.Default.GetInstance<IAppService>();

         environmentAdapter.Shutdown();
      }

      private void CancelCommit()
      {
         var appService = SimpleIoc.Default.GetInstance<IAppService>();

         if ( string.IsNullOrEmpty( CommitText ) )
         {
            appService.Shutdown();
         }
         else
         {
            var result = appService.DisplayMessageBox( Strings.ConfirmDiscardMessage, MessageBoxButton.YesNo );

            if ( result == MessageBoxResult.Yes )
            {
               appService.Shutdown();
            }
         }
      }

      private void ExpandUI()
      {
         if ( !_hasActivatedExpandedState )
         {
            _hasActivatedExpandedState = true;
            OnExpansionRequested( this, EventArgs.Empty );
         }
      }

      private void ShowHelp()
      {
         var appService = SimpleIoc.Default.GetInstance<IAppService>();

         appService.BeginStoryboard( "HelpControlVisibleStoryboard" );
      }
   }
}
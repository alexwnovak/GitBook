using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GitBook.Resources;
using GitBook.Service;

namespace GitBook.ViewModel
{
   public class MainViewModel : ViewModelBase
   {
      public RelayCommand<KeyEventArgs> CommitNotesKeyDownCommand
      {
         get;
         private set;
      }

      public string CommitText
      {
         get;
         set;
      }

      private bool _hasActivatedExpandedState;
       
      public MainViewModel()
      {
         CommitNotesKeyDownCommand = new RelayCommand<KeyEventArgs>( OnCommitNotesKeyDown );
      }

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
         }
      }

      private void SaveCommit()
      {
         App.CommitDocument.ShortMessage = CommitText;

         App.CommitDocument.Save();
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

            var appService = SimpleIoc.Default.GetInstance<IAppService>();

            appService.BeginStoryboard( "ExpandedWindowStoryboard" );
            appService.BeginStoryboard( "ExpandedGridStoryboard" );
         }
      }
   }
}
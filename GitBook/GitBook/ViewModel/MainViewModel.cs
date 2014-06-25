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
       
      public MainViewModel()
      {
         CommitNotesKeyDownCommand = new RelayCommand<KeyEventArgs>( OnCommitNotesKeyDown );
      }

      public void OnCommitNotesKeyDown( KeyEventArgs e )
      {
         if ( e.Key == Key.Escape )
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
      }
   }
}
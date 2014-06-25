using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace GitBook.ViewModel
{
   public class MainViewModel : ViewModelBase
   {
      public RelayCommand<KeyEventArgs> CommitNotesKeyDownCommand
      {
         get;
         private set;
      }
       
      public MainViewModel()
      {
         CommitNotesKeyDownCommand = new RelayCommand<KeyEventArgs>( OnCommitNotesKeyDown );
      }

      public void OnCommitNotesKeyDown( KeyEventArgs e )
      {
      }
   }
}
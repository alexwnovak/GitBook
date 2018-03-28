using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitModel;

namespace GitWrite.ViewModels
{
   public class CommitViewModel : ViewModelBase
   {
      public string CommitFilePath { get; }
      public CommitDocument CommitDocument { get; }

      public RelayCommand AcceptCommand { get; }
      public RelayCommand DiscardCommand { get; }
      public RelayCommand SettingsCommand { get; }

      public CommitViewModel( string commitFilePath,
         CommitDocument commitDocument )
      {
         CommitFilePath = commitFilePath;
         CommitDocument = commitDocument;

         AcceptCommand = new RelayCommand( OnAcceptCommand );
         DiscardCommand = new RelayCommand( OnDiscardCommand );
         SettingsCommand = new RelayCommand( OnSettingsCommand );
      }

      private void OnAcceptCommand()
      {
      }

      private void OnDiscardCommand()
      {
      }

      private void OnSettingsCommand()
      {
      }
   }
}

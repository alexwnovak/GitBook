using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitModel;

namespace GitWrite.ViewModels
{
   public class CommitViewModel : ViewModelBase
   {
      public string CommitFilePath { get; }
      public CommitDocument CommitDocument { get; }

      private readonly ICommitFileWriter _commitFileWriter;

      public RelayCommand AcceptCommand { get; }
      public RelayCommand DiscardCommand { get; }
      public RelayCommand SettingsCommand { get; }

      public CommitViewModel( string commitFilePath,
         CommitDocument commitDocument,
         ICommitFileWriter commitFileWriter )
      {
         CommitFilePath = commitFilePath;
         CommitDocument = commitDocument;

         _commitFileWriter = commitFileWriter;

         AcceptCommand = new RelayCommand( OnAcceptCommand );
         DiscardCommand = new RelayCommand( OnDiscardCommand );
         SettingsCommand = new RelayCommand( OnSettingsCommand );
      }

      private void OnAcceptCommand()
      {
         _commitFileWriter.ToFile( CommitFilePath, CommitDocument );
      }

      private void OnDiscardCommand()
      {
      }

      private void OnSettingsCommand()
      {
      }
   }
}

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitModel;
using GitWrite.Services;

namespace GitWrite.ViewModels
{
   public class CommitViewModel : ViewModelBase
   {
      public string CommitFilePath { get; }
      public CommitDocument CommitDocument { get; }

      private readonly ICommitFileWriter _commitFileWriter;
      private readonly IViewService _viewService;

      public RelayCommand AcceptCommand { get; }
      public RelayCommand DiscardCommand { get; }
      public RelayCommand SettingsCommand { get; }

      public CommitViewModel( string commitFilePath,
         CommitDocument commitDocument,
         ICommitFileWriter commitFileWriter,
         IViewService viewService )
      {
         CommitFilePath = commitFilePath;
         CommitDocument = commitDocument;

         _commitFileWriter = commitFileWriter;
         _viewService = viewService;

         AcceptCommand = new RelayCommand( OnAcceptCommand );
         DiscardCommand = new RelayCommand( OnDiscardCommand );
         SettingsCommand = new RelayCommand( OnSettingsCommand );
      }

      private async void OnAcceptCommand()
      {
         if ( string.IsNullOrWhiteSpace( CommitDocument.Subject ) )
         {
            _viewService.DisplaySubjectHint();
         }
         else
         {
            _commitFileWriter.ToFile( CommitFilePath, CommitDocument );
            await _viewService.CloseViewAsync( true );
         }
      }

      private async void OnDiscardCommand()
      {
         _commitFileWriter.ToFile( CommitFilePath, CommitDocument.Empty );
         await _viewService.CloseViewAsync( false );
      }

      private void OnSettingsCommand()
      {
      }
   }
}

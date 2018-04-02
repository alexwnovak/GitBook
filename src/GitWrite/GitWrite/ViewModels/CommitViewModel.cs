using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GitModel;
using GitWrite.Models;
using GitWrite.Services;

namespace GitWrite.ViewModels
{
   public class CommitViewModel : ViewModelBase
   {
      public string CommitFilePath { get; }
      public bool IsDirty { get; set; }

      private readonly ICommitFileReader _commitFileReader;
      private readonly ICommitFileWriter _commitFileWriter;
      private readonly IViewService _viewService;

      private CommitModel _commitModel;
      public CommitModel CommitModel
      {
         get => _commitModel;
         private set => Set( () => CommitModel, ref _commitModel, value );
      }

      public RelayCommand InitializeCommand { get; }
      public RelayCommand AcceptCommand { get; }
      public RelayCommand DiscardCommand { get; }
      public RelayCommand SettingsCommand { get; }

      public CommitViewModel( string commitFilePath,
         ICommitFileReader commitFileReader,
         ICommitFileWriter commitFileWriter,
         IViewService viewService )
      {
         CommitFilePath = commitFilePath;

         _commitFileReader = commitFileReader;
         _commitFileWriter = commitFileWriter;
         _viewService = viewService;

         InitializeCommand = new RelayCommand( OnInitializeCommand );
         AcceptCommand = new RelayCommand( OnAcceptCommand );
         DiscardCommand = new RelayCommand( OnDiscardCommand );
         SettingsCommand = new RelayCommand( OnSettingsCommand );
      }

      private void OnInitializeCommand()
      {
         var commitDocument = _commitFileReader.FromFile( CommitFilePath );

         CommitModel = new CommitModel
         {
            Subject = commitDocument.Subject,
            Body = commitDocument.Body
         };
      }

      private async void OnAcceptCommand()
      {
         if ( string.IsNullOrWhiteSpace( CommitModel.Subject ) )
         {
            _viewService.DisplaySubjectHint();
         }
         else
         {
            var commitDocument = new CommitDocument( CommitModel.Subject, CommitModel.Body );
            _commitFileWriter.ToFile( CommitFilePath, commitDocument );
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

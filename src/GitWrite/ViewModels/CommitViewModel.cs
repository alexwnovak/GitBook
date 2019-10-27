using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using GitModel;
using GitWrite.Models;

namespace GitWrite.ViewModels
{
   public class CommitViewModel : Screen
   {
      private Action<CommitDocument> _writeCommitFile;
      private Func<CommitDocument> _getPendingDocument = () => CommitDocument.Empty;

      private CommitModel _commit;
      public CommitModel Commit
      {
         get => _commit;
         set
         {
            _commit = value;
            NotifyOfPropertyChange( nameof( Commit ) );
         }
      }

      public CommitViewModel(
         GetCommitFilePathFunction getCommitFilePath,
         ReadCommitFileFunction readCommitFile,
         WriteCommitFileFunction writeCommitFile )
      {
         _writeCommitFile = d => writeCommitFile( getCommitFilePath(), d );

         string filePath = getCommitFilePath();
         var commitDocument = readCommitFile( filePath );

         Commit = new CommitModel
         {
            Subject = commitDocument.Subject,
            Body = commitDocument.Body.Aggregate( ( acc, line ) => acc += $"{Environment.NewLine}{line}" )
         };
      }

      public async Task Save()
      {
         _getPendingDocument = () => new CommitDocument
         {
            Subject = Commit.Subject,
            Body = Commit.Body.Split( Environment.NewLine )
         };

         await TryCloseAsync();
      }

      public async Task Discard()
      {
         await TryCloseAsync();
      }

      protected override Task OnDeactivateAsync( bool close, CancellationToken cancellationToken )
      {
         if ( close )
         {
            var document = _getPendingDocument();
            _writeCommitFile( document );
         }

         return Task.CompletedTask;
      }
   }
}

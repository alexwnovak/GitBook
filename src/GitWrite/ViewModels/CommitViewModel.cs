using System;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using GitModel;
using GitWrite.Models;
using Action = System.Action;

namespace GitWrite.ViewModels
{
   public class CommitViewModel : Screen
   {
      private Action<CommitDocument> _writeCommitFile;

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
         var commitDocument = new CommitDocument
         {
            Subject = Commit.Subject,
            Body = Commit.Body.Split( Environment.NewLine )
         };

         _writeCommitFile( commitDocument );

         await TryCloseAsync();
      }
   }
}

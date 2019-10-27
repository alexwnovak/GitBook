using System;
using System.Linq;
using Caliburn.Micro;
using GitWrite.Models;

namespace GitWrite.ViewModels
{
   public class CommitViewModel : Screen
   {
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
         ReadCommitFileFunction readCommitFile )
      {
         string filePath = getCommitFilePath();
         var commitDocument = readCommitFile( filePath );

         Commit = new CommitModel
         {
            Subject = commitDocument.Subject,
            Body = commitDocument.Body.Aggregate( ( acc, line ) => acc += $"{Environment.NewLine}{line}" )
         };
      }

      public void Save()
      {
      }
   }
}

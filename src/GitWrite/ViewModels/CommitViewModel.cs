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
      private readonly Action<CommitDocument> _writeCommitFile;
      private readonly ConfirmExitFunction _confirmExit;

      private bool _isDirty;
      private CloseAction _closeAction;

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
         WriteCommitFileFunction writeCommitFile,
         ConfirmExitFunction confirmExit )
      {
         _writeCommitFile = d => writeCommitFile( getCommitFilePath(), d );
         _confirmExit = confirmExit;

         string filePath = getCommitFilePath();
         var commitDocument = readCommitFile( filePath );

         Commit = new CommitModel
         {
            Subject = commitDocument.Subject,
            Body = commitDocument.Body.Aggregate( ( acc, line ) => acc += $"{Environment.NewLine}{line}" )
         };

         Commit.PropertyChanged += ( _, __ ) => _isDirty = true;
      }

      public async Task Save()
      {
         _closeAction = CloseAction.Save;
         await TryCloseAsync();
      }

      public async Task Discard()
      {
         _closeAction = CloseAction.Discard;
         await TryCloseAsync();
      }

      public override Task<bool> CanCloseAsync( CancellationToken cancellationToken )
      {
         bool canClose = true;

         if ( _isDirty )
         {
            var confirmResult = _confirmExit();
            canClose = confirmResult != ConfirmResult.Cancel;

            if ( confirmResult == ConfirmResult.Yes )
            {
               _closeAction = CloseAction.Save;
            }
            else
            {
               _closeAction =  CloseAction.Discard;
            }
         }

         return Task.FromResult( canClose );
      }

      protected override Task OnDeactivateAsync( bool close, CancellationToken cancellationToken )
      {
         if ( close )
         {
            CommitDocument document = CommitDocument.Empty;

            if ( _closeAction == CloseAction.Save )
            {
               document = new CommitDocument
               {
                  Subject = Commit.Subject,
                  Body = Commit.Body.Split( Environment.NewLine )
               };
            }

            _writeCommitFile( document );
         }

         return Task.CompletedTask;
      }
   }
}

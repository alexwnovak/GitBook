using System;
using Caliburn.Micro;
using GitWrite.Models;

namespace GitWrite.ViewModels
{
   public class ShellViewModel : Screen
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

      public ShellViewModel()
      {
         Commit = new CommitModel
         {
            Subject = "Subject text",
            Body = $"One{Environment.NewLine}Two"
         };
      }

      public void Save()
      {
      }
   }
}

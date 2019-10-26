﻿using System;
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

      public CommitViewModel()
      {
         Commit = new CommitModel
         {
            Subject = "is simply dummy text of the printinindustry. Lorem fud",
            Body = $"One{Environment.NewLine}Two"
         };
      }

      public void Save()
      {
      }
   }
}

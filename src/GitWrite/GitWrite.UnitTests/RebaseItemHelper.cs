using GitWrite.ViewModels;

namespace GitWrite.UnitTests
{
   public static class RebaseItemHelper
   {
      public static RebaseItem Create( RebaseItemAction action, string commitHash, string text )
         => new RebaseItem( text )
         {
            Action = action,
            CommitHash = commitHash
         };
   }
}

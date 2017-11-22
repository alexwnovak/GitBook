using GitModel;

namespace GitWrite.UnitTests
{
   public static class RebaseItemHelper
   {
      public static RebaseItem Create( RebaseAction action, string commitHash, string text ) =>
         new RebaseItem
         {
            Action = action,
            CommitHash = commitHash,
            Subject = text
         };
   }
}

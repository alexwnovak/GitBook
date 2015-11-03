using System;
using GitWrite.Resources;
using GitWrite.Views;

namespace GitWrite
{
   public static class HelpTextProvider
   {
      public static string GetTextForCommitState( CommitControlState state )
      {
         switch ( state )
         {
            case CommitControlState.EditingPrimaryMessage:
               return Strings.CommitEditingPrimaryMessageText;
            case CommitControlState.EditingSecondaryNotes:
               return Strings.CommitEditingSecondaryNotesText;
         }

         throw new ArgumentException( $"Unknown state: {state}", nameof( state ) );
      }
   }
}

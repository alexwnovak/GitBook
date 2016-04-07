using FluentAssertions;
using GitWrite.Resources;
using GitWrite.Views;
using Xunit;

namespace GitWrite.UnitTests
{
   public class HelpTextProviderTests
   {
      [Fact]
      public void GetTextForCommitState_EditingPrimaryMessage_ReturnsCorrespondingHelpText()
      {
         string helpText = HelpTextProvider.GetTextForCommitState( CommitControlState.EditingPrimaryMessage );

         helpText.Should().Be( Strings.CommitEditingPrimaryMessageText );
      }

      [Fact]
      public void GetTextForCommitState_EditingSecondaryNotes_ReturnsCorrespondingHelpText()
      {
         string helpText = HelpTextProvider.GetTextForCommitState( CommitControlState.EditingSecondaryNotes );

         helpText.Should().Be( Strings.CommitEditingSecondaryNotesText );
      }
   }
}

using GitWrite.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GitWrite.UnitTests
{
   [TestClass]
   public class HelpTextProviderTests
   {
      [TestMethod]
      public void GetTextForCommitState_EditingPrimaryMessage_ReturnsCorrespondingHelpText()
      {
         string helpText = HelpTextProvider.GetTextForCommitState( CommitControlState.EditingPrimaryMessage );

         Assert.AreEqual( Strings.CommitEditingPrimaryMessageText, helpText );
      }

      [TestMethod]
      public void GetTextForCommitState_EditingSecondaryNotes_ReturnsCorrespondingHelpText()
      {
         string helpText = HelpTextProvider.GetTextForCommitState( CommitControlState.EditingSecondaryNotes );

         Assert.AreEqual( Strings.CommitEditingSecondaryNotesText, helpText );
      }
   }
}

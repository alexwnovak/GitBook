using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GitWrite.UnitTests
{
   [TestClass]
   public class ApplicationModeInterpreterTests
   {
      [TestMethod]
      public void GetFromFileName_NullFileName_ReturnsUnknown()
      {
         var applicationMode = ApplicationModeInterpreter.GetFromFileName( null );

         Assert.AreEqual( ApplicationMode.Unknown, applicationMode );
      }

      [TestMethod]
      public void GetFromFileName_EmptyStringFileName_ReturnsUnknown()
      {
         var applicationMode = ApplicationModeInterpreter.GetFromFileName( string.Empty );

         Assert.AreEqual( ApplicationMode.Unknown, applicationMode );
      }

      [TestMethod]
      public void GetFromFileName_FileNameIsGibberish_ReturnsUnknown()
      {
         var applicationMode = ApplicationModeInterpreter.GetFromFileName( "AnUnknownFile.txt" );

         Assert.AreEqual( ApplicationMode.Unknown, applicationMode );
      }

      [TestMethod]
      public void GetFromFileName_PassingCommitFileName_ReturnsCommitMode()
      {
         var applicationMode = ApplicationModeInterpreter.GetFromFileName( GitFileNames.CommitFileName );

         Assert.AreEqual( ApplicationMode.Commit, applicationMode );
      }

      [TestMethod]
      public void GetFromFileName_PassingRebaseFileName_ReturnsRebaseMode()
      {
         var applicationMode = ApplicationModeInterpreter.GetFromFileName( GitFileNames.InteractiveRebaseFileName );

         Assert.AreEqual( ApplicationMode.InteractiveRebase, applicationMode );
      }

      [TestMethod]
      public void GetFromFileName_PassingPatchFileName_ReturnsPatchMode()
      {
         var applicationMode = ApplicationModeInterpreter.GetFromFileName( GitFileNames.EditPatchFileName );

         Assert.AreEqual( ApplicationMode.EditPatch, applicationMode );
      }
   }
}

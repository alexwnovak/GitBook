using System;
using System.Linq;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GitWrite.UnitTests
{
   [TestClass]
   public class CommitFileReaderTests
   {
      [TestCleanup]
      public void Cleanup()
      {
         SimpleIoc.Default.Reset();
      }

      [TestMethod]
      [ExpectedException( typeof( GitFileLoadException ) )]
      public void FromFile_FileDoesNotExist_ThrowsGitFileLoadException()
      {
         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         SimpleIoc.Default.Register( () => fileAdapterMock.Object );

         // Test

         var commitFileReader = new CommitFileReader();

         commitFileReader.FromFile( "SomeFile" );
      }

      [TestMethod]
      public void FromFile_FileExists_ReadsAllFileLines()
      {
         const string path = "SomeFile.txt";
         var contents = new[]
         {
            "# File contents"
         };

         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.Exists( path ) ).Returns( true );
         fileAdapterMock.Setup( fa => fa.ReadAllLines( path ) ).Returns( contents );
         SimpleIoc.Default.Register( () => fileAdapterMock.Object );

         // Test

         var commitFileReader = new CommitFileReader();

         commitFileReader.FromFile( path );

         // Assert

         fileAdapterMock.Verify( fa => fa.ReadAllLines( path ), Times.Once() );
      }

      [TestMethod]
      public void FromFile_FileExists_StoresTheFileLinesInTheDocument()
      {
         const string path = "SomeFile.txt";

         var lines = new[]
         {
            "LineOne",
            "LineTwo"
         };

         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.Exists( path ) ).Returns( true );
         fileAdapterMock.Setup( fa => fa.ReadAllLines( path ) ).Returns( lines );
         SimpleIoc.Default.Register( () => fileAdapterMock.Object );

         // Test

         var commitFileReader = new CommitFileReader();

         var commitDocument = commitFileReader.FromFile( path );

         // Assert

         Assert.AreEqual( lines.Length, commitDocument.RawLines.Length );
         Assert.AreEqual( lines[0], commitDocument.RawLines[0] );
         Assert.AreEqual( lines[1], commitDocument.RawLines[1] );
      }

      [TestMethod]
      public void FromFile_FileExists_StoresThePathInTheDocument()
      {
         const string path = "SomeFile.txt";
         var contents = new[]
         {
            "# Commit file"
         };

         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.Exists( path ) ).Returns( true );
         fileAdapterMock.Setup( fa => fa.ReadAllLines( path ) ).Returns( contents );
         SimpleIoc.Default.Register( () => fileAdapterMock.Object );

         // Test

         var commitFileReader = new CommitFileReader();

         var commitDocument = commitFileReader.FromFile( path );

         // Assert

         Assert.AreEqual( path, commitDocument.Name );
      }

      [TestMethod]
      [ExpectedException( typeof( GitFileLoadException ) )]
      public void FromFile_FileReturnsNullLines_ThrowsGitFileLoadException()
      {
         const string path = "COMMIT_EDITMSG";
         string[] contents = null;

         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.Exists( path ) ).Returns( true );
         fileAdapterMock.Setup( fa => fa.ReadAllLines( path ) ).Returns( contents );
         SimpleIoc.Default.Register( () => fileAdapterMock.Object );

         // Test

         var commitFileReader = new CommitFileReader();

         commitFileReader.FromFile( path );
      }

      [TestMethod]
      [ExpectedException( typeof( GitFileLoadException ) )]
      public void FromFile_FileReturnsZeroLines_ThrowsGitFileLoadException()
      {
         const string path = "COMMIT_EDITMSG";
         var contents = new string[0];

         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.Exists( path ) ).Returns( true );
         fileAdapterMock.Setup( fa => fa.ReadAllLines( path ) ).Returns( contents );
         SimpleIoc.Default.Register( () => fileAdapterMock.Object );

         // Test

         var commitFileReader = new CommitFileReader();

         commitFileReader.FromFile( path );
      }

      [TestMethod]
      public void FromFile_FileContainsOnlyComments_DoesNotSetExistingNotes()
      {
         const string path = "COMMIT_EDITMSG";
         var contents = new[]
         {
            "",
            "# Please enter the commit message for your changes. Lines starting",
            "# with '#' will be ignored, and an empty message aborts the commit.",
            "# On branch feature/supportAmend",
            "# Changes to be committed:",
            "#	modified:   src/GitWrite/GitWrite.UnitTests/AppControllerTests.cs",
            "#"
         };

         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.Exists( path ) ).Returns( true );
         fileAdapterMock.Setup( fa => fa.ReadAllLines( path ) ).Returns( contents );
         SimpleIoc.Default.Register( () => fileAdapterMock.Object );

         // Test

         var commitFileReader = new CommitFileReader();

         var commitDocument = commitFileReader.FromFile( path );

         // Assert

         Assert.IsNull( commitDocument.ShortMessage );
         Assert.IsNull( commitDocument.LongMessage );
      }

      [TestMethod]
      public void FromFile_FileHasShortMessage_SetsShortMessage()
      {
         const string path = "COMMIT_EDITMSG";
         const string shortMessage = "+Whatever static class";
         var contents = new[]
         {
            shortMessage,
            "",
            "# Please enter the commit message for your changes. Lines starting",
            "# with '#' will be ignored, and an empty message aborts the commit.",
            "# On branch feature/supportAmend",
            "# Changes to be committed:",
            "#	modified:   src/GitWrite/GitWrite.UnitTests/AppControllerTests.cs",
            "#"
         };

         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.Exists( path ) ).Returns( true );
         fileAdapterMock.Setup( fa => fa.ReadAllLines( path ) ).Returns( contents );
         SimpleIoc.Default.Register( () => fileAdapterMock.Object );

         // Test

         var commitFileReader = new CommitFileReader();

         var commitDocument = commitFileReader.FromFile( path );

         // Assert

         Assert.AreEqual( shortMessage, commitDocument.ShortMessage );
      }

      [TestMethod]
      public void FromFile_FileHasShortMessageWithLeadingLineBreak_SetsShortMessage()
      {
         const string path = "COMMIT_EDITMSG";
         const string shortMessage = "+Whatever static class";
         var contents = new[]
         {
            "",
            shortMessage,
            "",
            "# Please enter the commit message for your changes. Lines starting",
            "# with '#' will be ignored, and an empty message aborts the commit.",
            "# On branch feature/supportAmend",
            "# Changes to be committed:",
            "#	modified:   src/GitWrite/GitWrite.UnitTests/AppControllerTests.cs",
            "#"
         };

         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.Exists( path ) ).Returns( true );
         fileAdapterMock.Setup( fa => fa.ReadAllLines( path ) ).Returns( contents );
         SimpleIoc.Default.Register( () => fileAdapterMock.Object );

         // Test

         var commitFileReader = new CommitFileReader();

         var commitDocument = commitFileReader.FromFile( path );

         // Assert

         Assert.AreEqual( shortMessage, commitDocument.ShortMessage );
      }

      [TestMethod]
      public void FromFile_FileHasShortMessageWithLeadingComments_SetsShortMessage()
      {
         const string path = "COMMIT_EDITMSG";
         const string shortMessage = "+Whatever static class";
         var contents = new[]
         {
            "# First line comment",
            "# Second line comment",
            "",
            shortMessage,
            "",
            "# Please enter the commit message for your changes. Lines starting",
            "# with '#' will be ignored, and an empty message aborts the commit.",
            "# On branch feature/supportAmend",
            "# Changes to be committed:",
            "#	modified:   src/GitWrite/GitWrite.UnitTests/AppControllerTests.cs",
            "#"
         };

         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.Exists( path ) ).Returns( true );
         fileAdapterMock.Setup( fa => fa.ReadAllLines( path ) ).Returns( contents );
         SimpleIoc.Default.Register( () => fileAdapterMock.Object );

         // Test

         var commitFileReader = new CommitFileReader();

         var commitDocument = commitFileReader.FromFile( path );

         // Assert

         Assert.AreEqual( shortMessage, commitDocument.ShortMessage );
      }

      [TestMethod]
      public void FromFile_FileHasLongMessageWithOneLine_SetsShortAndLongMessage()
      {
         const string path = "COMMIT_EDITMSG";
         const string shortMessage = "+Whatever static class";
         const string longMessage = "This will provide the whatever and this and that";

         var contents = new[]
         {
            shortMessage,
            longMessage,
            "",
            "# Please enter the commit message for your changes. Lines starting",
            "# with '#' will be ignored, and an empty message aborts the commit.",
            "# On branch feature/supportAmend",
            "# Changes to be committed:",
            "#	modified:   src/GitWrite/GitWrite.UnitTests/AppControllerTests.cs",
            "#"
         };

         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.Exists( path ) ).Returns( true );
         fileAdapterMock.Setup( fa => fa.ReadAllLines( path ) ).Returns( contents );
         SimpleIoc.Default.Register( () => fileAdapterMock.Object );

         // Test

         var commitFileReader = new CommitFileReader();

         var commitDocument = commitFileReader.FromFile( path );

         // Assert

         Assert.AreEqual( shortMessage, commitDocument.ShortMessage );
         Assert.AreEqual( longMessage, commitDocument.LongMessage );
      }

      [TestMethod]
      public void FromFile_FileHasLongMessageWithOneLineAndMiddleLineBreak_SetsShortAndLongMessage()
      {
         const string path = "COMMIT_EDITMSG";
         const string shortMessage = "+Whatever static class";
         const string longMessage = "This will provide the whatever and this and that";

         var contents = new[]
         {
            shortMessage,
            "",
            longMessage,
            "",
            "# Please enter the commit message for your changes. Lines starting",
            "# with '#' will be ignored, and an empty message aborts the commit.",
            "# On branch feature/supportAmend",
            "# Changes to be committed:",
            "#	modified:   src/GitWrite/GitWrite.UnitTests/AppControllerTests.cs",
            "#"
         };

         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.Exists( path ) ).Returns( true );
         fileAdapterMock.Setup( fa => fa.ReadAllLines( path ) ).Returns( contents );
         SimpleIoc.Default.Register( () => fileAdapterMock.Object );

         // Test

         var commitFileReader = new CommitFileReader();

         var commitDocument = commitFileReader.FromFile( path );

         // Assert

         Assert.AreEqual( shortMessage, commitDocument.ShortMessage );
         Assert.AreEqual( longMessage, commitDocument.LongMessage );
      }

      [TestMethod]
      public void FromFile_FileHasLongMessageWithOneLineAndCommentInBetween_SetsShortAndLongMessage()
      {
         const string path = "COMMIT_EDITMSG";
         const string shortMessage = "+Whatever static class";
         const string longMessage = "This will provide the whatever and this and that";

         var contents = new[]
         {
            shortMessage,
            "",
            "# A comment between the messages, like from a conflict resolution",
            "",
            longMessage,
            "",
            "# Please enter the commit message for your changes. Lines starting",
            "# with '#' will be ignored, and an empty message aborts the commit.",
            "# On branch feature/supportAmend",
            "# Changes to be committed:",
            "#	modified:   src/GitWrite/GitWrite.UnitTests/AppControllerTests.cs",
            "#"
         };

         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.Exists( path ) ).Returns( true );
         fileAdapterMock.Setup( fa => fa.ReadAllLines( path ) ).Returns( contents );
         SimpleIoc.Default.Register( () => fileAdapterMock.Object );

         // Test

         var commitFileReader = new CommitFileReader();

         var commitDocument = commitFileReader.FromFile( path );

         // Assert

         Assert.AreEqual( shortMessage, commitDocument.ShortMessage );
         Assert.AreEqual( longMessage, commitDocument.LongMessage );
      }

      [TestMethod]
      public void FromFile_FileHasLongMessageWithTwoLines_SetsShortAndLongMessage()
      {
         const string path = "COMMIT_EDITMSG";
         const string shortMessage = "+Whatever static class";
         const string longMessage = "This will provide the whatever and this and that";
         const string longMessage2 = "and it continues onto the second line";
         string expectedLongMessage = $"{longMessage}{Environment.NewLine}{longMessage2}";

         var contents = new[]
         {
            shortMessage,
            "",
            "# A comment between the messages, like from a conflict resolution",
            "",
            longMessage,
            longMessage2,
            "",
            "# Please enter the commit message for your changes. Lines starting",
            "# with '#' will be ignored, and an empty message aborts the commit.",
            "# On branch feature/supportAmend",
            "# Changes to be committed:",
            "#	modified:   src/GitWrite/GitWrite.UnitTests/AppControllerTests.cs",
            "#"
         };

         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.Exists( path ) ).Returns( true );
         fileAdapterMock.Setup( fa => fa.ReadAllLines( path ) ).Returns( contents );
         SimpleIoc.Default.Register( () => fileAdapterMock.Object );

         // Test

         var commitFileReader = new CommitFileReader();

         var commitDocument = commitFileReader.FromFile( path );

         // Assert

         Assert.AreEqual( shortMessage, commitDocument.ShortMessage );
         Assert.AreEqual( expectedLongMessage, commitDocument.LongMessage );
      }

      [TestMethod]
      public void FromFile_FileHasLongMessageWithThreeLinesAndNoSpaceFromTheShortMessage_SetsShortAndLongMessage()
      {
         const string path = "COMMIT_EDITMSG";
         const string shortMessage = "+Whatever static class";
         const string longMessage = "This will provide the whatever and this and that";
         const string longMessage2 = "and it continues onto the second line";
         const string longMessage3 = "and it continues onto the THIRD line";
         string expectedLongMessage = $"{longMessage}{Environment.NewLine}{longMessage2}{Environment.NewLine}{longMessage3}";

         var contents = new[]
         {
            shortMessage,
            longMessage,
            longMessage2,
            longMessage3,
            "",
            "# Please enter the commit message for your changes. Lines starting",
            "# with '#' will be ignored, and an empty message aborts the commit.",
            "# On branch feature/supportAmend",
            "# Changes to be committed:",
            "#	modified:   src/GitWrite/GitWrite.UnitTests/AppControllerTests.cs",
            "#"
         };

         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.Exists( path ) ).Returns( true );
         fileAdapterMock.Setup( fa => fa.ReadAllLines( path ) ).Returns( contents );
         SimpleIoc.Default.Register( () => fileAdapterMock.Object );

         // Test

         var commitFileReader = new CommitFileReader();

         var commitDocument = commitFileReader.FromFile( path );

         // Assert

         Assert.AreEqual( shortMessage, commitDocument.ShortMessage );
         Assert.AreEqual( expectedLongMessage, commitDocument.LongMessage );
      }

      [TestMethod]
      public void FromFile_FileHasLongMessageWithFourLinesAndNoComments_SetsShortAndLongMessage()
      {
         const string path = "COMMIT_EDITMSG";
         const string shortMessage = "+Whatever static class";
         const string longMessage = "This will provide the whatever and this and that";
         const string longMessage2 = "and it continues onto the second line";
         const string longMessage3 = "and it continues onto the THIRD line";
         const string longMessage4 = "and onto the FOURTH line";
         string expectedLongMessage = $"{longMessage}{Environment.NewLine}{longMessage2}{Environment.NewLine}{longMessage3}{Environment.NewLine}{longMessage4}";

         var contents = new[]
         {
            shortMessage,
            longMessage,
            longMessage2,
            longMessage3,
            longMessage4
         };

         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.Exists( path ) ).Returns( true );
         fileAdapterMock.Setup( fa => fa.ReadAllLines( path ) ).Returns( contents );
         SimpleIoc.Default.Register( () => fileAdapterMock.Object );

         // Test

         var commitFileReader = new CommitFileReader();

         var commitDocument = commitFileReader.FromFile( path );

         // Assert

         Assert.AreEqual( shortMessage, commitDocument.ShortMessage );
         Assert.AreEqual( expectedLongMessage, commitDocument.LongMessage );
      }

      [TestMethod]
      public void FromFile_FileHasLongLongMessage_SetsShortAndLongMessage()
      {
         const string path = "COMMIT_EDITMSG";
         const string shortMessage = "+Whatever static class";
         const string longMessage = "This is an extra long commit line. It intentionally goes over 72 characters to show that it won't be split or put on additional lines.";

         var contents = new[]
         {
            shortMessage,
            longMessage,
         };

         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.Exists( path ) ).Returns( true );
         fileAdapterMock.Setup( fa => fa.ReadAllLines( path ) ).Returns( contents );
         SimpleIoc.Default.Register( () => fileAdapterMock.Object );

         // Test

         var commitFileReader = new CommitFileReader();

         var commitDocument = commitFileReader.FromFile( path );

         // Assert

         Assert.AreEqual( shortMessage, commitDocument.ShortMessage );
         Assert.AreEqual( longMessage, commitDocument.LongMessage );
      }
   }
}

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

         Assert.AreEqual( lines.Length, commitDocument.InitialLines.Length );
         Assert.AreEqual( lines[0], commitDocument.InitialLines[0] );
         Assert.AreEqual( lines[1], commitDocument.InitialLines[1] );
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
      public void FromFile_FileHasShortMessage_DoesNotSetExistingNotes()
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

   }
}

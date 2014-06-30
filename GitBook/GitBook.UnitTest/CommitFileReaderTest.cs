using GalaSoft.MvvmLight.Ioc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GitBook.UnitTest
{
   [TestClass]
   public class CommitFileReaderTest
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

         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.Exists( path ) ).Returns( true );    
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
   }
}

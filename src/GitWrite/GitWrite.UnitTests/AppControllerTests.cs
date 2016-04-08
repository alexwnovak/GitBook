using FluentAssertions;
using Moq;
using Xunit;

namespace GitWrite.UnitTests
{
   public class AppControllerTest
   {
      [Fact]
      public void Start_CommandLineArgumentsAreNull_ExitsWithCodeOne()
      {
         // Setup

         var environmentAdapterMock = new Mock<IEnvironmentAdapter>();

         // Test

         var appController = new AppController( environmentAdapterMock.Object, null );

         appController.Start( null );

         // Assert

         environmentAdapterMock.Verify( ea => ea.Exit( 1 ), Times.Once() );
      }

      [Fact]
      public void Start_CommandLineArgumentsAreEmpty_ExitsWithCodeOne()
      {
         // Setup

         var environmentAdapterMock = new Mock<IEnvironmentAdapter>();

         // Test

         var appController = new AppController( environmentAdapterMock.Object, null );

         appController.Start( new string[0] );

         // Assert

         environmentAdapterMock.Verify( ea => ea.Exit( 1 ), Times.Once() );
      }

      [Fact]
      public void Start_HasCommandLineArgument_CallsCommitFileReader()
      {
         // Setup

         var commitFileReaderMock = new Mock<ICommitFileReader>();

         // Test

         var arguments = new[]
         {
            GitFileNames.CommitFileName
         };

         var appController = new AppController( null, commitFileReaderMock.Object );

         appController.Start( arguments );

         // Assert

         commitFileReaderMock.Verify( cfr => cfr.FromFile( arguments[0] ), Times.Once() );
      }

      [Fact]
      public void Start_HasCommandLineArgument_CommitDocumentIsStoredOnApp()
      {
         var commitDocument = new CommitDocument( null );

         // Setup

         var commitFileReaderMock = new Mock<ICommitFileReader>();
         commitFileReaderMock.Setup( cfr => cfr.FromFile( It.IsAny<string>() ) ).Returns( commitDocument );    

         // Test

         var arguments = new[]
         {
            GitFileNames.CommitFileName
         };

         var appController = new AppController( null, commitFileReaderMock.Object );

         var actualCommitDocument = appController.Start( arguments );

         // Assert

         actualCommitDocument.Should().Be( commitDocument );
      }

      [Fact]
      public void Start_PassesFileThatDoesNotExist_ExitsWithCodeOne()
      {
         // Setup

         var environmentAdapterMock = new Mock<IEnvironmentAdapter>();

         var commitFileReaderMock = new Mock<ICommitFileReader>();
         commitFileReaderMock.Setup( cfr => cfr.FromFile( It.IsAny<string>() ) ).Throws<GitFileLoadException>();

         // Test

         var arguments = new[]
         {
            "Some Argument"
         };

         var appController = new AppController( environmentAdapterMock.Object, commitFileReaderMock.Object );

         appController.Start( arguments );

         // Assert

         environmentAdapterMock.Verify( ea => ea.Exit( 1 ), Times.Once() );
      }

      [Fact]
      public void Start_PassesInFileThatExistsButIsNotAGitFile_ExitsWithCodeOne()
      {
         // Setup

         var environmentAdapterMock = new Mock<IEnvironmentAdapter>();

         // Test

         var arguments = new[]
         {
            "Not a Git file"
         };

         var appController = new AppController( environmentAdapterMock.Object, null );

         appController.Start( arguments );

         // Assert

         environmentAdapterMock.Verify( ea => ea.Exit( 1 ), Times.Once() );
      }
   }
}

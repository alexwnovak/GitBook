using FluentAssertions;
using GalaSoft.MvvmLight.Ioc;
using Moq;
using Xunit;

namespace GitWrite.UnitTests
{
   public class AppControllerTest
   {
      public AppControllerTest()
      {
         SimpleIoc.Default.Reset();
      }

      [Fact]
      public void Start_CommandLineArgumentsAreNull_ExitsWithCodeOne()
      {
         // Setup

         var environmentAdapterMock = new Mock<IEnvironmentAdapter>();
         SimpleIoc.Default.Register( () => environmentAdapterMock.Object );

         // Test

         var appController = new AppController();

         appController.Start( null );

         // Assert

         environmentAdapterMock.Verify( ea => ea.Exit( 1 ), Times.Once() );
      }

      [Fact]
      public void Start_CommandLineArgumentsAreEmpty_ExitsWithCodeOne()
      {
         // Setup

         var environmentAdapterMock = new Mock<IEnvironmentAdapter>();
         SimpleIoc.Default.Register( () => environmentAdapterMock.Object );

         // Test

         var appController = new AppController();

         appController.Start( new string[0] );

         // Assert

         environmentAdapterMock.Verify( ea => ea.Exit( 1 ), Times.Once() );
      }

      [Fact]
      public void Start_HasCommandLineArgument_CallsCommitFileReader()
      {
         // Setup

         var commitFileReaderMock = new Mock<ICommitFileReader>();
         SimpleIoc.Default.Register( () => commitFileReaderMock.Object );

         // Test

         var arguments = new[]
         {
            GitFileNames.CommitFileName
         };

         var appController = new AppController();

         appController.Start( arguments );

         // Assert

         commitFileReaderMock.Verify( cfr => cfr.FromFile( arguments[0] ), Times.Once() );
      }

      [Fact]
      public void Start_HasCommandLineArgument_CommitDocumentIsStoredOnApp()
      {
         var commitDocument = new CommitDocument();

         // Setup

         var commitFileReaderMock = new Mock<ICommitFileReader>();
         commitFileReaderMock.Setup( cfr => cfr.FromFile( It.IsAny<string>() ) ).Returns( commitDocument );    
         SimpleIoc.Default.Register( () => commitFileReaderMock.Object );

         App.CommitDocument = null;

         // Test

         var arguments = new[]
         {
            GitFileNames.CommitFileName
         };

         var appController = new AppController();

         appController.Start( arguments );

         // Assert

         App.CommitDocument.Should().Be( commitDocument );
      }

      [Fact]
      public void Start_PassesFileThatDoesNotExist_ExitsWithCodeOne()
      {
         // Setup

         var environmentAdapterMock = new Mock<IEnvironmentAdapter>();
         SimpleIoc.Default.Register( () => environmentAdapterMock.Object );

         var commitFileReaderMock = new Mock<ICommitFileReader>();
         commitFileReaderMock.Setup( cfr => cfr.FromFile( It.IsAny<string>() ) ).Throws<GitFileLoadException>();
         SimpleIoc.Default.Register( () => commitFileReaderMock.Object );

         App.CommitDocument = null;

         // Test

         var arguments = new[]
         {
            "Some Argument"
         };

         var appController = new AppController();

         appController.Start( arguments );

         // Assert

         environmentAdapterMock.Verify( ea => ea.Exit( 1 ), Times.Once() );
      }

      [Fact]
      public void Start_PassesInFileThatExistsButIsNotAGitFile_ExitsWithCodeOne()
      {
         // Setup

         var environmentAdapterMock = new Mock<IEnvironmentAdapter>();
         SimpleIoc.Default.Register( () => environmentAdapterMock.Object );

         // Test

         var arguments = new[]
         {
            "Not a Git file"
         };

         var appController = new AppController();

         appController.Start( arguments );

         // Assert

         environmentAdapterMock.Verify( ea => ea.Exit( 1 ), Times.Once() );
      }
   }
}

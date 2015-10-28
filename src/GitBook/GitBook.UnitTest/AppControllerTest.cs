using GalaSoft.MvvmLight.Ioc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GitWrite.UnitTests
{
   [TestClass]
   public class AppControllerTest
   {
      [TestCleanup]
      public void Cleanup()
      {
         SimpleIoc.Default.Reset();
      }

      [TestMethod]
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

      [TestMethod]
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

      [TestMethod]
      public void Start_HasCommandLineArgument_CallsCommitFileReader()
      {
         // Setup

         var commitFileReaderMock = new Mock<ICommitFileReader>();
         SimpleIoc.Default.Register( () => commitFileReaderMock.Object );

         // Test

         var arguments = new[]
         {
            "Some Argument"
         };

         var appController = new AppController();

         appController.Start( arguments );

         // Assert

         commitFileReaderMock.Verify( cfr => cfr.FromFile( arguments[0] ), Times.Once() );
      }

      [TestMethod]
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
            "Some Argument"
         };

         var appController = new AppController();

         appController.Start( arguments );

         // Assert

         Assert.AreEqual( commitDocument, App.CommitDocument );
      }

      [TestMethod]
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
   }
}

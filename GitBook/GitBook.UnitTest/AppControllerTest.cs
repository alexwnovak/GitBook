using GalaSoft.MvvmLight.Ioc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GitBook.UnitTest
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

   }
}

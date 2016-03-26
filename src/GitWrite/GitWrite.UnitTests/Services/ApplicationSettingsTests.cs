using System.Security.Cryptography.X509Certificates;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Moq;

namespace GitWrite.UnitTests.Services
{
   [TestClass]
   public class ApplicationSettingsTests
   {
      [TestInitialize]
      public void Initialize()
      {
         SimpleIoc.Default.Reset();
      }

      [TestMethod]
      public void ThemeProperty_SetsThemeName_IsStoredWithRegistry()
      {
         const string themeName = "SomeThemeName";

         // Setup

         var registryServiceMock = new Mock<IRegistryService>();
         SimpleIoc.Default.Register( () => registryServiceMock.Object );

         // Test

         var appSettings = new ApplicationSettings
         {
            Theme = themeName
         };

         // Assert

         registryServiceMock.Verify( rs => rs.WriteString( It.IsAny<RegistryKey>(), It.IsAny<string>(), It.IsAny<string>(), themeName ), Times.Once );
      }

      [TestMethod]
      public void ThemeProperty_GetsThemeName_ReturnsStringFromRegistry()
      {
         const string themeName = "SomeThemeName";

         // Setup

         var registryServiceMock = new Mock<IRegistryService>();
         registryServiceMock.Setup( rs => rs.ReadString( It.IsAny<RegistryKey>(), It.IsAny<string>(), It.IsAny<string>() ) ).Returns( themeName );
         SimpleIoc.Default.Register( () => registryServiceMock.Object );

         // Test

         var appSettings = new ApplicationSettings();
         string actualThemeName = appSettings.Theme;

         // Assert

         Assert.AreEqual( themeName, actualThemeName );
      }

      [TestMethod]
      public void WindowXProperty_SetsHorizontalPosition_IsStoredWithRegistry()
      {
         const int x = 12345;

         // Setup

         var registryServiceMock = new Mock<IRegistryService>();
         SimpleIoc.Default.Register( () => registryServiceMock.Object );

         // Test

         var appSettings = new ApplicationSettings
         {
            WindowX = x
         };

         // Assert

         registryServiceMock.Verify( rs => rs.WriteInt( It.IsAny<RegistryKey>(), It.IsAny<string>(), It.IsAny<string>(), x ), Times.Once );
      }

      [TestMethod]
      public void WindowXProperty_GetsHorizontalPosition_ReturnsIntFromRegistry()
      {
         const int x = 54321;

         // Setup

         var registryServiceMock = new Mock<IRegistryService>();
         registryServiceMock.Setup( rs => rs.ReadInt( It.IsAny<RegistryKey>(), It.IsAny<string>(), It.IsAny<string>() ) ).Returns( x );
         SimpleIoc.Default.Register( () => registryServiceMock.Object );

         // Test

         var appSettings = new ApplicationSettings();
         int actualWindowX = appSettings.WindowX;

         // Assert

         Assert.AreEqual( x, actualWindowX );
      }
   }
}

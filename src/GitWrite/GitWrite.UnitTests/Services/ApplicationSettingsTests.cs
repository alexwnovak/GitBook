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
   }
}

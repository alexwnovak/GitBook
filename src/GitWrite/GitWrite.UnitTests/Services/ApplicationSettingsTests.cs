using Xunit;
using Moq;
using FluentAssertions;
using GitWrite.Services;

namespace GitWrite.UnitTests.Services
{
   public class ApplicationSettingsTests
   {
      [Fact]
      public void GetValue_ReadingValue_ReadsValue()
      {
         var registryServiceMock = new Mock<IRegistryService>();
         registryServiceMock.Setup( rs => rs.GetValue( ApplicationSettings.Path, "Name" ) ).Returns( 123 );

         var appSettings = new ApplicationSettings( registryServiceMock.Object );
         object value = appSettings.GetSetting( "Name" );

         value.Should().Be( 123 );
      }
   }
}

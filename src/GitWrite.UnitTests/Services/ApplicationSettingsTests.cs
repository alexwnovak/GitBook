using Xunit;
using Moq;
using FluentAssertions;
using AutoFixture.Xunit2;
using GitWrite.Services;
using GitWrite.UnitTests.Infrastructure;

namespace GitWrite.UnitTests.Services
{
   public class ApplicationSettingsTests
   {
      [Theory, AutoMoqData]
      public void GetValue_ReadingValue_ReadsValue(
         [Frozen] Mock<IRegistryService> registryServiceMock,
         ApplicationSettings sut )
      {
         registryServiceMock.Setup( rs => rs.GetValue( ApplicationSettings.Path, "Name" ) ).Returns( 123 );

         object value = sut.GetSetting( "Name" );

         value.Should().Be( 123 );
      }
   }
}

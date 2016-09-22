using FluentAssertions;
using Moq;
using Xunit;
using GitWrite.Services;
using GitWrite.Views.Converters;

namespace GitWrite.UnitTests.Views.Converters
{
   public class TextLengthInversionConverterTests
   {
      [Fact]
      public void Convert_InputIsZero_ConvertsToMaxOf72()
      {
         const int maxLength = 72;

         // Arrange

         var appSettingsMock = new Mock<IApplicationSettings>();
         appSettingsMock.SetupGet( @as => @as.MaxCommitLength ).Returns( maxLength );

         // Act

         var converter = new TextLengthInversionConverter( appSettingsMock.Object );

         int invertedLength = (int) converter.Convert( 0, null, null, null );

         // Assert

         invertedLength.Should().Be( maxLength );
      }

      [Fact]
      public void Convert_InputIsMaxOf72_ReturnsZero()
      {
         const int maxLength = 72;

         // Arrange

         var appSettingsMock = new Mock<IApplicationSettings>();
         appSettingsMock.SetupGet( @as => @as.MaxCommitLength ).Returns( maxLength );

         // Act

         var converter = new TextLengthInversionConverter( appSettingsMock.Object );

         int invertedLength = (int) converter.Convert( maxLength, null, null, null );

         // Assert

         invertedLength.Should().Be( 0 );
      }

      [Fact]
      public void Convert_InputIsNegative_ReturnsZero()
      {
         const int maxLength = 72;

         // Arrange

         var appSettingsMock = new Mock<IApplicationSettings>();
         appSettingsMock.SetupGet( @as => @as.MaxCommitLength ).Returns( maxLength );

         // Act

         var converter = new TextLengthInversionConverter( appSettingsMock.Object );

         int invertedLength = (int) converter.Convert( -1, null, null, null );

         // Assert

         invertedLength.Should().Be( 0 );
      }

      [Fact]
      public void Convert_InputIsGreaterThanMaxOf72_ReturnsZero()
      {
         const int maxLength = 72;

         // Arrange

         var appSettingsMock = new Mock<IApplicationSettings>();
         appSettingsMock.SetupGet( @as => @as.MaxCommitLength ).Returns( maxLength );

         // Act

         var converter = new TextLengthInversionConverter( appSettingsMock.Object );

         int invertedLength = (int) converter.Convert( maxLength + 1, null, null, null );

         // Assert

         invertedLength.Should().Be( 0 );
      }
   }
}

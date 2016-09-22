using Xunit;
using FluentAssertions;
using Moq;
using GitWrite.Services;
using GitWrite.Views.Converters;

namespace GitWrite.UnitTests.Views.Converters
{
   public class StringLengthToProgressConverterTests
   {
      [Fact]
      public void Convert_PassesNullValue_ReturnsZero()
      {
         // Arrange

         var appSettingsMock = new Mock<IApplicationSettings>();

         // Act

         var converter = new StringLengthToProgressConverter( appSettingsMock.Object );

         double progress = (double) converter.Convert( null, null, null, null );

         // Assert

         progress.Should().Be( 0.0 );
      }

      [Fact]
      public void Convert_PassesInEmptyString_ReturnsZero()
      {
         // Arrange

         var appSettingsMock = new Mock<IApplicationSettings>();

         // Act

         var converter = new StringLengthToProgressConverter( appSettingsMock.Object );

         double progress = (double) converter.Convert( string.Empty, null, null, null );

         // Assert

         progress.Should().Be( 0.0 );
      }

      [Fact]
      public void Convert_PassesUnparsableString_ReturnsZero()
      {
         // Arrange

         var appSettingsMock = new Mock<IApplicationSettings>();

         // Act

         var converter = new StringLengthToProgressConverter( appSettingsMock.Object );

         double progress = (double) converter.Convert( "NotAnInteger", null, null, null );

         // Assert

         progress.Should().Be( 0.0 );
      }

      [Fact]
      public void Convert_PassesStringWithZeroInteger_ReturnsProgressValue()
      {
         // Arrange

         var appSettingsMock = new Mock<IApplicationSettings>();
         appSettingsMock.SetupGet( @as => @as.MaxCommitLength ).Returns( 72 );

         // Act

         var converter = new StringLengthToProgressConverter( appSettingsMock.Object );

         // Assert

         double progress = (double) converter.Convert( "0", null, null, null );

         progress.Should().Be( 0.0 );
      }

      [Fact]
      public void Convert_PassesStringWith72Integer_ReturnsProgressValue()
      {
         const int maxLength = 72;

         // Arrange

         var appSettingsMock = new Mock<IApplicationSettings>();
         appSettingsMock.SetupGet( @as => @as.MaxCommitLength ).Returns( maxLength );

         // Act

         var converter = new StringLengthToProgressConverter( appSettingsMock.Object );

         double progress = (double) converter.Convert( maxLength.ToString(), null, null, null );

         // Assert

         progress.Should().Be( 1.0 );
      }
   }
}

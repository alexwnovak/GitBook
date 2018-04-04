using Xunit;
using FluentAssertions;
using GitWrite.Views.Converters;

namespace GitWrite.UnitTests.Views.Converters
{
   public class StringLengthToProgressConverterTests
   {
      [Fact]
      public void Convert_ValueIsNull_ReturnsZero()
      {
         var converter = new StringLengthToProgressConverter( 0 );

         double progress = (double) converter.Convert( null, null, null, null );

         progress.Should().Be( 0.0 );
      }

      [Fact]
      public void Convert_ValueIsNotAnInteger_ReturnsZero()
      {
         var converter = new StringLengthToProgressConverter( 0 );

         double progress = (double) converter.Convert( string.Empty, null, null, null );

         progress.Should().Be( 0.0 );
      }

      [Fact]
      public void Convert_ValueIsMax_ReturnsFullProgress()
      {
         var converter = new StringLengthToProgressConverter( 100 );

         double progress = (double) converter.Convert( 100, null, null, null );

         progress.Should().Be( 1.0 );
      }
   }
}

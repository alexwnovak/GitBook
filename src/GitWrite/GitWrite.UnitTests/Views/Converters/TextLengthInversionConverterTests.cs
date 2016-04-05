using FluentAssertions;
using GitWrite.Views.Converters;
using Xunit;

namespace GitWrite.UnitTests.Views.Converters
{
   public class TextLengthInversionConverterTests
   {
      [Fact]
      public void Convert_InputIsZero_ConvertsToMaxOf72()
      {
         var converter = new TextLengthInversionConverter();

         int invertedLength = (int) converter.Convert( 0, null, null, null );

         invertedLength.Should().Be( 72 );
      }

      [Fact]
      public void Convert_InputIsMaxOf72_ReturnsZero()
      {
         var converter = new TextLengthInversionConverter();

         int invertedLength = (int) converter.Convert( 72, null, null, null );

         invertedLength.Should().Be( 0 );
      }

      [Fact]
      public void Convert_InputIsNegative_ReturnsZero()
      {
         var converter = new TextLengthInversionConverter();

         int invertedLength = (int) converter.Convert( -1, null, null, null );

         invertedLength.Should().Be( 0 );
      }

      [Fact]
      public void Convert_InputIsGreaterThanMaxOf72_ReturnsZero()
      {
         var converter = new TextLengthInversionConverter();

         int invertedLength = (int) converter.Convert( 73, null, null, null );

         invertedLength.Should().Be( 0 );
      }
   }
}

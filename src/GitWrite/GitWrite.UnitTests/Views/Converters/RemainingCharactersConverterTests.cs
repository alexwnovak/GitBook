using FluentAssertions;
using Xunit;
using GitWrite.Views.Converters;

namespace GitWrite.UnitTests.Views.Converters
{
   public class RemainingCharactersConverterTests
   {
      [Fact]
      public void Convert_InputIsZero_TheMaximumRemains()
      {
         var converter = new RemainingCharactersConverter( 100 );

         int invertedLength = (int) converter.Convert( 20, null, null, null );

         invertedLength.Should().Be( 80 );
      }

      [Fact]
      public void Convert_InputIsMaximum_NoCharactersRemain()
      {
         var converter = new RemainingCharactersConverter( 100 );

         int invertedLength = (int) converter.Convert( 100, null, null, null );

         invertedLength.Should().Be( 0 );
      }

      [Fact]
      public void Convert_InputIsNegative_NoCharactersRemain()
      {
         var converter = new RemainingCharactersConverter( 100 );

         int invertedLength = (int) converter.Convert( -1, null, null, null );

         invertedLength.Should().Be( 0 );
      }

      [Fact]
      public void Convert_InputIsGreaterThanTheMaximum_NoCharactersRemain()
      {
         var converter = new RemainingCharactersConverter( 100 );

         int invertedLength = (int) converter.Convert( 101, null, null, null );

         invertedLength.Should().Be( 0 );
      }
   }
}

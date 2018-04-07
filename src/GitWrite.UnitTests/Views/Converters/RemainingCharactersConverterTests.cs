using Xunit;
using FluentAssertions;
using GitWrite.Views.Converters;

namespace GitWrite.UnitTests.Views.Converters
{
   public class RemainingCharactersConverterTests
   {
      [Fact]
      public void Convert_SomeTextExists_CalculatesTheRemainingSize()
      {
         var converter = new RemainingCharactersConverter();

         string invertedLength = (string) converter.Convert( new object[] { 100, 20 }, null, null, null );

         invertedLength.Should().Be( "80" );
      }

      [Fact]
      public void Convert_TextIsFull_NoCharactersRemain()
      {
         var converter = new RemainingCharactersConverter();

         string invertedLength = (string) converter.Convert( new object[] { 100, 100 }, null, null, null );

         invertedLength.Should().Be( "0" );
      }

      [Fact]
      public void Convert_ThereIsNoText_AllCharactersRemain()
      {
         var converter = new RemainingCharactersConverter();

         string invertedLength = (string) converter.Convert( new object[] { 100, 0 }, null, null, null );

         invertedLength.Should().Be( "100" );
      }

      [Fact]
      public void Convert_MoreTextThanAvailableSpace_CalculatesNegativeSpaceRemaining()
      {
         var converter = new RemainingCharactersConverter();

         string invertedLength = (string) converter.Convert( new object[] { 100, 101 }, null, null, null );

         invertedLength.Should().Be( "-1" );
      }
   }
}

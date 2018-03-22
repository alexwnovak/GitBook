using System;
using FluentAssertions;
using GitWrite.Views.Converters;
using Xunit;

namespace GitWrite.UnitTests.Views.Converters
{
   public class StringToStringArrayConverterTests
   {
      [Fact]
      public void Convert_ValueIsNull_ReturnsEmptyString()
      {
         var converter = new StringToStringArrayConverter();

         var convertedValue = converter.Convert( null, null, null, null );

         convertedValue.Should().Be( string.Empty );
      }

      [Fact]
      public void Convert_ValueIsNotStringArray_ReturnsEmptyString()
      {
         var converter = new StringToStringArrayConverter();

         var convertedValue = converter.Convert( 5, null, null, null );

         convertedValue.Should().Be( string.Empty );
      }

      [Fact]
      public void Convert_ValueIsEmptyStringArray_ReturnsEmptyString()
      {
         var converter = new StringToStringArrayConverter();

         var convertedValue = converter.Convert( new string[0], null, null, null );

         convertedValue.Should().Be( string.Empty );
      }

      [Fact]
      public void Convert_ValueHasOneLine_ReturnsLine()
      {
         var lines = new[]
         {
            "First line"
         };

         var converter = new StringToStringArrayConverter();

         var convertedValue = converter.Convert( lines, null, null, null );

         convertedValue.Should().Be( "First line" );
      }

      [Fact]
      public void Convert_ValueHasMultipleLines_ReturnsOneLineWithLineBreaks()
      {
         var lines = new[]
         {
            "First line",
            "Second line"
         };

         var converter = new StringToStringArrayConverter();

         var convertedValue = converter.Convert( lines, null, null, null );

         convertedValue.Should().Be( $"First line{Environment.NewLine}Second line" );
      }
   }
}

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

      [Fact]
      public void ConvertBack_ValueIsNull_ReturnsEmptyStringArray()
      {
         var converter = new StringToStringArrayConverter();

         var convertedValue = (string[]) converter.ConvertBack( null, null, null, null );

         convertedValue.Should().BeEmpty();
      }

      [Fact]
      public void ConvertBack_ValueIsNotString_ReturnsEmptyStringArray()
      {
         var converter = new StringToStringArrayConverter();

         var convertedValue = (string[]) converter.ConvertBack( 5, null, null, null );

         convertedValue.Should().BeEmpty();
      }

      [Fact]
      public void ConvertBack_ValueIsEmptyString_ReturnsEmptyStringArray()
      {
         var converter = new StringToStringArrayConverter();

         var convertedValue = (string[]) converter.ConvertBack( string.Empty, null, null, null );

         convertedValue.Should().BeEmpty();
      }

      [Fact]
      public void ConvertBack_ValueIsOneLine_ReturnsArrayWithOneElement()
      {
         var converter = new StringToStringArrayConverter();

         var convertedValue = (string[]) converter.ConvertBack( "The only line", null, null, null );

         convertedValue.Should().ContainSingle( "The only line" );
      }

      [Fact]
      public void ConvertBack_HasTwoLinesWithBlankLineInBetween_ReturnsArrayOfLines()
      {
         var converter = new StringToStringArrayConverter();

         var convertedValue = (string[]) converter.ConvertBack( $"First line{Environment.NewLine}{Environment.NewLine}Second line", null, null, null );

         convertedValue.Should().HaveCount( 3 );
         convertedValue[0].Should().Be( "First line" );
         convertedValue[1].Should().BeEmpty();
         convertedValue[2].Should().Be( "Second line" );
      }
   }
}

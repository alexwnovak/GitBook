using System.Windows;
using GitWrite.Views.Converters;
using Xunit;
using FluentAssertions;

namespace GitWrite.UnitTests.Views.Converters
{
   public class TextLengthToVisibilityConverterTests
   {
      [Fact]
      public void Convert_ValueIsNotInt_ReturnsCollapsed()
      {
         var converter = new TextLengthToVisibilityConverter();

         Visibility convertedValue = (Visibility) converter.Convert( "Not an int", null, null, null );

         convertedValue.Should().Be( Visibility.Collapsed );
      }

      [Fact]
      public void Convert_LengthIsZero_ReturnsVisible()
      {
         var converter = new TextLengthToVisibilityConverter();

         Visibility convertedValue = (Visibility) converter.Convert( 0, null, null, null );

         convertedValue.Should().Be( Visibility.Visible );
      }

      [Fact]
      public void Convert_LengthIsGreaterThanZero_ReturnsCollapsed()
      {
         var converter = new TextLengthToVisibilityConverter();

         Visibility convertedValue = (Visibility) converter.Convert( 1, null, null, null );

         convertedValue.Should().Be( Visibility.Collapsed );
      }
   }
}

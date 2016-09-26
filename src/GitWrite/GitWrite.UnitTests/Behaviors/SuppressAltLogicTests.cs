using System.Windows.Input;
using FluentAssertions;
using Xunit;
using GitWrite.Behaviors;

namespace GitWrite.UnitTests.Behaviors
{
   public class SuppressAltLogicTests
   {
      [Fact]
      public void ShouldSuppress_KeyIsLeftAlt_Suppresses()
      {
         var logicController = new SuppressAltLogic();

         bool shouldSuppress = logicController.ShouldSuppress( Key.LeftAlt );

         shouldSuppress.Should().BeTrue();
      }

      [Fact]
      public void ShouldSuppress_KeyIsRightAlt_Suppresses()
      {
         var logicController = new SuppressAltLogic();

         bool shouldSuppress = logicController.ShouldSuppress( Key.RightAlt );

         shouldSuppress.Should().BeTrue();
      }
   }
}

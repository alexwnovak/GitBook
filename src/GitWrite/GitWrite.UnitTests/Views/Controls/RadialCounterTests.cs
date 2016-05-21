using FluentAssertions;
using GitWrite.Views.Controls;
using Xunit;

namespace GitWrite.UnitTests.Views.Controls
{
   public class RadialCounterTests
   {
      [StaFact]
      public void Value_SetsValueLessThanMaximum_ValueSnapsToMinimum()
      {
         var radialCounter = new RadialCounter
         {
            Minimum = 0,
            Maximum = 100,
            Value = -20
         };

         radialCounter.Value.Should().Be( radialCounter.Minimum );
      }

      [StaFact]
      public void Value_SetsValueGreaterThanMaximum_ValueSnapsToMaximum()
      {
         var radialCounter = new RadialCounter
         {
            Minimum = 0,
            Maximum = 100,
            Value = 123
         };

         radialCounter.Value.Should().Be( radialCounter.Maximum );
      }

      [StaFact]
      public void Value_SetsValueBetweenMinimumAndMaximum_ValueIsSet()
      {
         var radialCounter = new RadialCounter
         {
            Minimum = 0,
            Maximum = 100,
            Value = 66
         };

         radialCounter.Value.Should().Be( 66 );
      }
   }
}

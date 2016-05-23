using FluentAssertions;
using GitWrite.Services;
using GitWrite.Views.Controls;
using Moq;
using Xunit;

namespace GitWrite.UnitTests.Views.Controls
{
   public class RadialCounterTests
   {
      [StaFact]
      public void Value_SetsValueLessThanMaximum_ValueSnapsToMinimum()
      {
         var resourceLocatorMock = new Mock<IResourceLocator>();

         var radialCounter = new RadialCounter( resourceLocatorMock.Object )
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
         var resourceLocatorMock = new Mock<IResourceLocator>();

         var radialCounter = new RadialCounter( resourceLocatorMock.Object )
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
         var resourceLocatorMock = new Mock<IResourceLocator>();

         var radialCounter = new RadialCounter( resourceLocatorMock.Object )
         {
            Minimum = 0,
            Maximum = 100,
            Value = 66
         };

         radialCounter.Value.Should().Be( 66 );
      }

      [StaFact]
      public void Value_MinimumBecomesGreaterThanCurrentValue_CurrentValueBecomesTheMinimum()
      {
         var resourceLocatorMock = new Mock<IResourceLocator>();

         var radialCounter = new RadialCounter( resourceLocatorMock.Object )
         {
            Minimum = 0,
            Maximum = 100,
            Value = 25
         };

         radialCounter.Minimum = 50;

         radialCounter.Value.Should().Be( radialCounter.Minimum );
      }

      [StaFact]
      public void Value_MaximumBecomesLessThanCurrentValue_CurrentValueBecomesTheMaximum()
      {
         var resourceLocatorMock = new Mock<IResourceLocator>();

         var radialCounter = new RadialCounter( resourceLocatorMock.Object )
         {
            Minimum = 0,
            Maximum = 100,
            Value = 75
         };

         radialCounter.Maximum = 50;

         radialCounter.Value.Should().Be( radialCounter.Maximum );
      }

      [StaFact]
      public void Minimum_MinimumIsSetGreaterThanMaximum_MinimumIsClampedEqualToMaximum()
      {
         var resourceLocatorMock = new Mock<IResourceLocator>();

         var radialCounter = new RadialCounter( resourceLocatorMock.Object )
         {
            Minimum = 0,
            Maximum = 100
         };

         radialCounter.Minimum = 125;

         radialCounter.Minimum.Should().Be( radialCounter.Maximum );
      }

      [StaFact]
      public void Maximum_MaximumIsSetLessThanMinimum_MaximumIsClampedEqualToMinimum()
      {
         var resourceLocatorMock = new Mock<IResourceLocator>();

         var radialCounter = new RadialCounter( resourceLocatorMock.Object )
         {
            Minimum = 25,
            Maximum = 100
         };

         radialCounter.Maximum = 0;

         radialCounter.Maximum.Should().Be( radialCounter.Minimum );
      }
   }
}

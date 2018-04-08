using System;
using System.Windows.Media;
using FluentAssertions;
using GitWrite.Services;
using GitWrite.Views;
using Moq;
using Xunit;

namespace GitWrite.UnitTests.Views
{
   public class AccentColorExtensionTests
   {
      [Fact]
      public void ProvideValue_BackgroundIsBright_GeneratesTranslucentDarkColor()
      {
         var appSettingsMock = new Mock<IApplicationSettings>();
         appSettingsMock.Setup( @as => @as.GetSetting( "BackgroundColor" ) ).Returns( "#FFF" );

         var extension = new AccentColorExtension( appSettingsMock.Object )
         {
            Opacity = 0.2
         };

         var brush = (SolidColorBrush) extension.ProvideValue( Mock.Of<IServiceProvider>() );

         brush.Color.Should().BeEquivalentTo( Color.FromArgb( 51, 0, 0, 0 ) );
      }

      [Fact]
      public void ProvideValue_BackgroundIsDark_GeneratesTranslucentLightkColor()
      {
         var appSettingsMock = new Mock<IApplicationSettings>();
         appSettingsMock.Setup( @as => @as.GetSetting( "BackgroundColor" ) ).Returns( "#000" );

         var extension = new AccentColorExtension( appSettingsMock.Object )
         {
            Opacity = 0.2
         };

         var brush = (SolidColorBrush) extension.ProvideValue( Mock.Of<IServiceProvider>() );

         brush.Color.Should().BeEquivalentTo( Color.FromArgb( 51, 255, 255, 255 ) );
      }
   }
}

using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using Xunit;
using Moq;
using FluentAssertions;
using GitWrite.Services;
using GitWrite.Views;

namespace GitWrite.UnitTests.Views
{
   public class SettingsExtensionTests
   {
      [Fact]
      public void ProvideValue_RetrievesValidColorString_IsConvertedToBrush()
      {
         var applicationSettingsMock = new Mock<IApplicationSettings>();
         applicationSettingsMock.Setup( @as => @as.GetSetting( "MainColor" ) ).Returns( "#FF0000" );

         var provideValueTargetMock = new Mock<IProvideValueTarget>();
         provideValueTargetMock.SetupGet( pvt => pvt.TargetProperty ).Returns( Panel.BackgroundProperty );
         var serviceProviderMock = new Mock<IServiceProvider>();
         serviceProviderMock.Setup( sp => sp.GetService( typeof( IProvideValueTarget ) ) ).Returns( provideValueTargetMock.Object );

         var settingsExtension = new SettingsExtension( applicationSettingsMock.Object, "MainColor" );
         var value = (SolidColorBrush) settingsExtension.ProvideValue( serviceProviderMock.Object );

         value.Color.Should().Be( Colors.Red );
      }

      [Fact]
      public void ProvideValue_RetrievesValidInteger_ReturnsInteger()
      {
         var applicationSettingsMock = new Mock<IApplicationSettings>();
         applicationSettingsMock.Setup( @as => @as.GetSetting( "MaxLength" ) ).Returns( 50 );

         var provideValueTargetMock = new Mock<IProvideValueTarget>();
         provideValueTargetMock.SetupGet( pvt => pvt.TargetProperty ).Returns( TextBox.MaxLengthProperty );
         var serviceProviderMock = new Mock<IServiceProvider>();
         serviceProviderMock.Setup( sp => sp.GetService( typeof( IProvideValueTarget ) ) ).Returns( provideValueTargetMock.Object );

         var settingsExtension = new SettingsExtension( applicationSettingsMock.Object, "MaxLength" );
         var value = (int) settingsExtension.ProvideValue( serviceProviderMock.Object );

         value.Should().Be( 50 );
      }

   }
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Xunit;
using Moq;
using FluentAssertions;
using GalaSoft.MvvmLight.Messaging;
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

         var settingsExtension = new SettingsExtension( applicationSettingsMock.Object, Mock.Of<IMessenger>(), "MainColor" );
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

         var settingsExtension = new SettingsExtension( applicationSettingsMock.Object, Mock.Of<IMessenger>(), "MaxLength" );
         var value = (int) settingsExtension.ProvideValue( serviceProviderMock.Object );

         value.Should().Be( 50 );
      }

      [Fact]
      public void RefreshSettingsMessage_RefreshingColor_SetsTheColor()
      {
         var applicationSettingsMock = new Mock<IApplicationSettings>();
         applicationSettingsMock.SetupSequence( @as => @as.GetSetting( "MainColor" ) )
            .Returns( "#0000FF" )
            .Returns( "#FF0000" );

         var messenger = new Messenger();
         var associatedObject = new DependencyObject();

         var provideValueTargetMock = new Mock<IProvideValueTarget>();
         provideValueTargetMock.SetupGet( pvt => pvt.TargetProperty ).Returns( Panel.BackgroundProperty );
         provideValueTargetMock.SetupGet( pvt => pvt.TargetObject ).Returns( associatedObject );
         var serviceProviderMock = new Mock<IServiceProvider>();
         serviceProviderMock.Setup( sp => sp.GetService( typeof( IProvideValueTarget ) ) ).Returns( provideValueTargetMock.Object );

         var settingsExtension = new SettingsExtension( applicationSettingsMock.Object, messenger, "MainColor" );
         settingsExtension.ProvideValue( serviceProviderMock.Object );

         messenger.Send( new RefreshSettingsMessage(), "MainColor" );

         var value = (SolidColorBrush) associatedObject.GetValue( Panel.BackgroundProperty );
         value.Color.Should().Be( Colors.Red );
      }
   }
}

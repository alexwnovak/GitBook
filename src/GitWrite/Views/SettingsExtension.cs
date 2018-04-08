using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GitWrite.Services;

namespace GitWrite.Views
{
   [MarkupExtensionReturnType( typeof( object ) )]
   public class SettingsExtension : MarkupExtension
   {
      private readonly IApplicationSettings _applicationSettings;
      private readonly string _name;

      private DependencyObject _targetObject;
      private DependencyProperty _targetProperty;

      public SettingsExtension( string name )
         : this( SimpleIoc.Default.GetInstance<IApplicationSettings>(), Messenger.Default, name )
      {
      }

      public SettingsExtension( IApplicationSettings applicationSettings, IMessenger messenger, string name )
      {
         _applicationSettings = applicationSettings;
         _name = name;

         messenger.Register<RefreshSettingsMessage>( this, name, _ => OnRefreshSettings(), true );
      }

      private object GetSettingValue()
      {
         object value = _applicationSettings.GetSetting( _name );

         if ( value.GetType() == _targetProperty.PropertyType )
         {
            return value;
         }

         var valueConverter = TypeDescriptor.GetConverter( _targetProperty.PropertyType );
         return valueConverter.ConvertFrom( value );
      }

      private void OnRefreshSettings()
      {
         var settingValue = GetSettingValue();
         _targetObject.SetValue( _targetProperty, settingValue );
      }

      public override object ProvideValue( IServiceProvider serviceProvider )
      {
         var valueTarget = (IProvideValueTarget) serviceProvider.GetService( typeof( IProvideValueTarget ) );
         _targetObject = (DependencyObject) valueTarget.TargetObject;
         _targetProperty = (DependencyProperty) valueTarget.TargetProperty;

         return GetSettingValue();
      }
   }
}

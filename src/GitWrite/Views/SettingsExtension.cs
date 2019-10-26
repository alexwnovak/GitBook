using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using GitWrite.Services;
using GitWrite.Views.Converters;

namespace GitWrite.Views
{
   [MarkupExtensionReturnType( typeof( object ) )]
   public class SettingsExtension : MarkupExtension
   {
      private readonly IApplicationSettings _applicationSettings;

      private DependencyObject _targetObject;
      private DependencyProperty _targetProperty;

      public string Name { get; set; }
      public IValueConverter Converter { get; set; } = DefaultConverter.Instance;

      public SettingsExtension( string name )
         //: this( SimpleIoc.Default.GetInstance<IApplicationSettings>(), Messenger.Default, name )
      {
      }

      internal SettingsExtension( IApplicationSettings applicationSettings,
         //IMessenger messenger,
         string name )
      {
         _applicationSettings = applicationSettings;
         Name = name;

         //messenger.Register<RefreshSettingsMessage>( this, name, _ => OnRefreshSettings(), true );
      }

      private object GetSettingValue()
      {
         object value = _applicationSettings.GetSetting( Name );

         if ( value.GetType() == _targetProperty.PropertyType )
         {
            return Converter.Convert( value, _targetProperty.PropertyType, null, CultureInfo.DefaultThreadCurrentUICulture );
         }

         var valueConverter = TypeDescriptor.GetConverter( _targetProperty.PropertyType );
         var parsedValue = valueConverter.ConvertFrom( value );

         return Converter.Convert( parsedValue, _targetProperty.PropertyType, null, CultureInfo.DefaultThreadCurrentUICulture );
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

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Services;

namespace GitWrite.Views
{
   public class SettingsExtension : MarkupExtension
   {
      private readonly IApplicationSettings _applicationSettings;
      private readonly string _name;

      public SettingsExtension( string name )
         : this( SimpleIoc.Default.GetInstance<IApplicationSettings>() , name )
      {
      }

      public SettingsExtension( IApplicationSettings applicationSettings, string name )
      {
         _applicationSettings = applicationSettings;
         _name = name;
      }

      public override object ProvideValue( IServiceProvider serviceProvider )
      {
         var valueTarget = (IProvideValueTarget) serviceProvider.GetService( typeof( IProvideValueTarget ) );
         var targetProperty = (DependencyProperty) valueTarget.TargetProperty;

         object value = _applicationSettings.GetSetting( _name );

         if ( value.GetType() == targetProperty.PropertyType )
         {
            return value;
         }

         var valueConverter = TypeDescriptor.GetConverter( targetProperty.PropertyType );
         return valueConverter.ConvertFrom( value );
      }
   }
}

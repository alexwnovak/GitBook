using System;
using System.Windows.Markup;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Services;

namespace GitWrite.Views
{
   public class SettingsExtension : MarkupExtension
   {
      private readonly IApplicationSettings _applicationSettings = SimpleIoc.Default.GetInstance<IApplicationSettings>();
      private readonly string _name;

      public SettingsExtension( string name )
      {
         _name = name;
      }

      public override object ProvideValue( IServiceProvider serviceProvider ) =>
         _applicationSettings.GetSetting( _name );
   }
}

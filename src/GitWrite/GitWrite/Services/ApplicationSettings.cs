using System;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Win32;

namespace GitWrite.Services
{
   public class ApplicationSettings : IApplicationSettings
   {
      private const string _path = @"SOFTWARE\GitWrite";
      private readonly Lazy<IRegistryService> _registryLazy = new Lazy<IRegistryService>( () => SimpleIoc.Default.GetInstance<IRegistryService>() );
      private IRegistryService RegistryService => _registryLazy.Value;

      public string Theme
      {
         get
         {
            return RegistryService.ReadString( Registry.CurrentUser, _path, nameof( Theme ) );
         }
         set
         {
            RegistryService.WriteString( Registry.CurrentUser, _path, nameof( Theme ), value );
         }
      }
   }
}

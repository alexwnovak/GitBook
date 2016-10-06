using Microsoft.Win32;
using GalaSoft.MvvmLight.Ioc;

namespace GitWrite.Services
{
   public class ApplicationSettings : IApplicationSettings
   {
      private const string _path = @"SOFTWARE\GitWrite";
      private readonly IRegistryService _registryService;

      public string Theme
      {
         get
         {
            var savedTheme = _registryService.ReadString( Registry.CurrentUser, _path, nameof( Theme ) );
            return string.IsNullOrEmpty( savedTheme ) ? "Default" : savedTheme;
         }
         set
         {
            _registryService.WriteString( Registry.CurrentUser, _path, nameof( Theme ), value );
         }
      }

      public int WindowX
      {
         get
         {
            return _registryService.ReadInt( Registry.CurrentUser, _path, nameof( WindowX ) );
         }
         set
         {
            _registryService.WriteInt( Registry.CurrentUser, _path, nameof( WindowX ), value );
         }
      }

      public int WindowY
      {
         get
         {
            return _registryService.ReadInt( Registry.CurrentUser, _path, nameof( WindowY ) );
         }
         set
         {
            _registryService.WriteInt( Registry.CurrentUser, _path, nameof( WindowY ), value );
         }
      }

      public bool PlaySoundOnLaunch
      {
         get
         {
            return _registryService.ReadBool( Registry.CurrentUser, _path, nameof( PlaySoundOnLaunch ) );
         }
         set
         {
            _registryService.WriteBool( Registry.CurrentUser, _path, nameof( PlaySoundOnLaunch ), value );
         }
      }

      public int MaxCommitLength
      {
         get
         {
            int storedValue = _registryService.ReadInt( Registry.CurrentUser, _path, nameof( MaxCommitLength ) );

            return storedValue == 0 ? 72 : storedValue;
         }
         set
         {
            _registryService.WriteInt( Registry.CurrentUser, _path, nameof( MaxCommitLength ), value );
         }
      }

      public ApplicationSettings()
      {
         _registryService = SimpleIoc.Default.GetInstance<IRegistryService>();
      }

      public ApplicationSettings( IRegistryService registryService )
      {
         _registryService = registryService;
      }
   }
}

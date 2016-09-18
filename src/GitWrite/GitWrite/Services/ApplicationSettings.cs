using Microsoft.Win32;

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
            return _registryService.ReadString( Registry.CurrentUser, _path, nameof( Theme ) );
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

      public ApplicationSettings( IRegistryService registryService )
      {
         _registryService = registryService;
      }
   }
}

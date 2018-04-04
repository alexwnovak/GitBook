namespace GitWrite.Services
{
   public class ApplicationSettings : IApplicationSettings
   {
      public const string Path = @"SOFTWARE\GitWrite";
      private readonly IRegistryService _registryService;

      public ApplicationSettings( IRegistryService registryService ) => _registryService = registryService;

      public object GetSetting( string name ) => _registryService.GetValue( Path, name );
   }
}

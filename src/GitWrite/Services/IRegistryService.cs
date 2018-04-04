namespace GitWrite.Services
{
   public interface IRegistryService
   {
      object GetValue( string path, string name );
   }
}

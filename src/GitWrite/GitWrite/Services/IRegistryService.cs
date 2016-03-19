namespace GitWrite.Services
{
   public interface IRegistryService
   {
      string GetTheme();

      void SetTheme( string name );
   }
}

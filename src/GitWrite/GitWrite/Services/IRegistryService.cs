namespace GitWrite.Services
{
   public interface IRegistryService
   {
      string GetTheme();

      void SetTheme( string name );

      void SetWindowX( int x );
      void SetWindowY( int y );

      int GetWindowX();
      int GetWindowY();
   }
}

using Microsoft.Win32;

namespace GitWrite.Services
{
   public interface IRegistryService
   {
      string ReadString( RegistryKey registryKey, string path, string name );
      void WriteString( RegistryKey registryKey, string path, string name, string value );

      int ReadInt( RegistryKey registryKey, string path, string name );
      void WriteInt( RegistryKey registryKey, string path, string name, int value );

      string GetTheme();

      void SetTheme( string name );

      void SetWindowX( int x );
      void SetWindowY( int y );

      int GetWindowX();
      int GetWindowY();
   }
}

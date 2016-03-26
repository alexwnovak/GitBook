using System;
using Microsoft.Win32;

namespace GitWrite.Services
{
   public class RegistryService : IRegistryService
   {
      public string ReadString( RegistryKey registryKey, string path, string name )
         => OpenKey( registryKey, path, k => (string) k.GetValue( name ) );

      public void WriteString( RegistryKey registryKey, string path, string name, string value )
         => OpenKey( registryKey, path, k => k.SetValue( name, value ) );

      public int ReadInt( RegistryKey registryKey, string path, string name )
         => OpenKey( registryKey, path, k => (int) k.GetValue( name ) );

      public void WriteInt( RegistryKey registryKey, string path, string name, int value )
         => OpenKey( registryKey, path, k => k.SetValue( name, value ) );

      private static T OpenKey<T>( RegistryKey registryKey, string path, Func<RegistryKey, T> readAction )
      {
         using ( var key = registryKey.CreateSubKey( path ) )
         {
            return readAction( key );
         }
      }

      private static void OpenKey( RegistryKey registryKey, string path, Action<RegistryKey> writeAction )
      {
         using ( var key = registryKey.CreateSubKey( path ) )
         {
            writeAction( key );
         }
      }
   }
}

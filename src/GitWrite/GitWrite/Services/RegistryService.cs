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

      public string GetTheme()
      {
         using ( var gitWriteKey = Registry.CurrentUser.CreateSubKey( @"SOFTWARE\GitWrite" ) )
         {
            string theme = (string) gitWriteKey.GetValue( "Theme" );

            if ( string.IsNullOrEmpty( theme ) )
            {
               SetTheme( "Default" );
               return "Default";
            }

            return theme;
         }
      }

      public void SetTheme( string name )
      {
         using ( var gitWriteKey = Registry.CurrentUser.CreateSubKey( @"SOFTWARE\GitWrite" ) )
         {
            gitWriteKey.SetValue( "Theme", name );
         }
      }

      public void SetWindowX( int x )
      {
         using ( var gitWriteKey = Registry.CurrentUser.CreateSubKey( @"SOFTWARE\GitWrite" ) )
         {
            gitWriteKey.SetValue( "WindowX", x );
         }
      }

      public void SetWindowY( int y )
      {
         using ( var gitWriteKey = Registry.CurrentUser.CreateSubKey( @"SOFTWARE\GitWrite" ) )
         {
            gitWriteKey.SetValue( "WindowY", y );
         }
      }

      public int GetWindowX()
      {
         using ( var gitWriteKey = Registry.CurrentUser.CreateSubKey( @"SOFTWARE\GitWrite" ) )
         {
            return (int) gitWriteKey.GetValue( "WindowX" );
         }
      }

      public int GetWindowY()
      {
         using ( var gitWriteKey = Registry.CurrentUser.CreateSubKey( @"SOFTWARE\GitWrite" ) )
         {
            return (int) gitWriteKey.GetValue( "WindowY" );
         }
      }
   }
}

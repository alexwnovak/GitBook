using Microsoft.Win32;

namespace GitWrite.Services
{
   public class RegistryService : IRegistryService
   {
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
   }
}

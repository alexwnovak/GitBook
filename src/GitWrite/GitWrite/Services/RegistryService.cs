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
   }
}

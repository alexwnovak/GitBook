using Microsoft.Win32;

namespace GitWrite.Services
{
   public class RegistryService : IRegistryService
   {
      public object GetValue( string path, string name )
      {
         using ( var key = Registry.CurrentUser.CreateSubKey( path ) )
         {
            return key.GetValue( name );
         }
      }
   }
}

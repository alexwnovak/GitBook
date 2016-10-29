using System;
using System.IO;

namespace GitWrite
{
   public static class ErrorLog
   {
      private const string _appPath = "GitWrite";
      private const string _filePath = "ErrorLog.txt";

      private static void EnsureDirectoryExists()
      {
         string localAppDataPath = Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData );
         string fullPath = Path.Combine( localAppDataPath, _appPath );

         if ( !Directory.Exists( fullPath ) )
         {
            Directory.CreateDirectory( fullPath );
         }
      }

      private static string GetFullLogPath()
      {
         string localAppDataPath = Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData );
         return Path.Combine( localAppDataPath, _appPath, _filePath );
      }

      public static void Write( string message )
      {
         EnsureDirectoryExists();

         string fullLogPath = GetFullLogPath();

         using ( var fileStream = new FileStream( fullLogPath, FileMode.Append, FileAccess.Write ) )
         {
            using ( var streamWriter = new StreamWriter( fileStream ) )
            {
               streamWriter.WriteLine( message );
            }
         }
      }
   }
}

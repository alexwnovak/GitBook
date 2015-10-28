using System.IO;

namespace GitBook
{
   public class FileAdapter : IFileAdapter
   {
      public bool Exists( string path )
      {
         return File.Exists( path );
      }

      public string[] ReadAllLines( string path )
      {
         return File.ReadAllLines( path );
      }

      public void WriteAllLines( string path, string[] lines )
      {
         File.WriteAllLines( path, lines );  
      }
   }
}

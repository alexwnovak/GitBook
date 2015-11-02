using System.IO;

namespace GitWrite
{
   public class FileAdapter : IFileAdapter
   {
      public bool Exists( string path ) => File.Exists( path );

      public string[] ReadAllLines( string path ) => File.ReadAllLines( path );

      public void WriteAllLines( string path, string[] lines ) => File.WriteAllLines( path, lines );
   }
}

using System.Collections.Generic;

namespace GitWrite
{
   public interface IFileAdapter
   {
      bool Exists( string path );

      long GetFileSize( string path );

      string[] ReadAllLines( string path );

      void WriteAllLines( string path, IEnumerable<string> lines );

      void Delete( string path );

      void Create( string path );
   }
}

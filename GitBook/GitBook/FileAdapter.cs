using System.IO;

namespace GitBook
{
   public class FileAdapter : IFileAdapter
   {
      public bool Exists( string path )
      {
         return File.Exists( path );
      }
   }
}

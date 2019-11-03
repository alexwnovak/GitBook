using System;
using System.IO;

namespace GitWrite.Tests.Internal
{
   internal class TemporaryFolder : IDisposable
   {
      public string FolderPath { get; }

      public TemporaryFolder()
      {
         FolderPath = Path.Combine( Path.GetTempPath(), $"GitWriteTests_{Guid.NewGuid()}" );
         Directory.CreateDirectory( FolderPath );
      }

      public string CreateFile( string contents )
      {
         string fullPath = Path.Combine( FolderPath, Guid.NewGuid().ToString() );
         File.WriteAllText( fullPath, contents );
         return fullPath;
      }

      public void Dispose()
      {
         Directory.Delete( FolderPath, true );
      }
   }
}

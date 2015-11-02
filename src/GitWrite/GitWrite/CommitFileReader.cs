using GalaSoft.MvvmLight.Ioc;

namespace GitWrite
{
   public class CommitFileReader : ICommitFileReader
   {
      private readonly IFileAdapter _fileAdapter = SimpleIoc.Default.GetInstance<IFileAdapter>();

      public CommitDocument FromFile( string path )
      {
         ThrowIfNotExists( path );

         var commitDocument = CreateBasicDocument( path );

         if ( commitDocument.InitialLines == null || commitDocument.InitialLines.Length == 0 )
         {
            throw new GitFileLoadException( "Incoming Git commit file is empty" );
         }

         ResolveExistingCommitMessages( commitDocument );

         return commitDocument;
      }

      private void ThrowIfNotExists( string path )
      {
         bool fileExists = _fileAdapter.Exists( path );

         if ( !fileExists )
         {
            throw new GitFileLoadException();
         }
      }

      private CommitDocument CreateBasicDocument( string path )
      {
         return new CommitDocument
         {
            InitialLines = _fileAdapter.ReadAllLines( path ),
            Name = path
         };
      }

      private static void ResolveExistingCommitMessages( CommitDocument commitDocument )
      {
         bool hasFoundShortMessage = false;

         foreach ( string line in commitDocument.InitialLines )
         {
            if ( line.StartsWith( "#" ) )
            {
               continue;
            }

            if ( !string.IsNullOrEmpty( line ) )
            {
               if ( !hasFoundShortMessage )
               {
                  hasFoundShortMessage = true;
                  commitDocument.ShortMessage = line;
               }
            }
         }
      }
   }
}

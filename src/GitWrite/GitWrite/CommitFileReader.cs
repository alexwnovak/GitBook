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
   }
}

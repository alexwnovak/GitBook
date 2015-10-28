using GalaSoft.MvvmLight.Ioc;

namespace GitWrite
{
   public class CommitFileReader : ICommitFileReader
   {
      public CommitDocument FromFile( string path )
      {
         var fileAdapter = SimpleIoc.Default.GetInstance<IFileAdapter>();

         bool fileExists = fileAdapter.Exists( path );

         if ( !fileExists )
         {
            throw new GitFileLoadException();
         }

         return new CommitDocument
         {
            InitialLines = fileAdapter.ReadAllLines( path ),
            Path = path
         };
      }
   }
}

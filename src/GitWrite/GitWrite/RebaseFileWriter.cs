using System.Linq;

namespace GitWrite
{
   public class RebaseFileWriter : IRebaseFileWriter
   {
      private readonly IFileAdapter _fileAdapter;

      public RebaseFileWriter( IFileAdapter fileAdapter )
      {
         _fileAdapter = fileAdapter;
      }

      public void Save( RebaseDocument rebaseDocument )
      {
         var allLines = rebaseDocument.RebaseItems.Select( i => $"{i.Action.ToString().ToLower()} {i.CommitHash} {i.Text}" );

         _fileAdapter.WriteAllLines( rebaseDocument.Name, allLines );
      }
   }
}

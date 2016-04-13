using System.Linq;
using GitWrite.ViewModels;

namespace GitWrite
{
   public class InteractiveRebaseDocument
   {
      private readonly IFileAdapter _fileAdapter;

      public string Name
      {
         get;
         set;
      }

      public string[] RawLines
      {
         get;
         set;
      }

      public RebaseItem[] RebaseItems
      {
         get;
         set;
      }

      public InteractiveRebaseDocument( IFileAdapter fileAdapter )
      {
         _fileAdapter = fileAdapter;
      }

      public void Save()
      {
         var allLines = RebaseItems.Select( i => $"{i.Action.ToString().ToLower()} {i.CommitHash} {i.Text}" );

         _fileAdapter.WriteAllLines( Name, allLines );
      }
   }
}

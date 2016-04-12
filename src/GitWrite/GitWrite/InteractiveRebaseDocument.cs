using GitWrite.ViewModels;

namespace GitWrite
{
   public class InteractiveRebaseDocument
   {
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

      public InteractiveRebaseDocument()
      {
         RebaseItems = new[]
         {
            new RebaseItem( "One" ), 
            new RebaseItem( "Two" ), 
            new RebaseItem( "Three" ), 
         };
      }
   }
}

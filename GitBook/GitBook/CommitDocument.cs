namespace GitBook
{
   public class CommitDocument
   {
      public string Path
      {
         get;
         set;
      }

      public string[] InitialLines
      {
         get;
         set;
      }

      public string ShortMessage
      {
         get;
         set;
      }
   }
}

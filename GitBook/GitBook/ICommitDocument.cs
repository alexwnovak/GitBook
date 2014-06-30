namespace GitBook
{
   public interface ICommitDocument
   {
      string ShortMessage
      {
         get;
         set;
      }

      void Save();
   }
}

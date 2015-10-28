namespace GitWrite
{
   public interface ICommitDocument
   {
      string ShortMessage
      {
         get;
         set;
      }

      string LongMessage
      {
         get;
         set;
      }

      void Save();
   }
}

using GalaSoft.MvvmLight.Ioc;

namespace GitBook
{
   public class CommitDocument : ICommitDocument
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

      public string LongMessage
      {
         get;
         set;
      }

      public void Save()
      {
         var lines = new[]
         {
            ShortMessage,
            string.Empty,
            LongMessage
         };

         var fileAdapter = SimpleIoc.Default.GetInstance<IFileAdapter>();

         fileAdapter.WriteAllLines( Path, lines );
      }
   }
}

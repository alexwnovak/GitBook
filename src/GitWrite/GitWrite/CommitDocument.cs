using GalaSoft.MvvmLight.Ioc;

namespace GitWrite
{
   public class CommitDocument : ICommitDocument
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

         fileAdapter.WriteAllLines( Name, lines );
      }

      public void Clear()
      {
         var fileAdapter = SimpleIoc.Default.GetInstance<IFileAdapter>();

         fileAdapter.Delete( Name );

         fileAdapter.Create( Name );
      }
   }
}

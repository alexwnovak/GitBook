namespace GitWrite
{
   public class CommitDocument : ICommitDocument
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

      public CommitDocument( IFileAdapter fileAdapter )
      {
         _fileAdapter = fileAdapter;
      }

      public void Save()
      {
         var lines = new[]
         {
            ShortMessage,
            string.Empty,
            LongMessage
         };

         _fileAdapter.WriteAllLines( Name, lines );
      }

      public void Clear()
      {
         _fileAdapter.Delete( Name );
         _fileAdapter.Create( Name );
      }
   }
}

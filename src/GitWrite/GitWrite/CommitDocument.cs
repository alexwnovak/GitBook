using System.Collections.Generic;
using System.Linq;
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

      public List<string> LongMessage
      {
         get;
         set;
      } = new List<string>();

      public void Save()
      {
         var shortMessage = new[]
         {
            ShortMessage,
            string.Empty,
         };

         var lines = shortMessage.Concat( LongMessage );

         var fileAdapter = SimpleIoc.Default.GetInstance<IFileAdapter>();

         fileAdapter.WriteAllLines( Name, lines );
      }
   }
}

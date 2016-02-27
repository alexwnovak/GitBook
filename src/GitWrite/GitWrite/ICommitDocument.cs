using System.Collections.Generic;

namespace GitWrite
{
   public interface ICommitDocument
   {
      string Name
      {
         get;
      }

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

      void Clear();
   }
}

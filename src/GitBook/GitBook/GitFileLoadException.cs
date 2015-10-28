using System;
using System.Runtime.Serialization;

namespace GitWrite
{
   [Serializable]
   public class GitFileLoadException : Exception
   {
      public GitFileLoadException()
      {
      }

      public GitFileLoadException( string message )
         : base( message )
      {
      }

      public GitFileLoadException( string message, Exception inner )
         : base( message, inner )
      {
      }

      protected GitFileLoadException( SerializationInfo info, StreamingContext context )
         : base( info, context )
      {
      }
   }
}

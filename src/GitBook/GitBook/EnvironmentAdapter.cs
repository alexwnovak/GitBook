using System;

namespace GitWrite
{
   public class EnvironmentAdapter : IEnvironmentAdapter
   {
      public void Exit( int exitCode )
      {
         Environment.Exit( exitCode );
      }
   }
}

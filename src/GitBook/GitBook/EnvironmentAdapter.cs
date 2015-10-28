using System;

namespace GitBook
{
   public class EnvironmentAdapter : IEnvironmentAdapter
   {
      public void Exit( int exitCode )
      {
         Environment.Exit( exitCode );
      }
   }
}

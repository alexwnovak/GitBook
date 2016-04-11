using System.IO;

namespace GitWrite
{
   public class AppController
   {
      private readonly IEnvironmentAdapter _environmentAdapter;

      public ApplicationMode ApplicationMode
      {
         get;
         private set;
      }

      public AppController( IEnvironmentAdapter environmentAdapter )
      {
         _environmentAdapter = environmentAdapter;
      }

      public ApplicationMode Start( string[] arguments )
      {
         if ( arguments == null || arguments.Length == 0 )
         {
            _environmentAdapter.Exit( 1 );
            return ApplicationMode.Unknown;
         }

         string fileName = Path.GetFileName( arguments[0] );
         ApplicationMode = ApplicationModeInterpreter.GetFromFileName( fileName );

         if ( ApplicationMode == ApplicationMode.Unknown )
         {
            _environmentAdapter.Exit( 1 );
            return ApplicationMode.Unknown;
         }

         return ApplicationMode;
      }
   }
}

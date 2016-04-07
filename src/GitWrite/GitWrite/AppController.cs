using System.IO;
using System.Threading.Tasks;
using GitWrite.Views;

namespace GitWrite
{
   public class AppController : IAppController
   {
      private readonly IEnvironmentAdapter _environmentAdapter;
      private readonly ICommitFileReader _commitFileReader;

      public ApplicationMode ApplicationMode
      {
         get;
         private set;
      }

      public AppController( IEnvironmentAdapter environmentAdapter, ICommitFileReader commitFileReader )
      {
         _environmentAdapter = environmentAdapter;
         _commitFileReader = commitFileReader;
      }

      public void Start( string[] arguments )
      {
         if ( arguments == null || arguments.Length == 0 )
         {
            _environmentAdapter.Exit( 1 );
            return;
         }

         string fileName = Path.GetFileName( arguments[0] );
         ApplicationMode = ApplicationModeInterpreter.GetFromFileName( fileName );

         if ( ApplicationMode == ApplicationMode.Unknown )
         {
            _environmentAdapter.Exit( 1 );
            return;
         }

         try
         {
            App.CommitDocument = _commitFileReader.FromFile( arguments[0] );
         }
         catch ( GitFileLoadException )
         {
            _environmentAdapter.Exit( 1 );
         }
      }

      public async Task ShutDownAsync( ExitReason exitReason )
      {
         
      }
   }
}

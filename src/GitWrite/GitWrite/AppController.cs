using System.IO;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Views;

namespace GitWrite
{
   public class AppController : IAppController
   {
      public ApplicationMode ApplicationMode
      {
         get;
         private set;
      }

      public void Start( string[] arguments )
      {
         if ( arguments == null || arguments.Length == 0 )
         {
            var environmentAdapter = SimpleIoc.Default.GetInstance<IEnvironmentAdapter>();

            environmentAdapter.Exit( 1 );

            return;
         }

         string fileName = Path.GetFileName( arguments[0] );
         ApplicationMode = ApplicationModeInterpreter.GetFromFileName( fileName );

         if ( ApplicationMode == ApplicationMode.Unknown )
         {
            var environmentAdapter = SimpleIoc.Default.GetInstance<IEnvironmentAdapter>();

            environmentAdapter.Exit( 1 );

            return;
         }

         var commitFileReader = SimpleIoc.Default.GetInstance<ICommitFileReader>();

         try
         {
            App.CommitDocument = commitFileReader.FromFile( arguments[0] );
         }
         catch ( GitFileLoadException )
         {
            var environmentAdapter = SimpleIoc.Default.GetInstance<IEnvironmentAdapter>();

            environmentAdapter.Exit( 1 );
         }
      }

      public async Task ShutDownAsync( ExitReason exitReason )
      {
         
      }
   }
}

using GalaSoft.MvvmLight.Ioc;

namespace GitBook
{
   public class AppController
   {
      public void Start( string[] arguments )
      {
         if ( arguments == null || arguments.Length == 0 )
         {
            var environmentAdapter = SimpleIoc.Default.GetInstance<IEnvironmentAdapter>();

            environmentAdapter.Exit( 1 );
         }
         else
         {
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
      }
   }
}

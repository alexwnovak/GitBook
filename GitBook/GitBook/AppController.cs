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

         var commitFileReader = SimpleIoc.Default.GetInstance<ICommitFileReader>();

         commitFileReader.FromFile( arguments[0] );
      }
   }
}

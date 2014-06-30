using GalaSoft.MvvmLight.Ioc;

namespace GitBook
{
   public class AppController
   {
      public void Start( string[] arguments )
      {
         if ( arguments == null )
         {
            var environmentAdapter = SimpleIoc.Default.GetInstance<IEnvironmentAdapter>();

            environmentAdapter.Exit( 1 );
         }
      }
   }
}

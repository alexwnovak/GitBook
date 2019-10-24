using System.Windows;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Messaging;

namespace GitWrite
{
   public partial class App : Application
   {
      private void Application_OnStartup( object sender, StartupEventArgs e )
      {
         ServiceLocator.SetLocatorProvider( () => SimpleIoc.Default );
         StartupUri = AppControllerFactory.GetController( e ).GetStartupUri();

         var messageBroker = new MessageBroker();
         messageBroker.Initialize();
      }
   }
}

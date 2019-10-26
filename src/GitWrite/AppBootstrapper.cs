using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Caliburn.Micro;
using GitWrite.ViewModels;

namespace GitWrite
{
   public class AppBootstrapper : BootstrapperBase
   {
      public AppBootstrapper()
      {
         Initialize();
      }

      protected override void OnStartup( object sender, StartupEventArgs e )
      {
         DisplayRootViewFor<CommitViewModel>();
      }
   }
}

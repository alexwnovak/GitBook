using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using GitModel;
using GitWrite.ViewModels;

namespace GitWrite
{
   public class AppBootstrapper : BootstrapperBase
   {
      private SimpleContainer _container = new SimpleContainer();

      public AppBootstrapper()
      {
         Initialize();
      }

      protected override object GetInstance( Type service, string key ) => _container.GetInstance( service, key );
      protected override IEnumerable<object> GetAllInstances( Type service ) => _container.GetAllInstances( service );
      protected override void BuildUp( object instance ) => _container.BuildUp( instance );

      protected override void Configure()
      {
         _container.Singleton<IWindowManager, WindowManager>();

         _container.PerRequest<CommitViewModel>();
         _container.Handler<GetCommitFilePathFunction>( c => new GetCommitFilePathFunction( () => Environment.GetCommandLineArgs().Last() ) );
         _container.Handler<ReadCommitFileFunction>( c => new ReadCommitFileFunction( filePath => new CommitFileReader().FromFile( filePath ) ) );
         _container.Handler<WriteCommitFileFunction>( c => new WriteCommitFileFunction( ( filePath, document ) => new CommitFileWriter().ToFile( filePath, document ) ) );
      }

      protected override void OnStartup( object sender, StartupEventArgs e )
      {
         DisplayRootViewFor<CommitViewModel>();
      }
   }
}

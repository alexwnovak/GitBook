using System;
using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using GitModel;
using GitWrite.Services;
using GitWrite.ViewModels;

namespace GitWrite
{
   public class CommitObjectComposer : IObjectComposer
   {
      private readonly ISimpleIoc _container;

      public CommitObjectComposer( ISimpleIoc container )
      {
         _container = container;
      }

      public ISimpleIoc Container => _container;

      public void Compose()
      {
         _container.Register<IApplicationSettings, ApplicationSettings>();
         _container.Register<ICommitFileReader, CommitFileReader>();
         _container.Register<ICommitFileWriter, CommitFileWriter>();
         _container.Register<IRebaseFileWriter, RebaseFileWriter>();
         _container.Register<IViewService, ViewService>();
         _container.Register<IRegistryService, RegistryService>();
         _container.Register<CommitViewModel>();
         _container.Register<Func<Window>>( () => () => SimpleIoc.Default.GetInstance<Window>() );
      }
   }
}

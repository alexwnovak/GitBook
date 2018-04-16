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
      public ISimpleIoc Container { get; }

      public CommitObjectComposer( ISimpleIoc container )
      {
         Container = container;
      }

      public void Compose()
      {
         Container.Register<IApplicationSettings, ApplicationSettings>();
         Container.Register<ICommitFileReader, CommitFileReader>();
         Container.Register<ICommitFileWriter, CommitFileWriter>();
         Container.Register<IRebaseFileWriter, RebaseFileWriter>();
         Container.Register<IViewService, ViewService>();
         Container.Register<IRegistryService, RegistryService>();
         Container.Register<CommitViewModel>();
         Container.Register<Func<Window>>( () => () => SimpleIoc.Default.GetInstance<Window>() );
      }
   }
}

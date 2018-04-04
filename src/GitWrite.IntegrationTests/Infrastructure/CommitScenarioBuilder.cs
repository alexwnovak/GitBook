using System;
using System.IO;
using GalaSoft.MvvmLight.Ioc;
using GitModel;
using GitWrite.Services;
using Moq;

namespace GitWrite.IntegrationTests.Infrastructure
{
   internal class CommitScenarioBuilder
   {
      private readonly CommitObjectComposer _objectComposer;
      private readonly string _filePath;

      private CommitScenarioBuilder()
      {
         _objectComposer = new CommitObjectComposer( new SimpleIoc() );
         _objectComposer.Compose();

         _filePath = Path.Combine( Path.GetTempPath(), $"GitWrite_IntegrationTest_{Guid.NewGuid()}", GitFileNames.CommitFileName );
         _objectComposer.Container.Register( () => _filePath );

         With( Mock.Of<IViewService> );
      }

      public static CommitScenarioBuilder Create() => new CommitScenarioBuilder();

      public CommitScenarioBuilder With<TClass>( Func<TClass> factory ) where TClass : class
      {
         _objectComposer.Container.Unregister<TClass>();
         _objectComposer.Container.Register( factory );
         return this;
      }

      public CommitScenario Build()
      {
         return new CommitScenario( _objectComposer, _filePath );
      }
   }
}

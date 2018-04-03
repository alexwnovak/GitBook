using System;
using System.IO;
using GitWrite.ViewModels;

namespace GitWrite.IntegrationTests.Infrastructure
{
   internal class CommitScenario : IDisposable
   {
      private readonly IObjectComposer _objectComposer;
      public string FilePath { get; }

      public CommitScenario( IObjectComposer objectComposer, string filePath )
      {
         DisposalRegistry.Instance.Register( this );

         _objectComposer = objectComposer;
         FilePath = filePath;

         StageCommitDocument( filePath );

         _objectComposer.Container.GetInstance<CommitViewModel>().InitializeCommand.Execute( null );
      }

      private static void StageCommitDocument( string filePath )
      {
         string path = Path.GetDirectoryName( filePath );
         Directory.CreateDirectory( path );
         File.Create( filePath ).Close();
      }

      public void SetSubject( string subject )
      {
         _objectComposer.Container.GetInstance<CommitViewModel>().CommitModel.Subject = subject;
      }

      public void SetBody( string[] body )
      {
         _objectComposer.Container.GetInstance<CommitViewModel>().CommitModel.Body = body;
      }

      public void AcceptChanges()
      {
         _objectComposer.Container.GetInstance<CommitViewModel>().AcceptCommand.Execute( null );
      }

      public void Dispose()
      {
         string path = Path.GetDirectoryName( FilePath );
         Directory.Delete( path, true );
      }
   }
}

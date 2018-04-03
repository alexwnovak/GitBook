using System;
using System.Collections.Generic;
using System.Threading;

namespace GitWrite.IntegrationTests.Infrastructure
{
   internal class DisposalRegistry : IDisposable
   {
      private static readonly ThreadLocal<DisposalRegistry> _instance = new ThreadLocal<DisposalRegistry>( () => new DisposalRegistry() );
      public static DisposalRegistry Instance => _instance.Value;

      private readonly List<IDisposable> _disposables = new List<IDisposable>();

      private DisposalRegistry()
      {
      }

      public void Register( IDisposable disposable )
      {
         _disposables.Add( disposable );
      }

      public void Dispose()
      {
         _disposables.ForEach( d => d.Dispose() );
      }
   }
}
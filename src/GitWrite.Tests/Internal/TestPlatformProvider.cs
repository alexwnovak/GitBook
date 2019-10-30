using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using Action = System.Action;

namespace GitWrite.Tests.Internal
{
   public class TestPlatformProvider : IPlatformProvider
   {
      public bool WasClosed { get; private set; }

      public bool InDesignMode => throw new NotImplementedException();
      public bool PropertyChangeNotificationsOnUIThread => throw new NotImplementedException();
      public void BeginOnUIThread( System.Action action ) => throw new NotImplementedException();
      public void ExecuteOnFirstLoad( object view, Action<object> handler ) => throw new NotImplementedException();
      public void ExecuteOnLayoutUpdated( object view, Action<object> handler ) => throw new NotImplementedException();
      public object GetFirstNonGeneratedView( object view ) => throw new NotImplementedException();

      public Func<CancellationToken, Task> GetViewCloseAction( object viewModel, ICollection<object> views, bool? dialogResult )
      {
         return async ct =>
         {
            if ( viewModel is IGuardClose guardClose &&
                 viewModel is IDeactivate deactivate )
            {
               bool canClose = await guardClose.CanCloseAsync( ct );

               if ( canClose )
               {
                  await deactivate.DeactivateAsync( true, ct );
                  WasClosed = true;
               }
            }
         };
      }

      public void OnUIThread( Action action ) => action();
      public Task OnUIThreadAsync( Func<Task> action ) => action();
   }
}

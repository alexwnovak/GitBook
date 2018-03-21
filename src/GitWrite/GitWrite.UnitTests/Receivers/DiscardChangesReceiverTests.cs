using GitWrite.Messages;
using GitWrite.Receivers;
using GitWrite.UnitTests.Internal;
using Xunit;

namespace GitWrite.UnitTests.Receivers
{
   public class DiscardChangesReceiverTests
   {
      [Fact]
      public void OnReceive_ChangesAreDiscarded_ExitsApplication()
      {
         var receiver = new DiscardChangesReceiver();

         var message = new DiscardChangesMessage();
         var harness = new ReceiverHarness<DiscardChangesReceiver>( receiver );
         harness.Send( message );

         harness.VerifySend<ExitApplicationMessage>();
      }
   }
}

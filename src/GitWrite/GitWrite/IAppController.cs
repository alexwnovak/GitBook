using System.Threading.Tasks;
using GitWrite.Views;

namespace GitWrite
{
   public interface IAppController
   {
      Task ShutDownAsync( ExitReason exitReason );
   }
}

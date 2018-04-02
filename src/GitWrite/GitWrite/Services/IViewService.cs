using System.Threading.Tasks;

namespace GitWrite.Services
{
   public interface IViewService
   {
      Task CloseViewAsync( bool acceptChanges );
      void DisplaySubjectHint();
      ExitReason ConfirmDiscard();
   }
}

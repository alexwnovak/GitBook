using GitWrite.ViewModels;

namespace GitWrite.Services
{
   public interface IViewService
   {
      ExitReason ConfirmExit();
   }
}

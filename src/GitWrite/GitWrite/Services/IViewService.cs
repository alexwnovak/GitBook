using System.Threading.Tasks;
using GitWrite.Views.Controls;

namespace GitWrite.Services
{
   public interface IViewService
   {
      Task<ConfirmationResult> ConfirmExitAsync();
   }
}

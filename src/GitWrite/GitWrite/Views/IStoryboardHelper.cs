using System.Threading.Tasks;

namespace GitWrite.Views
{
   public interface IStoryboardHelper
   {
      Task PlayAsync( string name );
   }
}

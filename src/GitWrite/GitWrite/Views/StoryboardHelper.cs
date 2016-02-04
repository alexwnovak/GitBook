using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace GitWrite.Views
{
   public class StoryboardHelper : IStoryboardHelper
   {
      public Task PlayAsync( string name )
      {
         var source = new TaskCompletionSource<bool>();

         var storyboard = Application.Current.MainWindow.Resources[name] as Storyboard;

         if ( storyboard == null )
         {
            return Task.FromResult( true );
         }

         storyboard.Completed += ( sender, args ) => source.SetResult( true );
         storyboard.Begin();

         return source.Task;
      }
   }
}

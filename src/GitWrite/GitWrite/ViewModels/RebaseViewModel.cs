using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GitWrite.Services;

namespace GitWrite.ViewModels
{
   public class RebaseViewModel : GitWriteViewModelBase
   {
      private readonly RebaseDocument _document;

      public ObservableCollection<RebaseItem> Items 
      {
         get;
      }

      public string Title => "Rebasing";

      public RebaseViewModel( IViewService viewService, IAppService appService, RebaseDocument document )
         : base ( viewService, appService )
      {
         _document = document;

         Items = new ObservableCollection<RebaseItem>( document.RebaseItems );
      }

      public void SwapItems( int indexOne, int indexTwo )
      {
         var tempItem = Items[indexOne];
         Items.RemoveAt( indexOne );
         Items.Insert( indexTwo, tempItem );
      }

      protected override Task<bool> OnSaveAsync()
      {
         _document.RebaseItems = Items.ToArray();
         _document.Save();

         return Task.FromResult( true );
      }
   }
}

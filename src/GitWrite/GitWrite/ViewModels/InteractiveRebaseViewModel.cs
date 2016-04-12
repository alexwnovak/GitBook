using System.Collections.ObjectModel;
using GitWrite.Services;

namespace GitWrite.ViewModels
{
   public class InteractiveRebaseViewModel : GitWriteViewModelBase
   {
      private readonly InteractiveRebaseDocument _document;

      public ObservableCollection<RebaseItem> Items 
      {
         get;
      }

      public InteractiveRebaseViewModel( IViewService viewService, IAppService appService, InteractiveRebaseDocument document )
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
   }
}

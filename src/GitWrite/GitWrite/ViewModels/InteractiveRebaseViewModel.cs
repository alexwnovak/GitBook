using System.Collections.ObjectModel;
using GitWrite.Services;

namespace GitWrite.ViewModels
{
   public class InteractiveRebaseViewModel : GitWriteViewModelBase
   {
      public ObservableCollection<RebaseItem> Items 
      {
         get;
      }

      public InteractiveRebaseViewModel( IViewService viewService, IAppService appService )
         : base ( viewService, appService )
      {
         Items = new ObservableCollection<RebaseItem>
         {
            new RebaseItem( "One" ),
            new RebaseItem( "Two" ),
            new RebaseItem( "Three" )
         };
      }

      public void SwapItems( int indexOne, int indexTwo )
      {
         var tempItem = Items[indexOne];
         Items.RemoveAt( indexOne );
         Items.Insert( indexTwo, tempItem );
      }
   }
}

using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace GitWrite.ViewModels
{
   public class InteractiveRebaseViewModel : ViewModelBase
   {
      public ObservableCollection<RebaseItem> Items 
      {
         get;
      }

      public InteractiveRebaseViewModel()
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

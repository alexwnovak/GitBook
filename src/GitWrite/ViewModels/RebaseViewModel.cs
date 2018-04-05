using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GitModel;
using GitWrite.Services;

namespace GitWrite.ViewModels
{
   public class RebaseViewModel : ViewModelBase
   {
      private readonly IRebaseFileWriter _rebaseFileWriter;
      private readonly RebaseDocument _document;

      public ObservableCollection<RebaseItem> Items 
      {
         get;
      }

      public string Title => "Rebasing";

      public RebaseViewModel( IViewService viewService,
         IRebaseFileWriter rebaseFileWriter, 
         RebaseDocument document,
         IMessenger messenger )
         : base( messenger )
      {
         _rebaseFileWriter = rebaseFileWriter;
         _document = document;

         Items = new ObservableCollection<RebaseItem>( document.Items );
      }

      public void SwapItems( int indexOne, int indexTwo )
      {
         var tempItem = Items[indexOne];
         Items.RemoveAt( indexOne );
         Items.Insert( indexTwo, tempItem );
      }

      protected Task<bool> OnSaveAsync()
      {
         _document.Items = Items.ToArray();
         _rebaseFileWriter.ToFile( "TempFile", _document );

         return Task.FromResult( true );
      }
   }
}

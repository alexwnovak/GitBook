using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GitWrite.Services;

namespace GitWrite.ViewModels
{
   public class GitWriteViewModelBase : ViewModelBase
   {
      protected IViewService ViewService
      {
         get;
      }

      public bool IsDirty
      {
         get;
         set;
      }

      private bool _isExiting;
      public bool IsExiting
      {
         get => _isExiting;
         set
         {
            Set( () => IsExiting, ref _isExiting, value );
         }
      }

      public RelayCommand PasteCommand
      {
         get;
         protected internal set;
      }

      public GitWriteViewModelBase( IViewService viewService, IMessenger messenger )
         : base( messenger )
      {
         ViewService = viewService;
      }
   }
}

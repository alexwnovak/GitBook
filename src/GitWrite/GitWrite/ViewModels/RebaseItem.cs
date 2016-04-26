using GalaSoft.MvvmLight;

namespace GitWrite.ViewModels
{
   public class RebaseItem : ObservableObject
   {
      public string Text
      {
         get;
         set;
      }

      public string CommitHash
      {
         get;
         set;
      }

      private bool _isEditing;
      public bool IsEditing
      {
         get
         {
            return _isEditing;
         }
         set
         {
            Set( () => IsEditing, ref _isEditing, value );
         }
      }

      private RebaseItemAction _action;
      public RebaseItemAction Action
      {
         get
         {
            return _action;
         }
         set
         {
            Set( () => Action, ref _action, value );
         }
      }

      public RebaseItem( string text )
      {
         Text = text;
      }

      public override string ToString() => Text;
   }
}

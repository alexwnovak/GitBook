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

      private RebaseItemAction _action;
      public RebaseItemAction Action
      {
         get => _action;
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

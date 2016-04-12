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

      public RebaseItemAction Action
      {
         get;
         set;
      }

      public RebaseItem( string text )
      {
         Text = text;
      }

      public override string ToString() => Text;
   }
}

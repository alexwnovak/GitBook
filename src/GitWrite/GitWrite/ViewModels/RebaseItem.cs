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

      public RebaseItem( string text )
      {
         Text = text;
      }

      public override string ToString() => Text;
   }
}

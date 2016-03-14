using GalaSoft.MvvmLight;

namespace GitWrite.ViewModels
{
   public class RebaseItem : ObservableObject
   {
      private double _offsetY;
      public double OffsetY
      {
         get
         {
            return _offsetY;
         }
         set
         {
            Set( () => OffsetY, ref _offsetY, value );
         }
      }

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

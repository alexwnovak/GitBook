using System;
using GitWrite.ViewModels;

namespace GitWrite
{
   public static class RebaseItemActionExtensions
   {
      public static RebaseItemAction PreviousValue( this RebaseItemAction itemAction )
      {
         int numValues = Enum.GetValues( itemAction.GetType() ).Length;

         int thisValue = ((int) itemAction) - 1;

         return thisValue < 0 ? (RebaseItemAction) (thisValue + numValues) : (RebaseItemAction) thisValue;
      }

      public static RebaseItemAction NextValue( this RebaseItemAction itemAction )
      {
         int numValues = Enum.GetValues( itemAction.GetType() ).Length;

         int thisValue = (int) itemAction;

         return (RebaseItemAction) ( ++thisValue % numValues );
      }
   }
}

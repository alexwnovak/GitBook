using System;
using GitModel;

namespace GitWrite
{
   public static class RebaseItemActionExtensions
   {
      public static RebaseAction PreviousValue( this RebaseAction itemAction )
      {
         int numValues = Enum.GetValues( itemAction.GetType() ).Length;

         int thisValue = ((int) itemAction) - 1;

         return thisValue < 0 ? (RebaseAction) (thisValue + numValues) : (RebaseAction) thisValue;
      }

      public static RebaseAction NextValue( this RebaseAction itemAction )
      {
         int numValues = Enum.GetValues( itemAction.GetType() ).Length;

         int thisValue = (int) itemAction;

         return (RebaseAction) ( ++thisValue % numValues );
      }
   }
}

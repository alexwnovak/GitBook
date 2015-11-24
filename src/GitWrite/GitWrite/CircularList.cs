using System;

namespace GitWrite
{
   public class CircularList<T>
   {
      public void Add( T item )
      {
         throw new NotImplementedException();
      }

      #region ListNode implementation

      private class ListNode<TDataType>
      {
         public TDataType Value
         {
            get;
         }

         public ListNode<TDataType> Next
         {
            get;
            set;
         }

         public ListNode<TDataType> Previous
         {
            get;
            set;
         }

         public ListNode()
         {
         }

         public ListNode( TDataType value )
         {
            Value = value;
         }

         public override string ToString() => Value.ToString();
      }

      #endregion
   }
}

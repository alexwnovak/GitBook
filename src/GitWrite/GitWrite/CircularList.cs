namespace GitWrite
{
   public class CircularList<T>
   {
      public int Count
      {
         get;
         private set;
      }

      public T this[int index] => GetElement( index );

      public void Add( T item )
      {
         Count++;
      }

      private T GetElement( int index )
      {
         throw new System.NotImplementedException();
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

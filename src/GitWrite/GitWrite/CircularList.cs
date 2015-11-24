using System;

namespace GitWrite
{
   public class CircularList<T>
   {
      private readonly ListNode<T> _headNode = new ListNode<T>();
      private readonly ListNode<T> _tailNode = new ListNode<T>();

      public int Count
      {
         get;
         private set;
      }

      public T this[int index] => GetElement( index );

      public CircularList()
      {
         _headNode.Next = _tailNode;
         _tailNode.Previous = _headNode;
      }

      public void Add( T item )
      {
         var newNode = new ListNode<T>( item );

         _tailNode.Previous.Next = newNode;
         _tailNode.Previous = newNode;

         Count++;
      }

      private T GetElement( int index )
      {
         if ( Count == 0 )
         {
            throw new InvalidOperationException( "CircularList contains no elements" );
         }

         ListNode<T> node = _headNode.Next;

         for ( int counter = 0; counter < index; counter++ )
         {
            node = node.Next;
         }

         return node.Value;
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

using System;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;
using GalaSoft.MvvmLight.Messaging;

namespace GitWrite.Views
{
   public class EventToMessage : TriggerAction<DependencyObject>
   {
      public static readonly DependencyProperty MessageTypeProperty = DependencyProperty.Register( nameof( MessageType ),
         typeof( Type ),
         typeof( EventToMessage ) );

      public Type MessageType
      {
         get
         {
            return (Type) GetValue( MessageTypeProperty );
         }
         set
         {
            SetValue( MessageTypeProperty, value );
         }
      }

      protected override void Invoke( object parameter )
      {
         object messageInstance = Activator.CreateInstance( MessageType );

         var sendMethods = typeof( IMessenger ).GetMethods().Where( m => m.Name == "Send" );
         var sendMethod = sendMethods.First();

         var closedSendMethod = sendMethod.MakeGenericMethod( MessageType );
         closedSendMethod.Invoke( Messenger.Default, new[] { messageInstance } );
      }
   }
}
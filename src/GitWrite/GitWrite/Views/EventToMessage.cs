using System;
using System.Linq;
using System.Reflection;
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
         get => (Type) GetValue( MessageTypeProperty );
         set => SetValue( MessageTypeProperty, value );
      }

      public static readonly DependencyProperty ParameterProperty = DependencyProperty.Register( nameof( Parameter ),
         typeof( object ),
         typeof( EventToMessage ) );

      public object Parameter
      {
         get => GetValue( ParameterProperty );
         set => SetValue( ParameterProperty, value );
      }

      protected override void Invoke( object parameter )
      {
         object messageInstance;

         if ( Parameter != null )
         {
            messageInstance = ActivateWithParameter();
         }
         else
         {
            messageInstance = Activator.CreateInstance( MessageType );
         }

         var sendMethods = typeof( IMessenger ).GetMethods().Where( m => m.Name == "Send" );
         var sendMethod = sendMethods.First();

         var closedSendMethod = sendMethod.MakeGenericMethod( MessageType );
         closedSendMethod.Invoke( Messenger.Default, new[] { messageInstance } );
      }

      private MessageBase ActivateWithParameter()
      {
         var constructors = MessageType.GetConstructors( BindingFlags.Public | BindingFlags.Instance );

         foreach ( var c in constructors )
         {
            var p = c.GetParameters();

            if ( p.Length == 1 && p[0].ParameterType == Parameter.GetType() )
            {
               return (MessageBase) c.Invoke( new[] { Parameter } );
            }
         }

         throw new InvalidOperationException();
      }
   }
}
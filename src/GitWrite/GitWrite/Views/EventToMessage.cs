using System;
using System.Collections.ObjectModel;
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

      public static readonly DependencyProperty ParametersProperty = DependencyProperty.Register( nameof( Parameters ),
         typeof( ObservableCollection<object> ),
         typeof( EventToMessage ),
         new FrameworkPropertyMetadata( new ObservableCollection<object>() ) );

      public ObservableCollection<object> Parameters
      {
         get => (ObservableCollection<object>) GetValue( ParametersProperty );
         set => SetValue( ParametersProperty, value );
      }

      protected override void Invoke( object parameter )
      {
         object messageInstance;

         if ( Parameters != null )
         {
            messageInstance = ActivateWithParameters();
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

      private MessageBase ActivateWithParameters()
      {
         var constructors = MessageType.GetConstructors( BindingFlags.Public | BindingFlags.Instance );

         foreach ( var c in constructors )
         {
            var constructorParameters = c.GetParameters();

            if ( constructorParameters.Length != Parameters.Count )
            {
               continue;
            }

            for ( int index = 0; index < constructorParameters.Length; index++ )
            {
               if ( constructorParameters[index].ParameterType != Parameters[index].GetType() )
               {
                  continue;
               }
            }

            return (MessageBase) c.Invoke( Parameters.ToArray() );
         }

         throw new InvalidOperationException();
      }
   }
}
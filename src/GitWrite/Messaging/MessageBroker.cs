using System;
using System.Linq;
using System.Reflection;
using GalaSoft.MvvmLight.Messaging;

namespace GitWrite.Messaging
{
   public class MessageBroker
   {
      public void Initialize()
      {
         var exportedTypes = Assembly.GetExecutingAssembly().GetExportedTypes();

         foreach ( var exportedType in exportedTypes )
         {
            bool isReceiver = exportedType.BaseType != null &&
                              exportedType.BaseType.IsGenericType &&
                              exportedType.BaseType.GetGenericTypeDefinition() == typeof( MessageReceiver<> );

            if ( isReceiver )
            {
               RegisterReceiver( exportedType );
            }
         }
      }

      private void RegisterReceiver( Type receiverType )
      {
         var receiverInstance = (IMessageReceiver) Activator.CreateInstance( receiverType );
         var messageType = receiverType.BaseType.GetGenericArguments().First();

         var registerMethod = typeof( Messenger ).GetMethods().First( m => m.Name == "Register" );
         var closedRegisterMethod = registerMethod.MakeGenericMethod( messageType );

         var parameters = new object[]
         {
            receiverInstance,
            new Action<MessageBase>( receiverInstance.Receive ),
            true
         };

         closedRegisterMethod.Invoke( Messenger.Default, parameters );
      }
   }
}

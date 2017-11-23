using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GitWrite.Receivers;

namespace GitWrite
{
   public class ReceiverManager
   {
      private readonly List<object> _receivers = new List<object>();

      public void Initialize()
      {
         var thisAssembly = Assembly.GetExecutingAssembly();

         var allReceivers = thisAssembly.GetTypes().Where( t => t.BaseType != null
            && t.BaseType.IsGenericType
            && t.BaseType?.GetGenericTypeDefinition() == typeof( MessageReceiver<> ) );

         foreach ( var receiver in allReceivers )
         {
            var receiverInstance = Activator.CreateInstance(receiver);
            _receivers.Add( receiverInstance );
         }
      }
   }
}

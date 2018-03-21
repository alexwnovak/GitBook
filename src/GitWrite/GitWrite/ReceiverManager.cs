using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Receivers;

namespace GitWrite
{
   public class ReceiverManager
   {
      private readonly List<IMessageReceiver> _receivers = new List<IMessageReceiver>();

      public void Initialize()
      {
         var thisAssembly = Assembly.GetExecutingAssembly();

         var allReceivers = thisAssembly.GetTypes().Where( t => t.BaseType != null
            && t.BaseType.IsGenericType
            && t.BaseType?.GetGenericTypeDefinition() == typeof( MessageReceiver<> ) );

         foreach ( var receiver in allReceivers )
         {
            var receiverInstance = (IMessageReceiver) SimpleIoc.Default.GetInstance( receiver );

            receiverInstance.Register();

            _receivers.Add( receiverInstance );
         }
      }
   }
}

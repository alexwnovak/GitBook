using System;
using System.Windows;
using GitWrite.Messages;

namespace GitWrite.Receivers
{
   public class ExitApplicationReceiver : MessageReceiver<ExitApplicationMessage>
   {
      private readonly Window _window;

      public ExitApplicationReceiver( Window window )
      {
         _window = window;
      }

      protected override void OnReceive( ExitApplicationMessage message )
      {
         _window.Close();
      }
   }
}

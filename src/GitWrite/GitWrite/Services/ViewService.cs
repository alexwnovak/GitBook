using System;
using System.Windows;

namespace GitWrite.Services
{
   public class ViewService : IViewService
   {
      private readonly Func<Window> _windowProvider;

      public ViewService( Func<Window> windowProvider ) => _windowProvider = windowProvider;

      public void CloseView() => _windowProvider().Close();
   }
}

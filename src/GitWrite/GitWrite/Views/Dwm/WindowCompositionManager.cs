using System;
using System.Runtime.InteropServices;

namespace GitWrite.Views.Dwm
{
   internal static class WindowCompositionManager
   {
      [DllImport( "user32.dll" )]
      private static extern int SetWindowCompositionAttribute( IntPtr hwnd, ref WindowCompositionAttributeData data );

   }
}

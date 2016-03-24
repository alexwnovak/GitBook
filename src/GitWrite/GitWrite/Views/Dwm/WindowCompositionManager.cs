using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace GitWrite.Views.Dwm
{
   internal static class WindowCompositionManager
   {
      public static void EnableWindowBlur( Window window )
      {
         var windowHelper = new WindowInteropHelper( window );

         var accent = new AccentPolicy
         {
            AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND
         };

         int accentStructSize = Marshal.SizeOf( accent );

         var accentPtr = Marshal.AllocHGlobal( accentStructSize );
         Marshal.StructureToPtr( accent, accentPtr, false );

         var data = new WindowCompositionAttributeData
         {
            Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
            SizeOfData = accentStructSize,
            Data = accentPtr
         };

         SetWindowCompositionAttribute( windowHelper.Handle, ref data );

         Marshal.FreeHGlobal( accentPtr );
      }

      [DllImport( "user32.dll" )]
      private static extern int SetWindowCompositionAttribute( IntPtr hwnd, ref WindowCompositionAttributeData data );
   }
}

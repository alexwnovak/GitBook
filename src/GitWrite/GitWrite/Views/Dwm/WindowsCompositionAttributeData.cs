using System;
using System.Runtime.InteropServices;

namespace GitWrite.Views.Dwm
{
   [StructLayout( LayoutKind.Sequential )]
   internal struct WindowCompositionAttributeData
   {
      public WindowCompositionAttribute Attribute;
      public IntPtr Data;
      public int SizeOfData;
   }
}

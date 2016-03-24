using System.Runtime.InteropServices;

namespace GitWrite.Views.Dwm
{
   [StructLayout( LayoutKind.Sequential )]
   internal struct AccentPolicy
   {
      public AccentState AccentState;
      public int AccentFlags;
      public int GradientColor;
      public int AnimationId;
   }
}

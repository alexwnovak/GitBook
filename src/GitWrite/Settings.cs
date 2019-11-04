using System.Windows.Media;

namespace GitWrite
{
   public class Settings
   {
      public Color WindowTintColor { get; set; } = (Color) ColorConverter.ConvertFromString( "#EEE" );
      public double WindowTintOpacity { get; set; } = 0.7;
   }
}

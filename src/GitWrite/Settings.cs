using System.Windows.Media;

namespace GitWrite
{
   public class Settings
   {
      public Color WindowTintColor { get; set; } = (Color) ColorConverter.ConvertFromString( "#EEE" );
      public double WindowTintOpacity { get; set; } = 0.7;
      public Color SeparatorColor { get; set; } = (Color) ColorConverter.ConvertFromString( "#CCC" );
      public Color RadialColor { get; set; } = (Color) ColorConverter.ConvertFromString( "#FFCF9E" );
      public Color SubjectTextColor { get; set; } = (Color) ColorConverter.ConvertFromString( "#000" );
   }
}

using System;
using System.Windows.Markup;
using System.Windows.Media;
using GitWrite.Services;

namespace GitWrite.Views
{
   [MarkupExtensionReturnType( typeof( Brush ) )]
   public class AccentColorExtension : MarkupExtension
   {
      public double Opacity { get; set; } = 1.0;
      private readonly IApplicationSettings _appSettings;

      public AccentColorExtension()
         //: this( SimpleIoc.Default.GetInstance<IApplicationSettings>() )
      {
      }

      internal AccentColorExtension( IApplicationSettings appSettings )
      {
         _appSettings = appSettings;
      }

      public override object ProvideValue( IServiceProvider serviceProvider )
      {
         string colorString = _appSettings.GetSetting( "BackgroundColor" ).ToString();
         var color = (Color) ColorConverter.ConvertFromString( colorString );

         double r = color.R / 255.0;
         double g = color.G / 255.0;
         double b = color.B / 255.0;

         double magnitude = Math.Sqrt( r * r + g * g + b * b );
         bool isDark = magnitude < 0.5;

         Color baseColor = isDark ? Colors.White : Colors.Black;

         var modifiedColor = Color.FromArgb( (byte) ( 255 * Opacity ), baseColor.R, baseColor.G, baseColor.B );
         return new SolidColorBrush( modifiedColor );
      }
   }
}

using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace GitWrite.Effects
{
   public class TextureFadeEffect : ShaderEffect
   {
      private static readonly PixelShader _pixelShader = EffectHelper.CreateShader( "Shaders/TextureFade.ps" );

      public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty( nameof( Input ),
         typeof( TextureFadeEffect ), 0 );

      public Brush Input
      {
         get
         {
            return (Brush) GetValue( InputProperty );
         }
         set
         {
            SetValue( InputProperty, value );
         }
      }

      public static readonly DependencyProperty BlendAmountProperty = DependencyProperty.Register( nameof( BlendAmount ),
         typeof( double ),
         typeof( TextureFadeEffect ),
         new UIPropertyMetadata( 1.0, PixelShaderConstantCallback( 0 ) ) );

      public double BlendAmount
      {
         get
         {
            return (double) GetValue( BlendAmountProperty );
         }
         set
         {
            SetValue( BlendAmountProperty, value );
         }
      }

      public TextureFadeEffect()
      {
         PixelShader = _pixelShader;
         UpdateShaderValue( InputProperty );
         UpdateShaderValue( BlendAmountProperty );
      }
   }
}

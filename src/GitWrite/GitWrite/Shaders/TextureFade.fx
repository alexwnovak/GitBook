sampler2D input : register( s0 );

float blendAmount : register( c0 );

float4 main( float2 uv : TEXCOORD ) : COLOR
{
   float4 color = tex2D( input, uv );

   color.a *= blendAmount;

   return color;
}

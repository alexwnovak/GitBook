sampler2D input : register( s0 );

float4 main( float2 uv : TEXCOORD ) : COLOR
{
   return tex2D( input, uv );
}

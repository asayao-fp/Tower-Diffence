Shader "Unlit/SetPositionShader"
{
SubShader {
Pass {
GLSLPROGRAM

#ifdef VERTEX
void main()
{
gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
}
#endif

#ifdef FRAGMENT
uniform vec4 _Time;
uniform vec4 _ScreenParams;

void main()
{

vec2 position = ( gl_FragCoord.xy / _ScreenParams.xy ) - 0.5 ;
vec3 col = vec3(0.0,0.0,0.0);
float time = _Time.x * 15.0;

for(int i = 0; i < 15; i++){
float red = 0.01  / abs(length(position +  float(i) * 0.1)  - 1.0 * abs(sin(time))) ;
col.r += red;
}

col.g = abs(cos(time));
col.b = abs(sin(time));

gl_FragColor = vec4( col , 0.5 );
}
#endif

ENDGLSL
}
}
FallBack "Transparent/Cutout/Diffuse"

}

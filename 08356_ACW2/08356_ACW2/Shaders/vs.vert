#version 330
 
in vec3 a_Position;
in vec2 a_TexCoord;
in vec3 a_Normal;
in vec3 a_BiTan;
in vec3 a_Tan;

uniform mat4 WorldViewProj;
uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjecttion;
uniform vec4 uLightPosition;
uniform vec4 uEyePosition;

out VS_OUT
{
 
  mat3 TBN_Matrix;
  vec2 v_TexCoord;
  vec3 ViewDirection;
  vec3 LightDirection;
 
} vs_out;

void main()
{
    gl_Position = WorldViewProj * vec4(a_Position, 1.0);
    vs_out.v_TexCoord = a_TexCoord;

    vec4 oSurfacePosition = vec4(a_Position, 1) * uModel * uView;

    vec3 T = normalize(vec3(uModel * vec4(a_Tan,   0.0)));
    vec3 B = normalize(vec3(uModel * vec4(a_BiTan, 0.0)));
    vec3 N = normalize(vec3(uModel * vec4(a_Normal, 0.0)));

    vs_out.TBN_Matrix = inverse(transpose(mat3(T,B,N)));

    vs_out.ViewDirection *= vs_out.TBN_Matrix;
    vs_out.LightDirection *= vs_out.TBN_Matrix;

    vs_out.ViewDirection = normalize(uEyePosition.xyz - oSurfacePosition.xyz);
    vs_out.LightDirection = normalize(uLightPosition.xyz - oSurfacePosition.xyz);
    
}
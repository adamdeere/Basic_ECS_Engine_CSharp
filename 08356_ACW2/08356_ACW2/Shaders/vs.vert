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

out VS_OUT
{
  vec4 oSurfacePosition;
  mat3 TBN_Matrix;
  vec2 v_TexCoord;
  vec4 v_Normal;
 
} vs_out;

void main()
{
    gl_Position = WorldViewProj * vec4(a_Position, 1.0);
    vs_out.v_TexCoord = a_TexCoord;
    vs_out.oSurfacePosition = vec4(a_Position, 1) * uModel * uView;

    vec3 T = normalize(vec3(uModel * vec4(a_Tan,   0.0)));
    vec3 B = normalize(vec3(uModel * vec4(a_BiTan, 0.0)));
    vec3 N = normalize(vec3(uModel * vec4(a_Normal, 0.0)));

    vs_out.TBN_Matrix = mat3(T, B, N);

    //vs_out.v_Normal = vec4(normalize(a_Normal * mat3(transpose(inverse(uModel * uView)))), 1);  
    
}
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

out vec2 v_TexCoord;
out vec4 v_Normal;
out vec3 v_BiTan;
out vec3 v_Tan;

out vec4 oSurfacePosition;

out VS_OUT {
    vec3 FragPos;
    vec2 TexCoords;
    mat3 TBN;
} vs_out; 

void main()
{
    gl_Position = WorldViewProj * vec4(a_Position, 1.0);
    v_TexCoord = a_TexCoord;
    oSurfacePosition = vec4(a_Position, 1) * uModel * uView;
    v_Normal = vec4(normalize(a_Normal * mat3(transpose(inverse(uModel * uView)))), 1);  
    
}
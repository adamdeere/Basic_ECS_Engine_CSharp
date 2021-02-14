#version 330
 
in vec3 a_Position;
in vec2 a_TexCoord;
in vec3 a_Normal;
in vec3 a_biTan;
in vec3 a_Tan;
uniform mat4 WorldViewProj;

out vec2 v_TexCoord;

void main()
{
    gl_Position = WorldViewProj * vec4(a_Position, 1.0);
    v_TexCoord = a_TexCoord;
}
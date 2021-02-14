#version 330
 
in vec2 v_TexCoord;
in vec3 v_Normal;
in vec3 v_BiTan;
in vec3 v_Tan;
uniform sampler2D s_texture;

out vec4 Color;
 
void main()
{
    //Color = vec4(v_Normal, 1);
    Color = texture2D(s_texture, v_TexCoord);
}
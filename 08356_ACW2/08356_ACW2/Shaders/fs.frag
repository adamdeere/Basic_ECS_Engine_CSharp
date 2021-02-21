#version 330

uniform sampler2D s_texture;
uniform sampler2D s_HeightTexture;
uniform sampler2D s_MetalicTexture;
uniform sampler2D s_NormalTexture;
uniform sampler2D s_RoughnessTexture;





out vec4 Color;

vec4 diffuseColour = vec4 (1.2f, 0.8f,1.0f, 0.5f);
vec4 specularColour = vec4 (1.5f, 0.5f,1.5f, 0.5f);
vec4 ambient = vec4 (1,1,1,1);
float fSpecularPower = 4;

in VS_OUT 
{
    mat3 TBN_Matrix;
    vec2 v_TexCoord;
    vec3 ViewDirection;
    vec3 LightDirection;
} fs_in;  
 
void main()
{
        vec3  fvLightDirection = fs_in.LightDirection;
		vec3  fvViewDirection  = fs_in.ViewDirection;
		vec3 normal = normalize(2.0f * texture(s_NormalTexture, fs_in.v_TexCoord).rgb - 1);
		 normal.x*=3;
         normal.y*= 3;
	    vec4 baseColour = texture(s_texture, fs_in.v_TexCoord);

		float fNDotL  = dot(normal, fvLightDirection ); 
		vec3  fvReflection = normalize( ( ( 2.0 * normal ) * fNDotL ) - fvLightDirection ); 
        float fRDotV  = max( 0.0, dot( fvReflection, fvViewDirection ) );
		
		 vec4  fvTotalAmbient   = ambient * baseColour; 
		 vec4  fvTotalDiffuse   = diffuseColour * fNDotL * baseColour; 
         vec4  fvTotalSpecular  = specularColour *(pow(fRDotV, fSpecularPower));
		
		Color = fvTotalAmbient + fvTotalDiffuse + fvTotalSpecular;
	
}
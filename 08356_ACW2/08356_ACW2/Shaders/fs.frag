#version 330
 

uniform sampler2D s_texture;
uniform sampler2D s_HeightTexture;
uniform sampler2D s_MetalicTexture;
uniform sampler2D s_NormalTexture;
uniform sampler2D s_RoughnessTexture;


uniform vec3 uLightPosition;
uniform vec4 uEyePosition;


out vec4 Color;

vec4 diffuseColour = vec4 (1.2f, 0.8f,1.5f, 0.5f);
vec4 specularColour = vec4 (1.5f, 0.5f,1.5f, 0.5f);

in VS_OUT 
{
    vec4 oSurfacePosition;
    mat3 TBN_Matrix;
    vec2 v_TexCoord;
	vec4 v_Normal;
} fs_in;  
 
void main()
{
		vec3 normal = texture(s_NormalTexture, fs_in.v_TexCoord).rgb;
		normal = normal * 2.0 - 1.0;   
		normal = normalize(fs_in.TBN_Matrix * normal); 

		vec3 lightDir = fs_in.TBN_Matrix * normalize(uLightPosition - fs_in.oSurfacePosition.xyz); 
		vec3 eyeDirection = fs_in.TBN_Matrix * normalize(uEyePosition - fs_in.oSurfacePosition).xyz;

        vec3 reflectedVector = reflect(-lightDir, normal);

		float diffuseFactor = max(dot(normal, -lightDir), 1); 
		float specularFactor = pow(max(dot(reflectedVector, eyeDirection), 0), 1);
		float ambient = 1;
		
		vec4 uMat = texture(s_texture, fs_in.v_TexCoord);
		Color = (ambient * diffuseFactor  * specularFactor ) * uMat;
	
}
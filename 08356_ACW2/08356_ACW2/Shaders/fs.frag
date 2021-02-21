#version 330
 
in vec2 v_TexCoord;
in vec4 v_Normal;
in vec3 v_BiTan;
in vec3 v_Tan;
in vec4 oSurfacePosition;

uniform sampler2D s_texture;
uniform sampler2D s_HeightTexture;
uniform sampler2D s_MetalicTexture;
uniform sampler2D s_NormalTexture;
uniform sampler2D s_RoughnessTexture;


uniform vec4 uLightPosition;
uniform vec4 uEyePosition;


out vec4 Color;

vec4 diffuseColour = vec4 (1.2f, 0.8f,1.5f, 0.5f);
vec4 specularColour = vec4 (1.5f, 0.5f,1.5f, 0.5f);

 
void main()
{

		vec4 lightDir = normalize(uLightPosition - oSurfacePosition); 
		vec4 eyeDirection = normalize(uEyePosition - oSurfacePosition);
        vec4 reflectedVector = reflect(-lightDir, v_Normal);

		float diffuseFactor = max(dot(v_Normal, -lightDir), 1); 
		float specularFactor = pow(max(dot( reflectedVector, eyeDirection), 0), 1);
		float ambient = 1;
		
		vec4 uMat = texture(s_NormalTexture, v_TexCoord);
		Color = (ambient * diffuseFactor  * specularFactor ) * uMat;

     
}
#version 330
 
in vec2 v_TexCoord;
in vec3 v_Normal;
in vec3 v_BiTan;
in vec3 v_Tan;
in vec4 oSurfacePosition;
uniform sampler2D s_texture;

out vec4 Color;

vec4 diffuseColour = vec4 (1.2f, 0.8f,1.5f, 0.5f);
vec4 specularColour = vec4 (1.5f, 0.5f,1.5f, 0.5f);
vec4 uLightPosition = vec4 (1.0f, 0.8f,0.5f, 0.5f);
vec4 uEyePosition = vec4 (0.5f, 0.5f,0.5f, 0.5f);
 
void main()
{

		vec4 lightDir = normalize(uLightPosition - oSurfacePosition); 
		vec4 eyeDirection = normalize(uEyePosition - oSurfacePosition);
        vec4 reflectedVector = reflect(-lightDir, vec4(v_Normal, 1));

		float diffuseFactor = max(dot(vec4(v_Normal, 1), -lightDir), .5f); 
		float specularFactor = pow(max(dot( reflectedVector, eyeDirection), 0), .5f);
		float ambient = 2;
		
		vec4 uMat = texture(s_texture, v_TexCoord);
		Color = (ambient * diffuseFactor + specularColour * specularFactor ) * uMat;

     
}
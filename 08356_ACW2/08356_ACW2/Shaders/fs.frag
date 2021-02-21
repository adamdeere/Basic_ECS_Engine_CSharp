#version 330

uniform sampler2D s_texture;
uniform sampler2D s_HeightTexture;
uniform sampler2D s_MetalicTexture;
uniform sampler2D s_NormalTexture;
uniform sampler2D s_RoughnessTexture;
uniform sampler2D s_AOTexture;
//uniform float scale;
//uniform float bias0;

out vec4 Color;
float scale = 0.005f;
float bias0 = 0.005f;
vec4 diffuseColour = vec4 (.5f, .2f, 1.0f, 1);
vec4 specularColour = vec4 (.5f, 0.5f,1.5f, 0.5f);
vec4 ambient = vec4 (.5f,.5f,.5f,.5f);
float fSpecularPower = 1;
const float PI = 3.14159265359f;
in VS_OUT 
{
    mat3 TBN_Matrix;
    vec2 v_TexCoord;
    vec3 ViewDirection;
    vec3 LightDirection;
} fs_in;  

vec3 fresnelSchlick(float cosTheta, vec3 F0)
{
   return F0 + (1.0 - F0) * pow(max(1.0 - cosTheta, 0.0), 5.0);
}
 
float GeometrySchlickGGX(float NdotV, float roughness)
{
    float r = (roughness + 1.0);
    float k = (r*r) / 8.0;

    float num   = NdotV;
    float denom = NdotV * (1.0 - k) + k;
	
    return num / denom;
}
  
float GeometrySmith(vec3 N, vec3 V, vec3 L, float roughness)
{
    float NdotV = max(dot(N, V), 0.0);
    float NdotL = max(dot(N, L), 0.0);
    float ggx1 = GeometrySchlickGGX(NdotV, roughness);
    float ggx2 = GeometrySchlickGGX(NdotL, roughness);
	
    return ggx1 * ggx2;
}

float DistributionGGX(vec3 N, vec3 H, float roughness)
{
    float a      = roughness*roughness;
    float a2     = a*a;
    float NdotH  = max(dot(N, H), 0.0);
    float NdotH2 = NdotH*NdotH;
	
    float nom    = a2;
    float denom  = (NdotH2 * (a2 - 1.0) + 1.0);
    denom        = PI * denom * denom;
	
    return nom / denom;
}

void main()
{
        //passed in from vertex shader
        vec3  fvLightDirection = fs_in.LightDirection;
		vec3  fvViewDirection  = fs_in.ViewDirection;
		
        //texture infomation, height normal, albeido metalic and roughness
        float Height = texture2D(s_HeightTexture, fs_in.v_TexCoord).x;
		Height = scale * Height - bias0;
		vec2 TexCorrected = fs_in.v_TexCoord + Height * fvViewDirection.xy;
		vec3 normal = normalize(2.0f * texture(s_NormalTexture, TexCorrected).rgb - 1);

        vec3 baseColour = pow(texture(s_texture, TexCorrected).rgb, vec3(2.2f));
        //vec3 baseColour = texture(s_texture, TexCorrected).rgb;
        float metallic  = texture(s_MetalicTexture, fs_in.v_TexCoord).r;
        float roughness = texture(s_RoughnessTexture, fs_in.v_TexCoord).r;
        float ao        = texture(s_AOTexture, fs_in.v_TexCoord).r;
        // reflectance equation
        vec3 F0 = vec3(0.04); 
        F0 = mix(F0, baseColour, metallic);
        vec3 Lo = vec3(0.0);

        // calculate per-light radiance
        vec3 H = normalize(fvViewDirection + fvLightDirection);
        float lightDistance  = length(fvLightDirection);
        float attenuation = 1.0 / (lightDistance * lightDistance);
        vec3 radiance     = diffuseColour.xyz * attenuation;    

        // cook-torrance brdf
        float NDF = DistributionGGX(normal, H, roughness);        
        float G   = GeometrySmith(normal, fvViewDirection, fvLightDirection, roughness);      
        vec3 F    = fresnelSchlick(max(dot(H, fvViewDirection), 0.0), F0);  

        vec3 kS = F;
        vec3 kD = vec3(1.0) - kS;
        kD *= 1.0 - metallic;	  
        
        vec3 numerator    = NDF * G * F;
        float denominator = 4.0 * max(dot(normal, fvViewDirection), 0.0) * max(dot(normal, fvLightDirection), 0.0);
        vec3 specular     = numerator / max(denominator, 0.001);  
            
        // add to outgoing radiance Lo
        float NdotL = max(dot(normal, fvLightDirection), 0.0);                
        Lo += (kD * baseColour / PI + specular) * radiance * NdotL; 

       vec3 ambient = vec3(0.3) * baseColour * ao;
       vec3 color = ambient + Lo;
	
       color = color / (color + vec3(1.0));
       color = pow(color, vec3(1.0/2.2));  
       
       Color = vec4(color, 1.0);
}
#version 330 core
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D gPosition;
uniform sampler2D gNormal;
uniform sampler2D gAlbedoSpec;

uniform int type;

struct Light{
    vec3 Position;
    vec3 Color;
    float Radius;
};
const int NR_LIGHTS = 320;
uniform Light lights[NR_LIGHTS];
uniform vec3 viewPos;
uniform int lightSum;
uniform bool enableLightVolume;

const float constant  = 1.0; 
const float linear    = 0.7;
const float quadratic = 1.8;

void main()
{
    // retrieve data from G-buffer
    vec3 FragPos = texture(gPosition,TexCoords).rgb;
    vec3 Noraml = texture(gNormal,TexCoords).rgb;
    vec3 Albedo = texture(gAlbedoSpec,TexCoords).rgb;
    float Specular = texture(gAlbedoSpec,TexCoords).a;

    vec3 lighting = Albedo * 0.1; 
    vec3 viewDir =normalize(viewPos - FragPos);

    for(int i = 0; i < lightSum; ++i)
    {
        float distance = length(lights[i].Position - FragPos);

        if(enableLightVolume == false||distance < lights[i].Radius)
        {
            // diffuse 
            vec3 lightDir = normalize(lights[i].Position - FragPos);
            float attenuation = 1.0/(constant +  distance * linear + distance * distance * quadratic );
            
            vec3 diffuse = max(dot(lightDir,Noraml),0.0) * Albedo * attenuation * lights[i].Color;
            lighting += diffuse;
        }
    }

    if(type==1)
    {
        lighting = texture(gPosition,TexCoords).rgb;
    }
    else if(type==2)
    {
        lighting = texture(gNormal,TexCoords).rgb;
    }
    else if(type==3)
    {
        lighting = texture(gAlbedoSpec,TexCoords).rgb;
    }
    else if(type==4)
    {
        float specular=texture(gAlbedoSpec,TexCoords).a;
        lighting =vec3(specular,specular,specular);
    }

    FragColor = vec4(lighting,1.0);


}
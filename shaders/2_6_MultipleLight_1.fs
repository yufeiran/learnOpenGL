#version 330 core 
struct Material{
    sampler2D diffuse;
    sampler2D specular;
    sampler2D emission;
    float shininess;
};

struct Light{
    vec3 position;
    vec3 direction;
    float cutOff;
    float outerCutOff;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
};

struct DirLight{
    vec3 direction;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

struct PointLight{
    vec3 position;

    float constant;
    float linear;
    float quadratic;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
#define NR_POINT_LIGHTS 4

struct SpotLight
{
    vec3 position;
    vec3 direction;
    float cutOff;
    float outerCutOff;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
};

out vec4 FragColor;

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoords;


uniform Material material;
uniform DirLight dirLight;
uniform PointLight pointLights[NR_POINT_LIGHTS];
uniform SpotLight spotLight;


uniform float time;
uniform vec3 viewPos;

vec3 CalcDirLight(DirLight light,vec3 normal, vec3 viewDir);
vec3 CalcPointLight(PointLight light,vec3 normal,vec3 fragPos,vec3 viewDir);
vec3 CalcSpotLight(SpotLight light,vec3 normal,vec3 FragPos,vec3 viewDir);
vec3 CalcEmission(vec3 FragPos,vec3 viewDir);


void main()
{
    // properties
    vec3 norm=normalize(Normal);
    vec3 viewDir=normalize(viewPos-FragPos);

    // phase 1: Directional lighting
    vec3 result;
    result+=CalcDirLight(dirLight,norm,viewDir);
    // phase 2: Point lights
    for(int i=0;i<NR_POINT_LIGHTS;i++)
    {
        result+=CalcPointLight(pointLights[i],norm,FragPos,viewDir);
    }

    // phase 3: Spot light
    result+=CalcSpotLight(spotLight,norm,FragPos,viewDir);

    //result+=CalcEmission(FragPos,viewPos);

    FragColor=vec4(result,1.0);
}

vec3 CalcDirLight(DirLight light,vec3 normal, vec3 viewDir)
{
    vec3 lightDir=normalize(-light.direction);
    // diffuse shading
    float diff =max(dot(normal,lightDir),0.0);
    // specular shading
    vec3 reflectDir=reflect(-lightDir,normal);
    float spec=pow(max(dot(viewDir,reflectDir),0.0),material.shininess);
    // combine results
    vec3 ambient = light.ambient *vec3(texture(material.diffuse,TexCoords));
    vec3 diffuse=light.diffuse*diff*vec3(texture(material.diffuse,TexCoords));
    vec3 specular=light.specular*spec*vec3(texture(material.specular,TexCoords));
    return (ambient+diffuse+specular);
}

vec3 CalcPointLight(PointLight light,vec3 normal,vec3 fragPos,vec3 viewDir)
{
    vec3 lightDir=normalize(light.position-fragPos);
    // diffuse shading
    float diff=max(dot(normal,lightDir),0.0);
    // specular shading
    vec3 reflectDir=reflect(-lightDir,normal);
    float spec=pow(max(dot(viewDir,reflectDir),0.0),material.shininess);
    // attenuation
    float distance = length(light.position-fragPos);
    float attenuation=1.0/(light.constant+light.linear*distance+
    light.quadratic*(distance*distance));

    // combine results
    vec3 ambient = light.ambient*vec3(texture(material.diffuse,TexCoords));
    vec3 diffuse=light.diffuse*diff*vec3(texture(material.diffuse,TexCoords));
    vec3 specular=light.specular*spec*vec3(texture(material.specular,TexCoords));
    ambient *= attenuation;
    diffuse*=attenuation;
    specular*=attenuation;
    return (ambient+diffuse+specular);
}

vec3 CalcSpotLight(SpotLight light,vec3 normal,vec3 FragPos,vec3 viewDir)
{


    vec3 lightDir=normalize(light.position-FragPos);

    float theta=dot(lightDir,normalize(-light.direction));
    float epsilon=light.cutOff-light.outerCutOff;
    float intensity=clamp((theta-light.outerCutOff)/epsilon,0.0,1.0);

    // ambient shading
    vec3 ambient = light.ambient*vec3(texture(material.diffuse,TexCoords));

    // diffuse shading
    float diff=max(dot(normal,lightDir),0.0);
    // attenuation
    float distance=length(light.position-FragPos);
    float attenuation=1.0/(light.constant+light.linear*distance+light.quadratic*(distance*distance));

    vec3 result;
    if(theta>light.outerCutOff)
    {
        vec3 diffuse = light.diffuse*diff*vec3(texture(material.diffuse,TexCoords));

        // specular shading
        vec3 viewDir=normalize(viewPos-FragPos);
        vec3 reflectDir=reflect(-lightDir,normal);
        float spec=pow(max(dot(viewDir,reflectDir),0.0),material.shininess);
        vec3 specular=light.specular*spec*(vec3(texture(material.specular,TexCoords)));

        ambient*=attenuation;
        diffuse*=attenuation;
        specular*=attenuation;

        diffuse*=intensity;
        specular*=intensity;

        result=ambient+diffuse+specular;
    }
    else 
    {
        result=light.ambient*vec3(texture(material.diffuse,TexCoords))*attenuation;
    }
    return result;

}

vec3 CalcEmission(vec3 FragPos,vec3 viewDir)
{
    /* Emission */
    vec3 emission=vec3(0.0);
    float emissionDistance=length(FragPos-viewPos);
    //https://learnopengl.com/Lighting/Light-casters Choosing the right values
    // Distance =100
    float emissionAttenuation=1.0/(1.0+0.045*emissionDistance+0.0075*(emissionDistance*emissionDistance));
    if(texture(material.specular,TexCoords).r==0.0)
    {
        emission=texture(material.emission,TexCoords+vec2(0.0f,1.0f)*time).rgb*emissionAttenuation;
    }
    return emission;
}
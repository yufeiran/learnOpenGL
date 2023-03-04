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

out vec4 FragColor;

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoords;


uniform Material material;
uniform Light light;
uniform float time;
uniform vec3 viewPos;


void main()
{

    //ambient
    vec3 ambient=light.ambient* vec3(texture(material.diffuse,TexCoords));




    // diffuse
    vec3 norm=normalize(Normal);
    vec3 lightDir=normalize(light.position-FragPos);
    float diff=max(dot(norm,lightDir),0.0);

    float theta=dot(lightDir,normalize(-light.direction));
    float epsilon=light.cutOff-light.outerCutOff;
    float intensity=clamp((theta-light.outerCutOff)/epsilon,0.0,1.0);

    // attenuation
    float distance=length(light.position-FragPos);
    float attenuation=1.0/(light.constant+light.linear*distance+light.quadratic*(distance*distance));

    vec3 result;
    if(theta>light.outerCutOff)
    {
        vec3 diffuse=light.diffuse*diff*vec3(texture(material.diffuse,TexCoords));

        //specular
        vec3 viewDir=normalize(viewPos-FragPos);
        vec3 reflectDir=reflect(-lightDir,norm);
        float spec = pow(max(dot(viewDir,reflectDir),0.0),material.shininess);
        vec3 specular=light.specular*spec*( vec3(texture(material.specular,TexCoords)) );



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
    result+=emission;

    FragColor=vec4(result,1.0);
}
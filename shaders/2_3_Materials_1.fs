#version 330 core 
struct Material{
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
};

struct Light{
    vec3 position;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

out vec4 FragColor;

in vec3 Normal;
in vec3 FragPos;
in vec3 LightPos;

uniform vec3 objectColor;
uniform Material material;
uniform Light light;

//uniform vec3 viewPos;


void main()
{
    vec3 viewPos=vec3(0,0,0);
    
    vec3 ambient=light.ambient* material.ambient;


    vec3 norm=normalize(Normal);
    vec3 lightDir=normalize(LightPos-FragPos);
    
    float diff=max(dot(norm,lightDir),0.0);
    vec3 diffuse=light.diffuse*(diff*material.diffuse);

    vec3 viewDir=normalize(viewPos-FragPos);
    vec3 reflectDir=reflect(-lightDir,norm);

    float spec = pow(max(dot(viewDir,reflectDir),0.0),material.shininess);
    vec3 specular=light.specular*(spec*material.specular);
    
    vec3 result=(ambient+diffuse+specular)*objectColor;

    FragColor=vec4(result,1.0);
}
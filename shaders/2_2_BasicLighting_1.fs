#version 330 core 
out vec4 FragColor;

in vec3 Normal;
in vec3 FragPos;
in vec3 LightPos;

uniform vec3 objectColor;
uniform vec3 lightColor;

//uniform vec3 viewPos;

uniform float ambientStrength;
uniform float diffuseStrength;
uniform float specularStrength;
uniform int shininessValue;

void main()
{
    vec3 viewPos=vec3(0,0,0);
    
    vec3 ambient=ambientStrength*lightColor;


    vec3 norm=normalize(Normal);
    vec3 lightDir=normalize(LightPos-FragPos);
    
    float diff=max(dot(norm,lightDir),0.0);
    vec3 diffuse=diffuseStrength*diff*lightColor;

    vec3 viewDir=normalize(viewPos-FragPos);
    vec3 reflectDir=reflect(-lightDir,norm);

    float spec = pow(max(dot(viewDir,reflectDir),0.0),shininessValue);
    vec3 specular=specularStrength*spec*lightColor;
    
    vec3 result=(ambient+diffuse+specular)*objectColor;

    FragColor=vec4(result,1.0);
}
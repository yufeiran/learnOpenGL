#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform vec3 lightPos;
uniform vec3 lightColor;
uniform vec3 objectColor;

uniform float ambientStrength;
uniform float diffuseStrength;
uniform float specularStrength;
uniform int shininessValue;

out vec3 result;

void main()
{
    gl_Position = projection*view*model*vec4(aPos,1.0);
    vec3 FragPos=vec3(view*model*vec4(aPos,1.0));
    vec3 Normal=mat3(transpose(inverse(view*model)))*aNormal;
    vec3 LightPos=vec3(view*vec4(lightPos,1.0));

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
     result=(ambient+diffuse+specular)*objectColor;
}
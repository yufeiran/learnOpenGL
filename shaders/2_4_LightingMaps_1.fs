#version 330 core 
struct Material{
    sampler2D diffuse;
    sampler2D specular;
    sampler2D emission;
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
in vec2 TexCoords;


uniform Material material;
uniform Light light;
uniform float time;

//uniform vec3 viewPos;


void main()
{
    vec3 viewPos=vec3(0,0,0);
    
    vec3 ambient=light.ambient* vec3(texture(material.diffuse,TexCoords));


    vec3 norm=normalize(Normal);
    vec3 lightDir=normalize(LightPos-FragPos);
    
    float diff=max(dot(norm,lightDir),0.0);
    vec3 diffuse=light.diffuse*diff*vec3(texture(material.diffuse,TexCoords));

    vec3 viewDir=normalize(viewPos-FragPos);
    vec3 reflectDir=reflect(-lightDir,norm);

    float spec = pow(max(dot(viewDir,reflectDir),0.0),material.shininess);
    vec3 specular=light.specular*spec*( vec3(texture(material.specular,TexCoords)) );

    /* Emission */
    vec3 emission=vec3(0.0);
    if(texture(material.specular,TexCoords).r==0.0)
    {
        emission=texture(material.emission,TexCoords+vec2(0.0f,1.0f)*time).rgb*(0.5+0.8*(sin(time)+1.0)/2);
    }
    
    vec3 result=ambient+diffuse+specular+emission;

    FragColor=vec4(result,1.0);
}